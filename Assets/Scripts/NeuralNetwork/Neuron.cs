using System.Collections;
using System.Collections.Generic;
using System;
using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{

    public class Neuron
    {
        // Role of the neuron. It can be Input, Hidden or Output
        private Role actual_role;    

        // Activation function of the neuron
        private Activation_function actual_activation_function;

        // List of connection innovation numbers of the connections behind and forward the neuron
        private List<int> connections_behind;
        private List<int> connections_forward;

        // Value of the neuron (calculated at runtime)
        private double value;

        // Neuro innovation number. Used to identify the neuron in the neural network
        private int neuron_innov;

        // Name of the neuron
        private string name;


        /// <summary>
        /// Creates an empty neuron with an innovation number
        /// </summary>
        /// <param name="neuronInnov">Neuron innovation number</param>
        public Neuron(int neuronInnov)
        {
            connections_behind = new List<int>();
            connections_forward = new List<int>();
            neuron_innov = neuronInnov;
        }

        /// <summary>
        /// Creates a neuron with an innovation number
        /// </summary>
        /// <param name="neuronInnov">Neuron innovation number</param>
        /// <param name="role">Role of the neuron</param>
        /// <param name="af">Activation function of the neuron</param>
        /// <param name="val">Value of the neuron (calculated at runtime)</param>
        /// <param name="name">Name of the neuron (optional)</param>
        public Neuron(int neuronInnov, Role role = Role.hidden, Activation_function af = Activation_function.lineal, double val = 0.0, string name = "")
        {

            actual_role = role;
            actual_activation_function = af;

            connections_behind = new List<int>();
            connections_forward = new List<int>();

            this.name = name;
            value = val;

            neuron_innov = neuronInnov;


        }

        /// <summary>
        /// Creates a copy of a neuron
        /// </summary>
        /// <param name="n">Original neuron to be copied</param>
        public Neuron(Neuron n)
        {
            actual_role = n.GetActualRole();
            if (actual_role == Role.input) value = n.GetInputValue();
            else value = 0.0;
            actual_activation_function = n.ActivationFunction();

            connections_behind = n.GetConnectionsBehind();
            connections_forward = n.GetConnectionsForward();

            name = n.Name();


            neuron_innov = n.NeuronInnov();
        }

        /// <summary>
        /// Copy a neuron
        /// </summary>
        /// <returns>Copy of a neuron</returns>
        public Neuron Copy() => new Neuron(this);

        /// <summary>
        /// Returns the neuron input value, which is 0 if the neuron role is not input
        /// </summary>
        /// <returns>Input value</returns>
        public double GetInputValue()
        {
            if (actual_role == Role.input)
            {
                return value;
            }
            else return 0.0;
        }

        /// <summary>
        /// Set the connections behind the neuron from their connection innovation numbers
        /// </summary>
        /// <param name="cb">List of connection innovation numbers</param>
        public void SetConnectionsBehind(List<int> cb)
        {
            connections_behind = new List<int>();
            connections_behind.AddRange(cb);

        }

        /// <summary>
        /// Set the connections forward the neuron from their connection innovation numbers
        /// </summary>
        /// <param name="cb">List of connection innovation numbers</param>
        public void SetConnectionsForward(List<int> cf)
        {
            connections_forward = new List<int>();
            connections_forward.AddRange(cf);

        }

        /// <summary>
        /// Get a copy of the innovation numbers of the connections behind the neuron
        /// </summary>
        /// <returns>Copy of the list of the innovation numbers of the connections behind the neuron</returns>
        public List<int> GetConnectionsBehind() => new List<int>(connections_behind);

        /// <summary>
        /// Get a copy of the innovation numbers of the connections forward the neuron
        /// </summary>
        /// <returns>Copy of the list of the innovation numbers of the connections forward the neuron</returns>
        public List<int> GetConnectionsForward() => new List<int>(connections_forward);

        /// <summary>
        /// Adds a connection innovation number to the list of the innov numbers of the connections behind the neuron
        /// </summary>
        /// <param name="connection_innov">Connection innovation number</param>
        public void AddConnectionBehind(int connection_innov)
        {
            connections_behind.Add(connection_innov);
            connections_behind.Sort();
        }

        /// <summary>
        /// Adds a connection innovation number to the list of the innov numbers of the connections behind the neuron
        /// </summary>
        /// <param name="connection_innov">Connection innovation number</param>
        public void AddConnectionForward(int connection_innov)
        {
            connections_forward.Add(connection_innov);
            connections_forward.Sort();
        }

        /// <summary>
        /// Get the number of connections behind the neuron
        /// </summary>
        /// <returns>Number of connections</returns>
        public int NumberOfConnectionsBehind() => connections_behind.Count;

        /// <summary>
        /// Get the number of connections forward the neuron
        /// </summary>
        /// <returns>Number of connections</returns>
        public int NumberOfConnectionsForward() => connections_behind.Count;

        /// <summary>
        /// Returns the value when the activation function is applied
        /// </summary>
        /// <param name="value">Original value</param>
        /// <returns>Value activated</returns>
        public double GetValueActivated(double value) => ActivationFunction(value);

        /// <summary>
        /// Get the role of the neuron
        /// </summary>
        /// <returns>Role of the neuron</returns>
        public Role GetActualRole() => actual_role;

        /// <summary>
        /// Get the activation function of the neuron
        /// </summary>
        /// <returns>Activation function of the neuron</returns>
        public Activation_function ActivationFunction() => actual_activation_function;

        /// <summary>
        /// Get the name of the neuron
        /// </summary>
        /// <returns>Name of the neuron</returns>
        public string Name() => name;



        /// <summary>
        /// Set the input value for the neuron, only if the neuron role is "input"
        /// </summary>
        /// <param name="v">Input value</param>
        public void SetInputValue(double v)
        {
            if (actual_role == Role.input) value = v;
        }

        /// <summary>
        /// Activates the value with the activation function
        /// </summary>
        /// <param name="v">Original value</param>
        /// <returns></returns>
        private double ActivationFunction(double v)
        {
            switch (actual_activation_function)
            {
                case Activation_function.lineal:
                    return ActivationFunctionLineal(v);

                case Activation_function.ELU:
                    return ActivationFunctionELU(v);

                case Activation_function.leaky_ReLU:
                    return ActivationFunctionLReLU(v);

                case Activation_function.ReLU:
                    return ActivationFunctionReLU(v);

                case Activation_function.tanh:
                    return ActivationFunctionTanh(v);

                case Activation_function.sigmoid:
                    return ActivationFunctionSigmoid(v);
            }

            return v;

        }


        private double ActivationFunctionLineal(double v)
        {
            return v;
        }

        private double ActivationFunctionELU(double v)
        {

            if (v > 0) return v;
            else
            {
                double p = Math.Pow(Math.E, v);
                return 0.01 * (p - 1.0);
            }
        }

        private double ActivationFunctionLReLU(double v)
        {
            if (v > 0) return v;
            else return 0.01 * v;
        }

        private double ActivationFunctionReLU(double v)
        {
            if (v > 0) return v;
            else return 0;
        }

        private double ActivationFunctionTanh(double v) => Math.Tanh(v);

        private double ActivationFunctionSigmoid(double v) => 1.0 / (1 + Math.Pow(Math.E, -v));



        public int NeuronInnov() => neuron_innov;












    }




}

