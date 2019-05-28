using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VehicleSystem
{

    public class CarModel
    {
        protected string brand;
        protected string model;

        protected int mass;
        protected int stiffness;
        protected int throttle;

        protected int prize;

        public CarModel(string brand, string model, int mass, int stiffness, int throttle, int prize)
        {
            this.brand = brand;
            this.model = model;
            this.mass = mass;
            this.stiffness = stiffness;
            this.throttle = throttle;
            this.prize = prize;
   
        }

       

        public string GetBrand() => brand;
        public string GetModel() => model;
        public int GetMass() => mass;
        public int GetStiffness() => stiffness;
        public int GetThrottle() => throttle;

        public int GetPrize() => prize;


    }


    public class CarModelAI : CarModel
    {
        

        private string map;
        private double time;

        private NeuralNet.NeuralNetwork nn;

      

        public CarModelAI(string brand, string model, int mass, int stiffness, int throttle, string map, double time, NeuralNet.NeuralNetwork nn) : base(brand,model,mass,stiffness,throttle,0)
        {
            this.brand = brand;
            this.model = model;
            this.mass = mass;
            this.stiffness = stiffness;
            this.throttle = throttle;
            this.map = map;
            this.time = time;
            this.nn = nn;
            this.prize = 0;
        }


        public double GetTime() => time;
        
        public string GetMap() => map;

        public NeuralNet.NeuralNetwork GetNN() => nn;

    }




    public class OrderCarModels : IComparer<CarModelAI>
    {
        public int Compare(CarModelAI x, CarModelAI y)
        {
            if (x.GetTime() > y.GetTime()) return -1;
            else return 1;
        }
    }

}
