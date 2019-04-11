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



        public void Awake()
        {

            MAX_TIME_RUNNING = 180;
            time_same_check = 0;
            initializationTime = Time.timeSinceLevelLoad;
            checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPoint"));
            num_checkpoints = checkpoints.Count;
            checkpoints_checked = new List<GameObject>();

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


            fitness = -1000000;
            done_calculating_fitness = false;
        }


        private void Update()
        {



            time_running = Time.timeSinceLevelLoad - initializationTime;


            if (NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving && time_running - time_same_check > MAX_TIME_SAME_CHECK) done_calculating_fitness = true;

            if (time_running > MAX_TIME_RUNNING)
            {
                done_calculating_fitness = true;
                fitness = -10000000000;
            }

            if (!done_calculating_fitness && NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving)
            {
                CalculateFitness();
            }


        }

        public bool DoneCalculatingFitness() => done_calculating_fitness;
        public void SetDoneCalculatingFitness(bool done) => done_calculating_fitness = done;



        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "Border")
            {
                done_calculating_fitness = true;
                fitness = -100000000000;
            }

        }

        private void OnTriggerEnter(Collider col)
        {
            

            if (NEATAlgorithm.evolutionMode == EvolutionMode.EvolveDriving && col.gameObject.tag == "CheckPoint")
            {
                time_same_check = time_running;
                if (!checkpoints_checked.Contains(col.gameObject))
                {
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



        private void CalculateFitness()
        {


      


            if (checkpoints_checked.Count > 0) fitness = 50 * (checkpoints_checked.Count - 1) + Vector3.Distance(this.GetComponentInParent<Transform>().position, checkpoints_checked[checkpoints_checked.Count - 1].GetComponentInParent<Transform>().position);




        }


        public double GetFitness() => fitness;

    }

}
