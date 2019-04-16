#define DEBUGGING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;


namespace VehicleSystem
{
    public class CarRaycast : MonoBehaviour
    {
        private const int MAX_DISTANCE = 300;
        private Vector3[] raysDirection;

        [SerializeField]
        private int input_number_of_rays = 8;

        private static int number_of_rays = 8;

        private const int ray_length = 30;

        private float[] rayDistances;

        public static int GetNumberOfRays() => number_of_rays;

        public void GenerateRays()
        {
            raysDirection = new Vector3[number_of_rays];
            rayDistances = new float[number_of_rays];

            float angle_per_ray = 180.0f / (float)(number_of_rays - 1); 

            for(int i=0; i<number_of_rays; i++)
            {
                raysDirection[i] = new Vector3(Mathf.Cos((angle_per_ray * i * Mathf.PI) / 180), 0, Mathf.Sin((angle_per_ray * i * Mathf.PI) / 180)) * ray_length;
            }

            

        }



       
        public void CalculateDistances()
        {

            if (input_number_of_rays != number_of_rays)
            {
                number_of_rays = input_number_of_rays;
                GenerateRays();
            }



            RaycastHit hit;
            Color color;


            for (int i = 0; i < number_of_rays; i++)
            {
                color = Color.green;
                rayDistances[i] = 1000.0f;

                if (Physics.Raycast(transform.position, transform.TransformDirection(raysDirection[i]), out hit, Mathf.Infinity, LayerMask.GetMask("TransparentFX")))
                {
                    if (hit.distance < MAX_DISTANCE) rayDistances[i] = hit.distance;
                    else rayDistances[i] = MAX_DISTANCE;

                    
                }
                /*if (Physics.Raycast(transform.position, transform.TransformDirection(raysDirection[i]), out hit, Mathf.Infinity, LayerMask.GetMask("Vehicles")))
                {

                    if (hit.distance < rayDistances[i] && !hit.transform.gameObject.Equals(this.gameObject) && hit.transform.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < this.GetComponent<CarController>().Speed)
                    {
                        rayDistances[i] = hit.distance;
                        color = Color.yellow;
                    }
                }*/


                if(!GetComponentInParent<CarFitnessTest>().DoneCalculatingFitness()) Debug.DrawRay(transform.position, transform.TransformDirection(raysDirection[i]), color);
           
            }

           



        }


        public float[] GetDistances()
        {
            return rayDistances;
        }


        public float GetDistance(int index)
        {
            return rayDistances[index];
        }
    }

}
