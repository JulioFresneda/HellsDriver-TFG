using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VehicleSystem
{
    public class CarModel
    {
        private string brand;
        private string model;

        private int mass;
        private int stiffness;
        private int throttle;



        public CarModel(string brand, string model, int mass, int stiffness, int throttle)
        {
            this.brand = brand;
            this.model = model;
            this.mass = mass;
            this.stiffness = stiffness;
            this.throttle = throttle;

       
        }
    }

}
