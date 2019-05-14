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

        private int currentLap = 1;

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

        public int GetCurrentLap() => currentLap;

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
                if (started && checkpoints_not_checked.Count == 0 && currentLap == Race.GetTotalLaps())
                {
                    finalPosition = Race.GetPosition();
                    seconds = Time.timeSinceLevelLoad - startRaceTime;
                    Debug.Log("Finish " + finalPosition + " " + seconds);
                }
                else if (started && checkpoints_not_checked.Count == 0 && currentLap < Race.GetTotalLaps())
                {
                    currentLap++;
                    Debug.Log("Lap " + currentLap);
                    checkpoints_checked.Clear();
                    checkpoints_not_checked.AddRange(all_checkpoints);
                    checkpoints_checked.Add(col.gameObject);
                    checkpoints_not_checked.Remove(col.gameObject);
                    lastCheckpoint = 0;

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
                    if (col.gameObject.name[8] == ')') lastCheckpoint = int.Parse((col.gameObject.name[7] + ""));
                    else
                    {
                        string n = "";
                        n += col.gameObject.name[7];
                        n += col.gameObject.name[8];
                        lastCheckpoint = int.Parse(n);
                    }
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
                else if (g.gameObject.name != "CheckStart" && g.gameObject.name[8] != ')')
                {
                    
                    string n = "";
                    n += g.gameObject.name[7];
                    n += g.gameObject.name[8];
              
                    
                    if( int.Parse(n) == lastCheckpoint + 1) dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);
                }
                else if(g.gameObject.name == "CheckStart" && checkpoints_not_checked.Count == 0) dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);

            }

            distance = dist;
            return distance;
        }


        
    }

    

}
