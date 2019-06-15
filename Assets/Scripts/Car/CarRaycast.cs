#define DEBUGGING

using Racing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;


namespace VehicleSystem
{

    public enum RayCastPosition { Left, Right}

    public class CarRaycast : MonoBehaviour
    {
 
        private Vector3[] raysDirection;

        [SerializeField]
        private int number_of_rays = 11;


        [SerializeField]
        private bool HasFitnessTest = true;

    

        private const int ray_length = 60;

        [SerializeField]
        private float[] rayDistances;

        [SerializeField]
        private RayCastPosition raycastposition = RayCastPosition.Left;

        public int GetNumberOfRays()
        {
            return number_of_rays;
        }

        public void GenerateRays()
        {
           
            raysDirection = new Vector3[number_of_rays];
            rayDistances = new float[number_of_rays];

            float angle_per_ray = 180.0f / (float)(number_of_rays - 1); 

            for(int i=0; i<number_of_rays; i++)
            {
                raysDirection[i] = new Vector3(Mathf.Cos((angle_per_ray * i * Mathf.PI) / 180), 0, Mathf.Sin((angle_per_ray * i * Mathf.PI) / 180)) * ray_length;
                
            }

            this.gameObject.transform.Translate(new Vector3(0, 100, 0));


            if(raycastposition == RayCastPosition.Left && this.gameObject.transform.parent.transform.parent.GetComponent<RaceDriver>() != null)
            {
                
                Instantiate(this.gameObject.transform.parent.transform.parent.gameObject.transform.Find("ChasisCollider"), this.gameObject.transform.parent.transform.parent).Translate(0, 100, 0);
            }



        }

        

       
        public void CalculateDistances()
        {

  


            RaycastHit hit;
            Color color;


            for (int i = 0; i < number_of_rays; i++)
            {
                color = Color.green;
                rayDistances[i] = 1000.0f;

                if (Physics.Raycast(transform.position, transform.TransformDirection(raysDirection[i]), out hit, 1000f, LayerMask.GetMask("Border")))
                {
                    rayDistances[i] = hit.distance;
                   
                    //Debug.Log(hit.distance + " " + i);
                    
                }

                if (!NEAT.NEATAlgorithm.Training() && Physics.Raycast(transform.position, transform.TransformDirection(raysDirection[i]), out hit, 1000f, LayerMask.GetMask("Vehicles")))
                {

                    if (!hit.transform.gameObject.Equals(this.gameObject) && hit.transform.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        if (hit.transform.gameObject.GetComponent<CarController>().Speed < this.gameObject.GetComponentInParent<CarController>().Speed)
                        {
                            
                            rayDistances[i] = hit.distance;
                            color = Color.yellow;
                        }
                           
                    }
                }

                Debug.DrawRay(transform.position, transform.TransformDirection(raysDirection[i]), color);
                if ( HasFitnessTest && !GetComponentInParent<CarFitnessTest>().DoneCalculatingFitness()) Debug.DrawRay(transform.position, transform.TransformDirection(raysDirection[i]), color);
                else if(!HasFitnessTest) Debug.DrawRay(transform.position, transform.TransformDirection(raysDirection[i]), color);

                /*
                if(rayDistances[i] == 1000.0f)
                {
                    RaycastHit h;
                    Physics.Raycast(transform.position, transform.TransformDirection(raysDirection[i]), out h, Mathf.Infinity);
                    Debug.Log("OJO " + h.collider.name);
                }*/
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

        public RayCastPosition GetRayCastPosition() => raycastposition;
    }

}
