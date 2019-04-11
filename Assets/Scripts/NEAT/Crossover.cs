using System;
using System.Collections;
using System.Collections.Generic;
using NeuralNet;

namespace NEAT
{
    public class Crossover
    {
        public static NeuralNetwork GetCrossover(NeuralNetwork nn1, NeuralNetwork nn2)
        {
            NeuralNetwork better, worse;

            if (nn1.GetFitness() > nn2.GetFitness())
            {
                better = nn1;
                worse = nn2;
            }
            else
            {
                better = nn2;
                worse = nn1;
            }


            SortedDictionary<int, Connection> connections_better = better.Connections();
            SortedDictionary<int, Connection> connections_worse = worse.Connections();

            SortedDictionary<int, Neuron> neurons_better = better.Neurons();
            SortedDictionary<int, Neuron> neurons_worse = worse.Neurons();


            List<Connection> matches_connections = new List<Connection>();
            List<Connection> disjoint_connections = new List<Connection>();

            List<int> neurons_innovs = new List<int>();
            List<Neuron> neurons = new List<Neuron>();

            Random rnd = NEATAlgorithm.rnd;
            var keys = connections_better.Keys;
            int max = -1;
            foreach(int k in keys)
            {
                if (k > max) max = k;
            }
            for(int i=0; i<max+1; i++)
            {
                if(connections_better.ContainsKey(i) && connections_worse.ContainsKey(i))
                {
                    if (rnd.Next(0, 2) == 1)
                    {
                        matches_connections.Add(connections_better[i]);
                        if (!neurons_innovs.Contains(connections_better[i].InputNeuronInnov())) neurons_innovs.Add(connections_better[i].InputNeuronInnov());
                        if (!neurons_innovs.Contains(connections_better[i].OutputNeuronInnov())) neurons_innovs.Add(connections_better[i].OutputNeuronInnov());
                    }
                    else
                    {
                        matches_connections.Add(connections_worse[i]);
                        if (!neurons_innovs.Contains(connections_worse[i].InputNeuronInnov())) neurons_innovs.Add(connections_worse[i].InputNeuronInnov());
                        if (!neurons_innovs.Contains(connections_worse[i].OutputNeuronInnov())) neurons_innovs.Add(connections_worse[i].OutputNeuronInnov());
                    }
                    }
                else if (connections_better.ContainsKey(i))
                {
                    disjoint_connections.Add(connections_better[i]);
                    if (!neurons_innovs.Contains(connections_better[i].InputNeuronInnov())) neurons_innovs.Add(connections_better[i].InputNeuronInnov());
                    if (!neurons_innovs.Contains(connections_better[i].OutputNeuronInnov())) neurons_innovs.Add(connections_better[i].OutputNeuronInnov());
                }
            }

            Neuron tmp;
            foreach(int i in neurons_innovs)
            {
                tmp = better.FindNeuron(i);
                neurons.Add(new Neuron(tmp.NeuronInnov(), tmp.GetActualRole(), tmp.ActivationFunction(), name: tmp.Name()));
            }


            List<Connection> connections = new List<Connection>();
            connections.AddRange(matches_connections);
            connections.AddRange(disjoint_connections);

            foreach(Connection c in connections)
            {
                if (!c.IsEnabled() && rnd.NextDouble() > 0.9) c.SetStatus(true);
            }

            return new NeuralNetwork(neurons, connections);


        }
    }

}
