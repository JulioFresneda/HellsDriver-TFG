using NEAT;
using NeuralNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

namespace VehicleSystem
{
    public class CarAI : MonoBehaviour
    {
        // INPUTS FOR NN

        private double speed;
        public double Speed { get { return speed; } set { speed = value; } }
        private double wheelSteerAngle;
        public double WheelSteerAngle { get { return wheelSteerAngle; } set { wheelSteerAngle = value; } }
        private double boost;
        public double Boost { get { return boost; } set { boost = value; } }




        // Inputs for NN (outputs by car): Speed, wheelSteering, distWalls
        // Outputs by NN (inputs for car): Throttle, brake, turnInput, boost
        private NeuralNetwork nn;

        private CarRaycast carRaycast;
        private DrawNN drawNN;

        private List<Tuple<string, double>> inputs;
        private List<Tuple<string, double>> outputs;

        private List<double> wallDistances;

        [SerializeField]
        private bool NEAT = false;

        // Start is called before the first frame update
        void Awake()
        {
            wallDistances = new List<double>();
            carRaycast = GetComponentInParent<CarRaycast>();


            inputs = new List<Tuple<string, double>>();

            
            if (!NEAT)
            {
                NNToFile ntf = new NNToFile();
                nn = ntf.Read("car48_11.txt");
            }
            

    


        }

        // Update is called once per frame
        public void AIUpdate()
        {

            inputs.Clear();
            inputs.Add(new Tuple<string, double>("speed", speed));
            //inputs.Add(new Tuple<string, double>("wheelSteering", wheelSteerAngle));
            inputs.Add(new Tuple<string, double>("bias", 1));
            wallDistances.Clear();
            carRaycast.GenerateRays();
            carRaycast.CalculateDistances();
            foreach (float f in carRaycast.GetDistances())
            {
                wallDistances.Add((double)f);
            }
            for (int i = 0; i < wallDistances.Count; i++)
            {
                inputs.Add(new Tuple<string, double>("distWall " + i, wallDistances[i]));
            }


            




            nn.SetInputValues(inputs);
            outputs = nn.OutputValuesWithName();
        

            if (NEAT)
            {
                double thr = 0;
                double br = 0; ;
                foreach (Tuple<string, double> o in outputs)
                {
                    if (o.Item1 == "locksteering")
                    {
                        this.GetComponentInParent<CarFitnessTest>().SetLock(o.Item2);
                    }

                   
                    if (o.Item1 == "brake") br = (o.Item2)*2;
                }

                this.GetComponentInParent<CarFitnessTest>().SetThrottle(1 - br);
            }
            

        }

        public NeuralNetwork GetNeuralNetwork()
        {
            nn.lockweight = this.GetComponentInParent<CarFitnessTest>().lockweight;
            nn.throttleweight = this.GetComponentInParent<CarFitnessTest>().throttleweight;
            return nn;
        }

        public void SetNeuralNetwork(NeuralNetwork n)
        {
            nn = n;
        }


        public List<Tuple<string,double>> GetOutputs()
        {
            return outputs;
        }



    }

}
