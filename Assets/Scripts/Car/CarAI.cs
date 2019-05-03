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


        private CarRaycast carRaycastLeft;
        private CarRaycast carRaycastRight;


        private List<Tuple<string, double>> inputs;
        private List<Tuple<string, double>> outputs;

        private List<double> wallDistancesLeft, wallDistancesRight;

        [SerializeField]
        private bool NEAT = false;

        // Start is called before the first frame update
        void Awake()
        {
            wallDistancesLeft = new List<double>();
            wallDistancesRight = new List<double>();

            CarRaycast[] tempray = gameObject.GetComponentsInChildren<CarRaycast>();
            if (tempray[0].IsLeft())
            {
                carRaycastLeft = tempray[0];
                carRaycastRight = tempray[1];
            }
            else
            {
                carRaycastLeft = tempray[1];
                carRaycastRight = tempray[0];
            }



            inputs = new List<Tuple<string, double>>();

            
            if (!NEAT)
            {
                NNToFile ntf = new NNToFile();
                nn = ntf.Read("AIs/Carxd.txt");
            }

            carRaycastLeft.GenerateRays();
            carRaycastRight.GenerateRays();



        }

        // Update is called once per frame
        public void AIUpdate()
        {

            inputs.Clear();
            inputs.Add(new Tuple<string, double>("speed", speed));
            //inputs.Add(new Tuple<string, double>("wheelSteering", wheelSteerAngle));
            inputs.Add(new Tuple<string, double>("bias", 1));
            wallDistancesLeft.Clear();
            wallDistancesRight.Clear();

            
            carRaycastLeft.CalculateDistances();
            carRaycastRight.CalculateDistances();

            foreach (float f in carRaycastLeft.GetDistances())
            {
                wallDistancesLeft.Add((double)f);
            }

            foreach (float f in carRaycastRight.GetDistances())
            {
                wallDistancesRight.Add((double)f);
            }





            CalculateSteering();


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

                    if (o.Item1 == "boost" && o.Item2 > 0) this.GetComponentInParent<CarFitnessTest>().boosteds++;
                }

                this.GetComponentInParent<CarFitnessTest>().SetThrottle(1 - br);
            }
            

        }

        public NeuralNetwork GetNeuralNetwork()
        {
            nn.lockweight = this.GetComponentInParent<CarFitnessTest>().lockweight;
            nn.throttleweight = this.GetComponentInParent<CarFitnessTest>().throttleweight;
            nn.boosteds = this.GetComponentInParent<CarFitnessTest>().boosteds;
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




        public double CalculateSteering()
        {
    
            int raynum = 0;

            double objleft = 0;
            double objright = 0;

            for(int i=0; i<wallDistancesLeft.Count; i++)
            {
                if (objleft < wallDistancesLeft[i])
                {
                    objleft = wallDistancesLeft[i];
                    raynum = i;
                    
                }
                //Debug.Log("Left " + wallDistancesLeft[i] + " " + i);
            }

            int tempraynum = raynum;
            for(int i=0; i<wallDistancesRight.Count; i++)
            {
                if(objright < wallDistancesRight[i])
                {
                    objright = wallDistancesRight[i];
                    tempraynum = i;
                    
                }
                //Debug.Log("Right " + wallDistancesRight[i] + " " + i);
            }

            if(objright < objleft)
            {
                raynum = tempraynum;
                for(int i=0; i<wallDistancesRight.Count; i++)
                {
                    inputs.Add(new Tuple<string, double>("distWall " + i, wallDistancesRight[i]));
                }
            }
            else
            {
                for (int i = 0; i < wallDistancesLeft.Count; i++)
                {
                    inputs.Add(new Tuple<string, double>("distWall " + i, wallDistancesLeft[i]));
                }
            }


            double angle = 180.0f / (float)(wallDistancesRight.Count - 1)*(wallDistancesLeft.Count - raynum-1) - 90f;

            double steerangle = GetComponentInParent<CarController>().GetSteerAngle();
            if (angle < -steerangle) angle = -steerangle;
            else if (angle > steerangle) angle = steerangle;

            angle = angle / steerangle;

            //Debug.Log(angle);
            
            return angle;





        }



    }

}
