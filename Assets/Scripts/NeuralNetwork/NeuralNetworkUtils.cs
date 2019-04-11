using System.Collections;
using System.Collections.Generic;

namespace NeuralNet
{
    public class NeuralNetworkUtils
    {
        public enum Role { input, output, hidden };
        public enum Activation_function { lineal, ELU, leaky_ReLU, ReLU, tanh, sigmoid }

        public enum ConnectionStatus { Enabled, Disabled }
    }

}
