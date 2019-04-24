using NeuralNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNet.NeuralNetworkUtils;
using Random = System.Random;

namespace NEAT
{
    public class Mutation
    {

        public static double MutateWeightsProbability = 0.1;
        public static double AddNeuronProbability = 0.05;
        public static double AddConnectionProbability = 0.2;
        public static double RandomWeightsProbabilityWhenMutate = 0.1;
        public static double MutateWeightsRange = 0.02;


        public static void Mutate(NeuralNetwork nn)
        {
            Random rnd = NEATAlgorithm.rnd;

            if(NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving)
            {
                if (rnd.NextDouble() > MutateWeightsProbability)
                {
                    Mutation_MutateWeights(nn);
                }
                else if (rnd.NextDouble() <= AddNeuronProbability)
                {
                    Mutation_AddNeuron(nn);
                }
                else if (rnd.NextDouble() <= AddConnectionProbability)
                {
                    Mutation_AddConnection(nn);
                }
            }
            else
            {
                Mutation_MutateWeights(nn);
            }
            


        }



        public static void Mutation_AddConnection(NeuralNetwork nn)
        {           
            List<Tuple<int, int>> possible_connections = new List<Tuple<int, int>>();
            var neurons = nn.Neurons();
            int number_neuros = neurons.Count;
            foreach (KeyValuePair<int,Neuron> n in neurons)
            {
                foreach(KeyValuePair<int, Neuron> n2 in neurons)
                {
                    if (n.Key != n2.Key && n2.Value.GetActualRole() != Role.input && n.Value.GetActualRole() != Role.output)
                    {
                        if(!nn.HaveNeuronBehind(n2.Value,n.Value)) possible_connections.Add(new Tuple<int, int>(n.Key, n2.Key));
                    }
                }
            }

            foreach(KeyValuePair<int,Connection> c in nn.Connections())
            {
                possible_connections.Remove(new Tuple<int, int>(c.Value.InputNeuronInnov(), c.Value.OutputNeuronInnov()));
                possible_connections.Remove(new Tuple<int, int>(c.Value.OutputNeuronInnov(), c.Value.InputNeuronInnov()));
            }

            Random rnd = NEATAlgorithm.rnd;
            Tuple<int, int> con;
            if (possible_connections.Count > 0)
            {
                con = possible_connections[rnd.Next(0, possible_connections.Count)];
                bool used = false;
                foreach (Connection nc in NEATAlgorithm.NewConnections)
                {
                    if (nc.InputNeuronInnov() == con.Item1 && nc.OutputNeuronInnov() == con.Item2)
                    {
                        used = true;
                        nn.AddConnection(new Connection(con.Item1, con.Item2, RandomWeight(), nc.ConnectionInnov()));
                    }
                }

                if (!used)
                {
                    int innov = NEATAlgorithm.GetNewInnovConnection();
                    nn.AddConnection(new Connection(con.Item1, con.Item2, RandomWeight(), innov));
                    NEATAlgorithm.NewConnections.Add(new Connection(con.Item1, con.Item2,1,innov));
                }
            }

            



        }



        public static void Mutation_AddNeuron(NeuralNetwork nn)
        {
            Random rnd = NEATAlgorithm.rnd;
            SortedDictionary<int, Connection> dicconn = nn.Connections();
            List<Connection> listc = new List<Connection>();
            foreach(KeyValuePair<int,Connection> key in dicconn)
            {
                listc.Add(key.Value);
            }

            Connection c = listc[rnd.Next(0, listc.Count)];
            bool used = false;
            // The tuple contains a connection, the connection innov behind the neuron, the connection innov after the neuron, and the neuron innov
            foreach(Tuple<Connection,int,int,int> nc in NEATAlgorithm.NewNeurons)
            {
                if (nc.Item1.Equal(c))
                {
                    used = true;
                    nn.FindConnection(c.ConnectionInnov()).SetStatus(false);
                    nn.AddNeuron(new Neuron(nc.Item4, NeuralNetworkUtils.Role.hidden));
                    nn.AddConnection(new Connection(nc.Item1.InputNeuronInnov(), nc.Item4, 1, nc.Item2));
                    nn.AddConnection(new Connection(nc.Item4, nc.Item1.OutputNeuronInnov(), c.Weight(), nc.Item3));
                }
            }

            if (!used)
            {
                int newneuroinnov = NEATAlgorithm.GetNewInnovNeuron();
                int newc1 = NEATAlgorithm.GetNewInnovConnection();
                int newc2 = NEATAlgorithm.GetNewInnovConnection();

                nn.FindConnection(c.ConnectionInnov()).SetStatus(false);
                nn.AddNeuron(new Neuron(newneuroinnov, NeuralNetworkUtils.Role.hidden));
                nn.AddConnection(new Connection(c.InputNeuronInnov(), newneuroinnov, 1, newc1));
                nn.AddConnection(new Connection(newneuroinnov, c.OutputNeuronInnov(), c.Weight(), newc2));

                NEATAlgorithm.NewNeurons.Add(new Tuple<Connection, int, int, int>(c, newc1, newc2, newneuroinnov));
            }
            
        }


        public static void Mutation_MutateWeights(NeuralNetwork nn)
        {
            Random rnd = NEATAlgorithm.rnd;

            if(NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving)
            {
                foreach (KeyValuePair<int, Connection> c in nn.ConnectionsRef())
                {
                    if (rnd.NextDouble() > RandomWeightsProbabilityWhenMutate)
                    {
                        c.Value.SetWeight(MutateWeight(c.Value.Weight()));
                    }
                    else c.Value.SetWeight(RandomWeight());
                }
            }
            else
            {
                foreach (KeyValuePair<int, Connection> c in nn.ConnectionsRef())
                {
                    c.Value.SetWeight(MutateWeight(c.Value.Weight()));
                }
            }
            

            
            

        }

        





        public static double RandomWeight()
        {
            Random rnd = NEATAlgorithm.rnd;
            return rnd.NextDouble()*2-1;
        }

        public static double MutateWeight(double w)
        {
            Random rnd = NEATAlgorithm.rnd;
            // Change the weight in a range between -5% and +5%
            if( w != 0 ) return w + w * (MutateWeightsRange * (rnd.NextDouble() * 2 - 1));
            else return MutateWeightsRange * (rnd.NextDouble() * 2 - 1);
        }

    }
}

