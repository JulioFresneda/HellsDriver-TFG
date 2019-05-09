//#define DEBUGGING

using NEAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VehicleSystem;

namespace VehicleSystem
{
    public class CarFitnessTest : MonoBehaviour
    {
        public int MAX_TIME_RUNNING = 30;

        private static int num_done = 0;
        private static int num_coches = 100;


        private List<GameObject> checkpoints;
        private List<GameObject> checkpoints_checked;
        private int num_checkpoints;

        public float time_running;
        private float initializationTime;


        private double fitness;

        private bool done_calculating_fitness;

        public double time_same_check;

  
        private double mean_throttle;
        private double total_throttle;

        public double throttleweight;

        public int boosteds = 0;


        public double minthrottle = 0;
        public int checkbonus = 200;

        public double minthrottlerange = 0.8;
        public double max_time_same_check = 15;


        private bool started = false;

        public static void ResetDoneNumber()
        {
            num_done = 0;
        }

        public void Awake()
        {
            
            total_throttle = 1;
         
            mean_throttle = 0;
  
            MAX_TIME_RUNNING = 180;
            time_same_check = 0;
            initializationTime = Time.timeSinceLevelLoad;
            checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPoint"));
            num_checkpoints = checkpoints.Count;
            checkpoints_checked = new List<GameObject>();


            // Checkstart in checkeds
            bool startcheck = false;
            for (int i = 0; i < checkpoints.Count && !startcheck; i++)
            {
                if (checkpoints[i].name == "CheckStart")
                {
                    checkpoints_checked.Add(checkpoints[i]);
                    checkpoints.Remove(checkpoints[i]);
                    startcheck = true;
                }
            }


            fitness = 0;
            done_calculating_fitness = false;
        }

        private void Update()
        {
            
            if (num_done >= num_coches / 3)
            {

                done_calculating_fitness = true;

                CalculateFitness();
            }

            if (!done_calculating_fitness)
            {
                // Time running
                time_running = Time.timeSinceLevelLoad - initializationTime;

     

                // If same check surpass limit
                if (time_running - time_same_check > 15)
                {
                    done_calculating_fitness = true;
                    
                    CalculateFitness();
                }

                // if time running surpass limit
                if (time_running > MAX_TIME_RUNNING)
                {
                    done_calculating_fitness = true;
                    
                    CalculateFitness();
                    
                }

            }




        }

        public bool DoneCalculatingFitness() => done_calculating_fitness;
        public void SetDoneCalculatingFitness(bool done) => done_calculating_fitness = done;


        // If collision with border
        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "Border" && !done_calculating_fitness)
            {
                done_calculating_fitness = true;
              
                CalculateFitness();
                
            }

        }

        // If checkpoint
        private void OnTriggerEnter(Collider col)
        {
            

            if (col.gameObject.tag == "CheckPoint" && !done_calculating_fitness)
            {

                if (col.gameObject.name == "CheckStart")
                {
                    if (!started) started = true;
                    else if(checkpoints_checked.Count > 15)
                    {
                        done_calculating_fitness = true;
                        time_running = Time.timeSinceLevelLoad - initializationTime;
                        fitness = 10000000 - time_running;
                        

                        throttleweight = minthrottlerange + (1 - minthrottlerange) * (mean_throttle / total_throttle);
                        
                        if (throttleweight < minthrottle)
                        {
                           
                            fitness = 0;
                        }
                    }
                }
               


                if (!checkpoints_checked.Contains(col.gameObject))
                {
                    
                    time_same_check = time_running;
                    checkpoints_checked.Add(col.gameObject);

                }
                if (checkpoints.Contains(col.gameObject)) checkpoints.Remove(col.gameObject);

            }

           
        
        }




        public void SetThrottle(double thr)
        {

            mean_throttle += thr;
            total_throttle++;
        }



        private void CalculateFitness()
        {
            if (done_calculating_fitness)
            {
                if (checkpoints_checked.Count > 0 && checkpoints.Count > 0) fitness = checkbonus * (checkpoints_checked.Count - 1) + Vector3.Distance(this.GetComponentInParent<Transform>().position, checkpoints_checked[checkpoints_checked.Count - 1].GetComponentInParent<Transform>().position);
                else if (checkpoints.Count == 0) fitness = checkbonus * checkpoints_checked.Count * 2;

                throttleweight = minthrottlerange + (1-minthrottlerange) * (mean_throttle / total_throttle);

                fitness = fitness * throttleweight;


                if (throttleweight < minthrottle)
                {
                    fitness = 0;
                }
                
            }
            
            

        }


        public double GetFitness() => fitness;


    }

}
