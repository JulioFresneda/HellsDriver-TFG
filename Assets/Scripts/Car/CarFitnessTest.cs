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
        public int MAX_TIME_SAME_CHECK = 5;

        private List<GameObject> checkpoints;
        private List<GameObject> checkpoints_checked;
        private int num_checkpoints;

        private float time_running;
        private float initializationTime;


        private double fitness;

        private bool done_calculating_fitness;

        private double time_same_check;

        private double num_lock;
        private double total_lock;

        private double mean_throttle;
        private double total_throttle;

        public double lockweight;
        public double throttleweight;

        public void Awake()
        {
            num_lock = 0;
            total_throttle = 0;
            total_lock = 0;
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

            if (!done_calculating_fitness)
            {
                // Time running
                time_running = Time.timeSinceLevelLoad - initializationTime;

                // If same check surpass limit
                if (NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving && time_running - time_same_check > MAX_TIME_SAME_CHECK)
                {
                    done_calculating_fitness = true;
                    CalculateFitness();
                }

                // if time running surpass limit
                if (time_running > MAX_TIME_RUNNING)
                {
                    done_calculating_fitness = true;
                    CalculateFitness();
                    if (NEATAlgorithm.evolutionMode == EvolutionMode.EvolveSpeed) fitness = -20000000000;
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
                if(NEATAlgorithm.evolutionMode == EvolutionMode.EvolveSpeed) fitness = -100000000000;
            }

        }

        // If checkpoint
        private void OnTriggerEnter(Collider col)
        {
            

            if (NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving && col.gameObject.tag == "CheckPoint" && !done_calculating_fitness)
            {
                
                if (!checkpoints_checked.Contains(col.gameObject))
                {
                    
                    time_same_check = time_running;
                    checkpoints_checked.Add(col.gameObject);
                    if (MAX_TIME_RUNNING < 180) MAX_TIME_RUNNING += 10;
                }
                if (checkpoints.Contains(col.gameObject)) checkpoints.Remove(col.gameObject);

            }

            if(NEATAlgorithm.evolutionMode == EvolutionMode.EvolveSpeed)
            {
                if (col.gameObject.name == "Check (58)")//&& checkpoints_checked.Count > 50)
                {
                    done_calculating_fitness = true;
                    fitness = -time_running;
                }
            }
        
        }




        public void SetLock(double lck)
        {
            if (lck > 0) num_lock++;
            total_lock++;
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
                if (checkpoints_checked.Count > 0) fitness = 200 * (checkpoints_checked.Count - 1) + Vector3.Distance(this.GetComponentInParent<Transform>().position, checkpoints_checked[checkpoints_checked.Count - 1].GetComponentInParent<Transform>().position);

                lockweight = 0.7 + 0.3 * (num_lock / total_lock);
                throttleweight = 0.8 + 0.2 * (mean_throttle / total_throttle);

                fitness = fitness * lockweight * throttleweight;
                //if (lockweight < 0.75) fitness = 0;

            }
            
            

        }


        public double GetFitness() => fitness;


    }

}
