using System;
using System.Collections;
using System.Collections.Generic;
using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{
    [Serializable]
    public class NeuralNetwork
    {

        private SortedDictionary<int, Neuron> neurons;
        private List<int> inputs;
        private List<int> outputs;

        private SortedDictionary<int, Connection> connections;

        private double fitness;
        private double shared_fitness;
        private int specie_id;
        public int nnid = -1;

        public double lockweight;
        public double throttleweight;
        public int boosteds;


        public NeuralNetwork()
        {
            neurons = new SortedDictionary<int, Neuron>();
            connections = new SortedDictionary<int, Connection>();

            inputs = new List<int>();
            outputs = new List<int>();

            fitness = 0;
            shared_fitness = 0;
            specie_id = -1;
        }

        public NeuralNetwork(NeuralNetwork n)
        {
            neurons = n.Neurons();
            connections = n.Connections();

            inputs = n.Inputs();
            outputs = n.Outputs();

            fitness = n.GetFitness();
            shared_fitness = n.GetSharedFitness();
            nnid = n.nnid;
        }

       


        public NeuralNetwork(List<string> sinputs, List<string> soutputs)
        {
            neurons = new SortedDictionary<int, Neuron>();
            connections = new SortedDictionary<int, Connection>();

            inputs = new List<int>();
            outputs = new List<int>();

            for (int i = 0; i < sinputs.Count; i++)
            {
                this.AddNeuron(i, Role.input, Activation_function.lineal, name: sinputs[i]);
            }

            for (int i = 0; i < soutputs.Count; i++)
            {
                if(soutputs[i] == "throttle" || soutputs[i] == "brake") this.AddNeuron(i + sinputs.Count, Role.output, Activation_function.sigmoid, name: soutputs[i]);
                else this.AddNeuron(i + sinputs.Count, Role.output, Activation_function.tanh, name: soutputs[i]);
            }


            int coninnov = 0;
            foreach (int i in inputs)
            {
                foreach (int o in outputs)
                {
                    this.AddConnection(new Connection(i, o, 0, coninnov));
                    coninnov++;
                }
            }

            fitness = 0;
            shared_fitness = 0;
            specie_id = -1;


        }





        public NeuralNetwork(List<Neuron> neurons, List<Connection> connections)
        {

            this.neurons = new SortedDictionary<int, Neuron>();
            this.connections = new SortedDictionary<int, Connection>();

            inputs = new List<int>();
            outputs = new List<int>();

            foreach (Neuron n in neurons)
            {
                this.AddNeuron(n);

            }

            foreach (Connection c in connections)
            {
                this.AddConnection(c);
            }

            fitness = 0;
            shared_fitness = 0;
            specie_id = -1;
        }

        public void AddNeuron(int neuronInnov, Role role, Activation_function af, double val = 0.0, string name = "")
        {
            neurons.Add(neuronInnov, new Neuron(neuronInnov, role, af, val, name));
            if (role == Role.input) inputs.Add(neuronInnov);
            else if (role == Role.output) outputs.Add(neuronInnov);
        }


        public void AddNeuron(Neuron n)
        {
            neurons.Add(n.NeuronInnov(), n.Copy());
            if (n.GetActualRole() == Role.input) inputs.Add(n.NeuronInnov());
            else if (n.GetActualRole() == Role.output) outputs.Add(n.NeuronInnov());
        }

        public void AddConnection(Connection c)
        {
            connections.Add(c.ConnectionInnov(), c.Copy());
            neurons[c.InputNeuronInnov()].AddConnectionForward(c.ConnectionInnov());
            neurons[c.OutputNeuronInnov()].AddConnectionBehind(c.ConnectionInnov());


        }









        public Neuron FindNeuron(int neuronInnov) => neurons[neuronInnov];


        public Neuron FindNeuron(string name)
        {
            foreach (KeyValuePair<int, Neuron> n in neurons)
            {
                if (n.Value.Name() == name) return n.Value;
            }
            return null;
        }

        public Connection FindConnection(int connectionInnov) => connections[connectionInnov];




        public bool SetInputValue(int inputInnov, double value)
        {
            try
            {
                neurons[inputInnov].SetInputValue(value);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

        }

        public bool SetInputValue(string name, double value)
        {
            try
            {
                foreach (KeyValuePair<int, Neuron> n in neurons)
                {
                    if (n.Value.Name() == name) n.Value.SetInputValue(value);
                }

                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

        }

        public bool SetInputValues(List<double> inputValues)
        {
            bool ok = false;
            if (inputs.Count >= inputValues.Count)
            {
                for (int i = 0; i < inputs.Count; i++)
                {
                    neurons[inputs[i]].SetInputValue(inputValues[i]);
                }
                ok = true;
            }
            return ok;

        }

        public void SetInputValues(List<Tuple<string, double>> inputValues)
        {
            foreach (Tuple<string, double> input in inputValues)
            {
                this.SetInputValue(input.Item1, input.Item2);
            }

        }

        public List<Tuple<string, double>> OutputValuesWithName()
        {
            List<Tuple<string, double>> outs = new List<Tuple<string, double>>();
            foreach (int o in outputs)
            {
                outs.Add(new Tuple<string, double>(neurons[o].Name(), Value(neurons[o])));
            }
            return outs;
        }


        public List<double> OutputValues()
        {
            List<double> outs = new List<double>();
            foreach (int o in outputs)
            {
                outs.Add(Value(neurons[o]));
            }
            return outs;
        }


        public double Value(Neuron neuron)
        {
            double val = 0.0;

            if (neuron.GetActualRole() == Role.input)
            {
                val = neuron.GetInputValue();
            }
            else
            {
                foreach (int cinnov in neuron.GetConnectionsBehind())
                {
                    if (FindConnection(cinnov).IsEnabled()) val += FindConnection(cinnov).Weight() * Value(FindNeuron(FindConnection(cinnov).InputNeuronInnov()));
                }


            }

            return neuron.GetValueActivated(val);
        }

        public void DisableConnection(int connectionInnov) => connections[connectionInnov].SetStatus(ConnectionStatus.Disabled);



        public SortedDictionary<int, Neuron> Neurons()
        {
            SortedDictionary<int, Neuron> neu = new SortedDictionary<int, Neuron>();
            foreach (KeyValuePair<int, Neuron> n in neurons)
            {
                neu[n.Key] = n.Value.Copy();
            }

            return neu;
        }

        public SortedDictionary<int, Neuron> NeuronsRef() => neurons;

        public SortedDictionary<int, Connection> Connections()
        {
            SortedDictionary<int, Connection> con = new SortedDictionary<int, Connection>();
            foreach (KeyValuePair<int, Connection> c in connections)
            {
                con[c.Key] = c.Value.Copy();
            }

            return con;
        }

        public SortedDictionary<int, Connection> ConnectionsRef() => connections;




        public List<int> Inputs() => new List<int>(inputs);
        public List<int> Outputs() => new List<int>(outputs);

        public double GetFitness() => fitness;
        public void SetFitness(double fitness) => this.fitness = fitness;

        public double GetSharedFitness() => shared_fitness;
        public void SetSharedFitness(double shared_fitness) => this.shared_fitness = shared_fitness;

        public int GetSpecie() => specie_id;
        public void SetSpecie(int specie_id) => this.specie_id = specie_id;



        public bool HaveNeuronBehind(Neuron n, Neuron nb, List<int> neurons_visited = null)
        {
            List<int> visited = neurons_visited;
            if(visited == null)
            {
                visited = new List<int>();
            }

            if (n == null || nb == null) return true;
            else if (n.NeuronInnov() == nb.NeuronInnov()) return true;

            int max = 0;
            foreach (int i in visited)
            {
                if (i == n.NeuronInnov()) max++;

            }
            if (max > 5) return true;
            else
            {
                visited.Add(n.NeuronInnov());
                bool ok = false;
                foreach (int c in n.GetConnectionsBehind())
                {
                    if (HaveNeuronBehind(FindNeuron(FindConnection(c).InputNeuronInnov()), nb, visited) == true) ok = true;
                }
                return ok;
            }
        }

    }
}

