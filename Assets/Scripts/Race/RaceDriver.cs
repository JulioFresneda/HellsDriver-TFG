using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing
{
    public class RaceDriver : MonoBehaviour
    {
        
        

        private string drivername;
        private int finalPosition;
        [SerializeField]
        private int currentPosition;
        private float seconds;

        private float startRaceTime;
        private List<GameObject> all_checkpoints;
        private List<GameObject> checkpoints_not_checked;
        private List<GameObject> checkpoints_checked;

        [SerializeField]
        private int lastCheckpoint = 0;

        [SerializeField]
        double distance;

        public void SetFinalPosition(int p) => finalPosition = p;
        public void SetCurrentPosition(int p) => currentPosition = p;
        public void SetSeconds(float t) => seconds = t;
        public void SetName(string n) => drivername = n;

        public GameObject GetCar() => gameObject;
        public int GetCurrentPosition() => currentPosition;
        public int GetFinalPosition() => finalPosition;

        public int GetNumCheckpointsChecked() => checkpoints_checked.Count;

        private void Start()
        {
            finalPosition = -1;
            seconds = 0;
            startRaceTime = Time.timeSinceLevelLoad;

            checkpoints_not_checked = new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPoint"));
            all_checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPoint"));
            checkpoints_checked = new List<GameObject>();
        }


        private bool started = false;
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.name == "CheckStart")
            {
                if (started && checkpoints_not_checked.Count == 0)
                {
                    finalPosition = Race.GetPosition();
                    seconds = Time.timeSinceLevelLoad - startRaceTime;
                    Debug.Log("Finish " + finalPosition + " " + seconds);
                }
                else
                {
                    Debug.Log("Started");
                    started = true;
                    if (!checkpoints_checked.Contains(col.gameObject))
                    {
                        checkpoints_checked.Add(col.gameObject);
                    }
                    if (checkpoints_not_checked.Contains(col.gameObject)) checkpoints_not_checked.Remove(col.gameObject);
                }
                
            }
            else if(col.gameObject.tag == "CheckPoint")
            {
                if (!checkpoints_checked.Contains(col.gameObject))
                {
                    checkpoints_checked.Add(col.gameObject);
                    if(col.gameObject.name[8] == ')') lastCheckpoint = int.Parse((col.gameObject.name[7]+""));
                    else lastCheckpoint = int.Parse((col.gameObject.name[7] + col.gameObject.name[8] +""));
                }
                if (checkpoints_not_checked.Contains(col.gameObject)) checkpoints_not_checked.Remove(col.gameObject);
            }



        }

        public double GetDistanceNextCheckpoint()
        {
            double dist = -1;
            foreach(GameObject g in all_checkpoints)
            {
                if (g.gameObject.name != "CheckStart" && g.gameObject.name[8] == ')' && int.Parse(g.gameObject.name[7] + "") == lastCheckpoint + 1)
                {
                    dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);
                    
                }
                else if (g.gameObject.name != "CheckStart" && g.gameObject.name[8] != ')' && int.Parse(g.gameObject.name[7] + g.gameObject.name[8] + "") == lastCheckpoint + 1) dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);
            }

            if (checkpoints_checked.Count == 1)
            {
                distance = dist;
                return dist;
            }
            else
            {
                distance = (-200 * checkpoints_checked.Count) + dist; 
                return (-200 * checkpoints_checked.Count) + dist;
            }
        }


        
    }

    

}
