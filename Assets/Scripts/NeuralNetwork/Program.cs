using System;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{
    public class Program : MonoBehaviour
    {
        static void Main(string[] args)
        {

            List<Neuron> neurons = new List<Neuron>();


            for (int i = 0; i < 9; i++)
            {
                if (i < 3) neurons.Add(new Neuron(i, Role.input, Activation_function.ReLU, 1, "input" + i));
                else if (i < 7) neurons.Add(new Neuron(i, Role.hidden, Activation_function.ReLU));
                else neurons.Add(new Neuron(i, Role.output, Activation_function.ReLU, 0, "output" + i));
            }


            NeuralNetwork nn = new NeuralNetwork();

            foreach (Neuron n in neurons)
            {
                nn.AddNeuron(n);
            }

            List<Connection> connections = new List<Connection>();

            connections.Add(new Connection(0, 3, 0.5, 0));
            connections.Add(new Connection(0, 4, 0.25, 1));
            connections.Add(new Connection(1, 4, 0.2, 2));
            connections.Add(new Connection(1, 5, 0.1, 3));
            connections.Add(new Connection(2, 3, 0.4, 4));
            connections.Add(new Connection(2, 4, 0.3, 5));
            connections.Add(new Connection(2, 5, 0.2, 6));
            connections.Add(new Connection(2, 6, 0.1, 7));

            connections.Add(new Connection(3, 7, 1, 8));
            connections.Add(new Connection(3, 8, 0.3, 9));
            connections.Add(new Connection(4, 7, 0.1, 10));
            connections.Add(new Connection(4, 8, 0.8, 11));
            connections.Add(new Connection(5, 7, 1, 12));
            connections.Add(new Connection(5, 8, 2, 13));
            connections.Add(new Connection(6, 8, 2, 14));




            foreach (Connection c in connections)
            {
                nn.AddConnection(c);
            }

            nn.AddNeuron(new Neuron(9, Role.hidden, Activation_function.tanh, name: "añadido"));

            nn.DisableConnection(14);
            SortedDictionary<int, Neuron> ncopy = nn.Neurons();
            SortedDictionary<int, Connection> ccopy = nn.Connections();



            Console.WriteLine(ncopy.Count);

            foreach (KeyValuePair<int, Neuron> n in ncopy)
            {
                Console.WriteLine(n.Value.Name() + ": " + n.Value.NeuronInnov() + " (" + n.Key + ") ");
                Console.WriteLine("Conn behind: ");
                foreach (int c in n.Value.GetConnectionsBehind())
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
                Console.WriteLine("Conn forward: ");
                foreach (int c in n.Value.GetConnectionsForward())
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }
            foreach (KeyValuePair<int, Connection> c in ccopy)
            {
                Console.WriteLine(c.Value.ConnectionInnov() + " (" + c.Key + ") " + c.Value.Weight() + " " + c.Value.InputNeuronInnov() + " " + c.Value.OutputNeuronInnov());
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();



            List<double> ov = nn.OutputValues();
            foreach (double d in ov)
            {
                Console.WriteLine(d);
            }





            Console.ReadKey();




        }
    }

}
