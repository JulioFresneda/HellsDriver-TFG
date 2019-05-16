using System.Collections;
using System.Collections.Generic;
using System.IO;
using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{
    public class NNToFile
    {

        private SortedDictionary<int, Neuron> neurons;
        private SortedDictionary<int, Connection> connections;
        private double fitness;
        private double shared_fitness;
        private int specie_id;

        public NNToFile(NeuralNetwork nn)
        {
            neurons = nn.Neurons(); // 3
            connections = nn.Connections(); // 4
            fitness = nn.GetFitness(); // 0
            shared_fitness = nn.GetSharedFitness(); // 0
            specie_id = nn.GetSpecie(); // 0
        }

        public NNToFile()
        {
            neurons = new SortedDictionary<int, Neuron>();
            connections = new SortedDictionary<int, Connection>();
        }



        public NeuralNetwork Read(string path)
        {
            StreamReader sr = new StreamReader(path);

            string cabecera = sr.ReadLine();

            string sfitness = sr.ReadLine();
            fitness = double.Parse(sfitness);
            shared_fitness = double.Parse(sr.ReadLine());

             
            specie_id = int.Parse(sr.ReadLine());


            string str = sr.ReadLine();

            while (str != "<<")
            {
                
                int neuroinnov = int.Parse(sr.ReadLine());
                Role role = Role.input ;
                str = sr.ReadLine();
                if (str[0] == 'i') role = Role.input;
                else if (str[0] == 'h') role = Role.hidden;
                else if (str[0] == 'o') role = Role.output;

                str = sr.ReadLine();
                Activation_function af = Activation_function.lineal;
                if (str[0] == 'l' && str[1] == 'i') af = Activation_function.lineal;
                else if (str[0] == 'E') af = Activation_function.ELU;
                else if (str[0] == 'R') af = Activation_function.ReLU;
                else if (str[0] == 'l' && str[1] == 'e') af = Activation_function.leaky_ReLU;
                else if (str[0] == 's') af = Activation_function.sigmoid;
                else if (str[0] == 't') af = Activation_function.tanh;

                string name = sr.ReadLine();

                str = sr.ReadLine();


                neurons[neuroinnov] = (new Neuron(neuroinnov, role, af, name: name));
            }


            str = sr.ReadLine();
            while(str != "<<")
            {
                str = sr.ReadLine();
                int cinnov = int.Parse(str);
                int ini = int.Parse(sr.ReadLine());
                int oni = int.Parse(sr.ReadLine());
                double w = double.Parse(sr.ReadLine());
                ConnectionStatus cs = ConnectionStatus.Enabled;
                if (sr.ReadLine()[0] == 'D') cs = ConnectionStatus.Disabled;

                connections[cinnov] = new Connection(ini, oni, w, cinnov, cs);
                str = sr.ReadLine();
            }

            List<Neuron> ln = new List<Neuron>();
            foreach(KeyValuePair<int,Neuron> k in neurons)
            {
                ln.Add(k.Value);
            }
            List<Connection> lc = new List<Connection>();
            foreach (KeyValuePair<int, Connection> k in connections)
            {
                lc.Add(k.Value);
            }

            NeuralNetwork nn = new NeuralNetwork(ln, lc);
            nn.SetFitness(fitness);
            nn.SetSpecie(specie_id);
            nn.SetSharedFitness(shared_fitness);

            return nn;

        }


        public static void Write(Connection c, StreamWriter sw)
        {
            sw.WriteLine(">Connection " + c.ConnectionInnov());
            sw.WriteLine(c.ConnectionInnov());
            sw.WriteLine(c.InputNeuronInnov());
            sw.WriteLine(c.OutputNeuronInnov());
            sw.WriteLine(c.Weight());
            sw.WriteLine(c.Status().ToString());
        }


        public void Write(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("NeuralNetwork");

            if (fitness < 10000) sw.WriteLine(fitness);
            else sw.WriteLine(10000000 - fitness);
            sw.WriteLine(shared_fitness);
            sw.WriteLine(specie_id);

            foreach (KeyValuePair<int,Neuron> n in neurons)
            {
                Write(n.Value, sw);
            }

            sw.WriteLine("<<");

            foreach (KeyValuePair<int,Connection> c in connections)
            {
                Write(c.Value, sw);
            }

            sw.WriteLine("<<");


            sw.Close();
        }

     

        public static void Write(Neuron n, StreamWriter sw)
        {
            sw.WriteLine(">Neuron " + n.NeuronInnov());
            sw.WriteLine(n.NeuronInnov());
            sw.WriteLine(n.GetActualRole().ToString());
            sw.WriteLine(n.ActivationFunction().ToString());
            sw.WriteLine(n.Name());

        }

        
    }

}
