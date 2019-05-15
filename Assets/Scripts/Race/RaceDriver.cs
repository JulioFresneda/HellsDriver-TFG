using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

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

  
        private int lastCheckpointNumber = 0;
        private GameObject lastCheckpoint;

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




        private void Update()
        {
            CheckCrash();
        }








        private bool startCrashing = false;
        private bool crashed = false;
        private float crashTime;

        private void CheckCrash()
        {

            if(Mathf.Abs(gameObject.GetComponentInParent<CarController>().Speed) < 1f && lastCheckpoint != null )
            {
                if (!startCrashing)
                {
                    startCrashing = true;
                    crashTime = Time.timeSinceLevelLoad;
                }
                else
                {
                    if (Time.timeSinceLevelLoad-crashTime > 3f)
                    {
                        crashed = true;
                    }
                }
            }
            else
            {
                if (startCrashing)
                {
                    startCrashing = false;
                }
            }



            if (crashed)
            {
                
                GoToLastCheckPoint();
                crashed = false;
                startCrashing = false;
            }
        }



        private void GoToLastCheckPoint()
        {
            gameObject.transform.position = new Vector3(lastCheckpoint.transform.position.x, GameObject.FindWithTag("Road").transform.position.y + 1, lastCheckpoint.transform.position.z);
            gameObject.transform.rotation = lastCheckpoint.transform.rotation;
        
            gameObject.transform.Rotate(0, 0, -90);
            gameObject.transform.Rotate(0, 90,0);


            gameObject.transform.Translate(Vector3.forward * 10);


            OrderCheckPoints ocp = new OrderCheckPoints();
            all_checkpoints.Sort(ocp);
            
            

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
                    lastCheckpointNumber = 0;
                    lastCheckpoint = col.gameObject;

                }
                else if (checkpoints_not_checked.Contains(col.gameObject))
                {
                    Debug.Log("Started");
                    started = true;
                    if (!checkpoints_checked.Contains(col.gameObject))
                    {
                        checkpoints_checked.Add(col.gameObject);
                    }
                     checkpoints_not_checked.Remove(col.gameObject);
                    lastCheckpoint = col.gameObject;
                    
                }
                else if (checkpoints_checked.Contains(col.gameObject))
                {
                    GoToLastCheckPoint();
                }
                
            }
            else if(col.gameObject.tag == "CheckPoint")
            {
                if (!checkpoints_checked.Contains(col.gameObject))
                {
                    checkpoints_checked.Add(col.gameObject);
                    if (col.gameObject.name[8] == ')') lastCheckpointNumber = int.Parse((col.gameObject.name[7] + ""));
                    else
                    {
                        string n = "";
                        n += col.gameObject.name[7];
                        n += col.gameObject.name[8];
                        lastCheckpointNumber = int.Parse(n);
                    }
                    lastCheckpoint = col.gameObject;
                }
                else 
                {
                    GoToLastCheckPoint();
                }


                if (checkpoints_not_checked.Contains(col.gameObject)) checkpoints_not_checked.Remove(col.gameObject);
            }



        }

        public double GetDistanceNextCheckpoint()
        {
            double dist = -1;
            foreach(GameObject g in all_checkpoints)
            {
                if (g.gameObject.name != "CheckStart" && g.gameObject.name[8] == ')' && int.Parse(g.gameObject.name[7] + "") == lastCheckpointNumber + 1)
                {
                    dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);

                }
                else if (g.gameObject.name != "CheckStart" && g.gameObject.name[8] != ')')
                {
                    
                    string n = "";
                    n += g.gameObject.name[7];
                    n += g.gameObject.name[8];
              
                    
                    if( int.Parse(n) == lastCheckpointNumber + 1) dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);
                }
                else if(g.gameObject.name == "CheckStart" && checkpoints_not_checked.Count == 0) dist = Vector3.Distance(this.GetComponentInParent<Transform>().position, g.GetComponentInParent<Transform>().position);

            }

            distance = dist;
            return distance;
        }


        
    }

    

}


public class OrderCheckPoints : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        if (x.gameObject.name == "CheckStart") return -1;
        else if (y.gameObject.name == "CheckStart") return 1;
        else
        {
            if (x.gameObject.name[8] == ')' && y.gameObject.name[8] == ')')
            {
                if (int.Parse(x.gameObject.name[7] + "") < int.Parse(y.gameObject.name[7] + "")) return -1;
                else return 1;
            }
            else if (x.gameObject.name[8] != ')' && y.gameObject.name[8] != ')')
            {
                string nx = "";
                nx += x.gameObject.name[7];
                nx += x.gameObject.name[8];

                string ny = "";
                ny += y.gameObject.name[7];
                ny += y.gameObject.name[8];

                if (int.Parse(nx) < int.Parse(ny)) return -1;
                else return 1;
            }
            else if (x.gameObject.name[8] == ')') return -1;
            else return 1;
        }
    }
}
