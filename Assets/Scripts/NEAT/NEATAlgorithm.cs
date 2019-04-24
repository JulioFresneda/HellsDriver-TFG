using NeuralNet;
using System;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

namespace NEAT
{
    public enum EvolutionMode { EvolveSpeed, EvolveDriving }

    public class NEATAlgorithm : MonoBehaviour
    {

        /// <summary>
        /// NEAT Hyperparameters
        /// </summary>
        [Header("NEAT Hyperparameters")]
        [SerializeField]
        private int NumberOfCars = 100;

        [SerializeField]
        private int CompatibilityThreshold = 24;

        [SerializeField]
        private bool CrossCopy = true;

        [SerializeField]
        [Range(0, 1)]
        private double CrossProbability = 0.5;

        [SerializeField]
        private int SaveBestInGeneration = 100;

        [SerializeField]
        private bool SaveThisGeneration = false;

        [SerializeField]
        private EvolutionMode SelectEvolutionMode = EvolutionMode.EvolveDriving;


        /// <summary>
        /// Mutate hyperparameters
        /// </summary>

        [Header("Mutate Hyperparameters")]
        [SerializeField]
        [Range(0, 1)]
        private double MutateWeightsProbability = 0.1;

        [SerializeField]
        [Range(0, 1)]
        private double RandomWeightsProbabilityWhenMutate = 0.1;

        [SerializeField]
        [Range(0, 1)]
        private double MutateWeightsRange = 0.02;

        [SerializeField]
        [Range(0, 1)]
        public static double AddNeuronProbability = 0.05;

        [SerializeField]
        [Range(0, 1)]
        public static double AddConnectionProbability = 0.2;


        [Header("Reference objects")]
#pragma warning disable 0649
        [SerializeField]
        private Transform start;

        [SerializeField]
        private GameObject car;
#pragma warning restore 0649


        public static EvolutionMode evolutionMode = EvolutionMode.EvolveDriving;

        public static System.Random rnd;

        public static List<Connection> NewConnections;
        public static List<Tuple<Connection,int,int,int>> NewNeurons;

        

        private static int INNOV_CONNECTION;
        private static int INNOV_NEURON;


        

        private List<NeuralNetwork> nn_poblation;
        private List<GameObject> cars;

        private List<NeuralNetwork> nn_new_poblation;

        private List<NeuralNetwork> nn_representation_previous_gen;
        private Dictionary<int,NeuralNetwork> nn_champions;

        private bool readyforruncars, readyforevolve;


        private int generation = 0;

        private int carnumber = 0;



        // Start is called before the first frame update
        void Awake()
        {
            // Start static parameters
            evolutionMode = SelectEvolutionMode;
            Mutation.MutateWeightsProbability = MutateWeightsProbability;
            Mutation.AddConnectionProbability = AddConnectionProbability;
            Mutation.AddNeuronProbability = AddNeuronProbability;
            Mutation.RandomWeightsProbabilityWhenMutate = RandomWeightsProbabilityWhenMutate;
            Mutation.MutateWeightsRange = MutateWeightsRange;



            rnd = new System.Random(1);
            NewConnections = new List<Connection>();
            NewNeurons = new List<Tuple<Connection, int, int, int>>();

            nn_poblation = new List<NeuralNetwork>();
            nn_new_poblation = new List<NeuralNetwork>();

            nn_representation_previous_gen = new List<NeuralNetwork>();
            nn_champions = new Dictionary<int, NeuralNetwork>();

            cars = new List<GameObject>();

            readyforruncars = false;
            readyforevolve = false;



            for (int i = 0; i < NumberOfCars; i++)
            {
                nn_poblation.Add(GenerateBasicNN());
            }

            INNOV_CONNECTION = 0;
            INNOV_NEURON = 0;

            foreach(var k in nn_poblation[0].Connections().Keys)
            {
                if (k > INNOV_CONNECTION) INNOV_CONNECTION = k;
            }
            foreach(var k in nn_poblation[0].Neurons().Keys)
            {
                if (k > INNOV_NEURON) INNOV_NEURON = k;
            }
            INNOV_NEURON++;
            INNOV_CONNECTION++;


            readyforruncars = true;

        }

        private void Update()
        {
            


            if (readyforruncars) RunCars();    
            else if (readyforevolve)
            {
                var cars = GameObject.FindGameObjectsWithTag("Car");
                foreach (GameObject car in cars)
                {
                    Destroy(car);
                }
                EvolveGeneration();

                foreach (GameObject car in cars)
                {
                    car.GetComponentInParent<CarFitnessTest>().SetDoneCalculatingFitness(false);
                }
                



            }

            if (PoblationDoneCalculatingFitness())
            {
                foreach (GameObject car in cars)
                {
                    car.GetComponent<CarAI>().GetNeuralNetwork().SetFitness(car.GetComponent<CarFitnessTest>().GetFitness());
                }
                readyforevolve = true;
                readyforruncars = false;
            }


        }




        private void EvolveGeneration()
        {
            

            
            if(generation == SaveBestInGeneration || SaveThisGeneration)
            {
                CompareByFitness cmpf = new CompareByFitness();
                nn_poblation.Sort(cmpf);
                NNToFile ntf = new NNToFile(nn_poblation[nn_poblation.Count-1]);
                string n = "C:/Users/Julio/Desktop/cars/car" + generation + "_" + carnumber + ".txt";
                carnumber++;
                ntf.Write(n);
            }


            CompareByFitness cbf = new CompareByFitness();
            nn_poblation.Sort(cbf);

            Debug.Log("Generation: " + generation);
            Debug.Log("Best: " + nn_poblation[nn_poblation.Count - 1].GetFitness() + " " + nn_poblation[nn_poblation.Count - 1].lockweight + " " + nn_poblation[nn_poblation.Count - 1].throttleweight);
            double mean = 0;
            foreach(NeuralNetwork nn in nn_poblation)
            {
                mean+=nn.GetFitness();
            }
            mean = mean / nn_poblation.Count;

            Debug.Log("Mean: " + mean);

            readyforevolve = false;
            Debug.Log("Inicio Evolve " + System.GC.GetTotalMemory(true));
            Specialize();

            ObtainChampions();

            Mutate();

            Cross();

            Replace();

            

            readyforruncars = true;
            generation++;
        }




        private void RunCars()
        {
            
            readyforruncars = false;
            cars.Clear();
            foreach (NeuralNetwork n in nn_poblation)
            {
                cars.Add(Instantiate(car, start));
                cars[cars.Count - 1].GetComponent<CarAI>().SetNeuralNetwork(n);
            }
            
            
        }

        private void Specialize()
        {
            bool added;          
            foreach (NeuralNetwork n in nn_poblation)
            {
                added = false;
                for (int i = 0; i < nn_representation_previous_gen.Count && !added; i++)
                {
                    if (CompatibilityDistance(n, nn_representation_previous_gen[i]) < CompatibilityThreshold)
                    {
                        n.SetSpecie(i);
                        added = true;
                    }
                }

                if (!added)
                {
                    nn_representation_previous_gen.Add(n);
                    n.SetSpecie(nn_representation_previous_gen.Count - 1);
                }
            } 
        }


        private void ObtainChampions()
        {
            CompareBySpecieAndFitness comp = new CompareBySpecieAndFitness();
 
            // New champions
            nn_poblation.Sort(comp);
            nn_champions.Clear();
            foreach(KeyValuePair<int,List<NeuralNetwork>> specie in NNBySpecies(nn_poblation))
            {
                if (specie.Value.Count > 5)
                {
                    NeuralNetwork c = new NeuralNetwork(specie.Value[specie.Value.Count - 1]);
                    nn_champions[specie.Key] = c;
                }
            }

            
        }


    




        private void Mutate()
        {
            NewConnections.Clear();
            NewNeurons.Clear();
            foreach (NeuralNetwork n in nn_poblation)
            {
                Mutation.Mutate(n);
            }
            
        }

     

        private void Cross()
        {
            //Debug.Log("4.1 " + System.GC.GetTotalMemory(true));
            GenerateFitnessSharing();
            ////Debug.Log("4.2 " + System.GC.GetTotalMemory(true));
            List<NeuralNetwork> selected = new List<NeuralNetwork>();
            selected.AddRange(nn_poblation);
            CompareBySharedFitness comparersf = new CompareBySharedFitness();
            selected.Sort(comparersf);

            selected.RemoveRange(0, selected.Count / 2);

            nn_new_poblation.Clear();

            var dic = NNBySpecies(selected);

            if (CrossCopy)
            {
                foreach (KeyValuePair<int, List<NeuralNetwork>> s in dic)
                {
                    foreach (NeuralNetwork nn in s.Value)
                    {
                        nn_new_poblation.Add(new NeuralNetwork(nn));
                        nn_new_poblation.Add(new NeuralNetwork(nn));
                    }


                }

                CompareByFitness cbf = new CompareByFitness();
                nn_new_poblation.Sort(cbf);

                for(int i=0; i<nn_champions.Count; i++)
                {
                    nn_new_poblation[i] = new NeuralNetwork(nn_champions[i]);
                }
            }

            else
            {
                foreach (KeyValuePair<int, List<NeuralNetwork>> s in dic)
                {
                    for (int i = 0; i < s.Value.Count * 2 - 1; i++)
                    {
                        if (rnd.NextDouble() > CrossProbability) nn_new_poblation.Add(Crossover.GetCrossover(s.Value[rnd.Next(0, s.Value.Count)], s.Value[rnd.Next(0, s.Value.Count)]));
                        else nn_new_poblation.Add(new NeuralNetwork(s.Value[rnd.Next(0, s.Value.Count)]));
                    }
                    if (nn_champions.ContainsKey(s.Key))
                    {
                        nn_new_poblation.Add(new NeuralNetwork(nn_champions[s.Key]));
                    }
                    else
                    {
                        if (rnd.NextDouble() > CrossProbability) nn_new_poblation.Add(Crossover.GetCrossover(s.Value[rnd.Next(0, s.Value.Count)], s.Value[rnd.Next(0, s.Value.Count)]));
                        else nn_new_poblation.Add(new NeuralNetwork(s.Value[rnd.Next(0, s.Value.Count)]));
                    }
                }
            }
            
        }
            

        public static int GetInnovNeuron()
        {
            return INNOV_NEURON;
        }
        public static int GetInnovConnection()
        {
            return INNOV_CONNECTION;
        }
        public static int GetNewInnovNeuron()
        {
            INNOV_NEURON++;
            return INNOV_NEURON;
        }
        public static int GetNewInnovConnection()
        {
            INNOV_CONNECTION++;
            return INNOV_CONNECTION;
        }
            
        

        private void Replace()
        {
            GetRepresentation();
            

            nn_poblation.Clear();
            nn_poblation.AddRange(nn_new_poblation);

            
            nn_new_poblation.Clear();





 


        }

        private void GetRepresentation()
        {
            nn_representation_previous_gen.Clear();

            var dic = NNBySpecies(nn_poblation);

            foreach(KeyValuePair<int,List<NeuralNetwork>> specie in dic)
            {
                nn_representation_previous_gen.Add(specie.Value[rnd.Next(0, specie.Value.Count)]);
            }
        }


      



        private void GenerateFitnessSharing()
        {
            CompareBySpecieAndFitness comparespecie = new CompareBySpecieAndFitness();
            nn_poblation.Sort(comparespecie);

            var dic = NNBySpecies(nn_poblation);
            foreach(KeyValuePair<int,List<NeuralNetwork>> specie in dic)
            {
                foreach(NeuralNetwork nn in specie.Value)
                {
                    nn.SetSharedFitness(nn.GetFitness() / specie.Value.Count);
                }
            }
        }


        private bool PoblationDoneCalculatingFitness()
        {
            bool ok = true;
            foreach (GameObject car in cars)
            {
                if (!car.GetComponent<CarFitnessTest>().DoneCalculatingFitness()) ok = false;
            }
            return ok;
        }

        private double CompatibilityDistance(NeuralNetwork nn1, NeuralNetwork nn2)
        {
            double cd = 0;

            int matching = 0;
            int excess = 0;
            int disjoints = 0;
            int excess_and_disjoints = 0;

            var cnn1 = nn1.Connections();
            var cnn2 = nn2.Connections();

            int maxkey = 0;
            bool endexcess = false;
            SortedDictionary<int, Connection> smaller, bigger;

            smaller = cnn2;
            bigger = cnn1;
            foreach (KeyValuePair<int, Connection> k in cnn1)
            {
                if (k.Key > maxkey) maxkey = k.Key;
            }
            foreach (KeyValuePair<int, Connection> k in cnn2)
            {
                if (k.Key > maxkey)
                {
                    maxkey = k.Key;
                    smaller = cnn1;
                    bigger = cnn2;
                }
            }


            foreach (KeyValuePair<int,Connection> k in bigger)
            {
                if (smaller.ContainsKey(k.Key)) matching++;
                else excess_and_disjoints++;
            }
            foreach (KeyValuePair<int, Connection> k in smaller)
            {
                if (!bigger.ContainsKey(k.Key)) excess_and_disjoints++;     
            }


            for(int i = maxkey; i >= 0 && !endexcess; i--)
            {
                if (smaller.ContainsKey(i)) endexcess = true;
                else if (bigger.ContainsKey(i)) excess++;                
            }

            disjoints = excess_and_disjoints - excess;










            cd = 1.0 * excess + 1.0 * disjoints + 0.4 * matching;


            return cd;
        }

        private NeuralNetwork GenerateBasicNN()
        {
            if(evolutionMode == EvolutionMode.EvolveDriving)
            {
                // Inputs
                List<string> sinputs = new List<string>();
                sinputs.Add("speed");
                //sinputs.Add("wheelSteering");
                sinputs.Add("bias");


                for (int i = 0; i < CarRaycast.GetNumberOfRays(); i++)
                {
                    sinputs.Add("distWall " + i);
                }

                // Outputs
                List<string> soutputs = new List<string>();
                soutputs.Add("throttle");
                soutputs.Add("brake");
                soutputs.Add("turn");
                soutputs.Add("locksteering");

                return new NeuralNetwork(sinputs, soutputs);
            }
            
            else 
            {
                NNToFile ntf = new NNToFile();
                return ntf.Read("C:/Users/Julio/Desktop/cars/carfast2.txt");
            }
            
        }


        public bool ReadyForRunCars() => readyforruncars;
        public bool ReadyForEvolve() => readyforevolve;


        private SortedDictionary<int, List<NeuralNetwork>> NNBySpecies(List<NeuralNetwork> poblation)
        {
            SortedDictionary<int, List<NeuralNetwork>> dic = new SortedDictionary<int, List<NeuralNetwork>>();

            foreach (NeuralNetwork nn in poblation)
            {
                if (dic.ContainsKey(nn.GetSpecie())) dic[nn.GetSpecie()].Add(nn);
                else
                {
                    dic[nn.GetSpecie()] = new List<NeuralNetwork>();
                    dic[nn.GetSpecie()].Add(nn);
                }
            }


            CompareByFitness comp = new CompareByFitness();
            foreach(KeyValuePair<int, List<NeuralNetwork>> l in dic)
            {
                l.Value.Sort(comp);
            }

            return dic;
        }




    }





    






}





public class CompareBySharedFitness : IComparer<NeuralNetwork>
{
    public int Compare(NeuralNetwork x, NeuralNetwork y)
    {
        if (x.GetSharedFitness() < y.GetSharedFitness()) return -1;
        else if (x.GetSharedFitness() > y.GetSharedFitness()) return 1;
        else return 0;
    }
}

public class CompareByFitness : IComparer<NeuralNetwork>
{
    public int Compare(NeuralNetwork x, NeuralNetwork y)
    {
        if (x.GetFitness() < y.GetFitness()) return -1;
        else if (x.GetFitness() > y.GetFitness()) return 1;
        else return 0;
    }
}

public class CompareBySpecieAndFitness : IComparer<NeuralNetwork>
{
    public int Compare(NeuralNetwork x, NeuralNetwork y)
    {
        if (x.GetSpecie() < y.GetSpecie()) return -1;
        else if (x.GetSpecie() > y.GetSpecie()) return 1;
        else
        {
            if (x.GetFitness() < y.GetFitness()) return -1;
            else if (x.GetFitness() > y.GetFitness()) return 1;
            else return 0;
        }
    }
}

public class CompareBySpecieAndSharedFitness : IComparer<NeuralNetwork>
{
    public int Compare(NeuralNetwork x, NeuralNetwork y)
    {
        if (x.GetSpecie() < y.GetSpecie()) return -1;
        else if (x.GetSpecie() > y.GetSpecie()) return 1;
        else
        {
            if (x.GetSharedFitness() < y.GetSharedFitness()) return -1;
            else if (x.GetSharedFitness() > y.GetSharedFitness()) return 1;
            else return 0;
        }
    }
}
































