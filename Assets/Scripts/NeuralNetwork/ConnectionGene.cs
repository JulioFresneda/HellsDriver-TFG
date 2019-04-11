using System.Collections;
using System.Collections.Generic;

using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{
    public class Connection
    {
        private int input_neuron_innov;
        private int output_neuron_innov;

        private double weight;

        private ConnectionStatus status;

        private int connection_innov;


        public Connection(int inputNeuroInnov, int outputNeuroInnov, double weight, int connectionInnov, ConnectionStatus status = ConnectionStatus.Enabled)
        {
            input_neuron_innov = inputNeuroInnov;
            output_neuron_innov = outputNeuroInnov;
            this.weight = weight;
            this.status = status;
            this.connection_innov = connectionInnov;
        }

        public Connection(Connection c)
        {
            input_neuron_innov = c.InputNeuronInnov();
            output_neuron_innov = c.OutputNeuronInnov();
            this.weight = c.Weight();
            this.status = c.Status();
            this.connection_innov = c.ConnectionInnov();
        }

        public bool Equal(Connection c)
        {
            return (this.input_neuron_innov == c.InputNeuronInnov() && this.output_neuron_innov == c.OutputNeuronInnov());
        }

        public Connection Copy() => new Connection(this);
        public double Weight() => weight;
        public void SetWeight(double weight) => this.weight = weight;


        public void SetStatus(bool status)
        {
            if (status)
            {
                this.status = ConnectionStatus.Enabled;
            }
            else this.status = ConnectionStatus.Disabled;
        }

        public void SetStatus(ConnectionStatus status) => this.status = status;
        public bool IsEnabled() => (status == ConnectionStatus.Enabled);
        public ConnectionStatus Status() => status;
        public int ConnectionInnov() => connection_innov;
        public int InputNeuronInnov() => input_neuron_innov;
        public int OutputNeuronInnov() => output_neuron_innov;



    }



}
