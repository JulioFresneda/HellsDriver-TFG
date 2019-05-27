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



        private CarModel carModel;

        private bool finished = false;

        private float TimeLastCrash = 0;

        private int sprintDistance, lowerSprintDistance;

        [SerializeField]
        private double minVelocity = 15f;

        
        public void SetFinalPosition(int p) => finalPosition = p;
        public void SetCurrentPosition(int p) => currentPosition = p;
        public void SetSeconds(float t) => seconds = t;
        public void SetName(string n) => drivername = n;

        public GameObject GetCar() => gameObject;
        public int GetCurrentPosition() => currentPosition;
        public int GetFinalPosition() => finalPosition;

        public int GetNumCheckpointsChecked() => checkpoints_checked.Count;

        public int GetCurrentLap() => currentLap;


        public CarModel GetCarModel() => carModel;
        public void SetCarModel(CarModel carModel)
        {
            this.carModel = carModel;
            gameObject.GetComponent<Rigidbody>().mass = carModel.GetMass();
            gameObject.GetComponent<CarController>().throttlePower = carModel.GetThrottle();


            WheelFrictionCurve wfc = new WheelFrictionCurve();
            wfc.extremumSlip = 0.4f;
            wfc.extremumValue = 1f;
            wfc.asymptoteSlip = 0.5f;
            wfc.asymptoteValue = 0.75f;
            wfc.stiffness = carModel.GetStiffness();

            foreach (WheelCollider wheel in gameObject.GetComponentsInChildren<WheelCollider>())
            {
 
                wheel.sidewaysFriction = wfc;

            }


            
            
        }

        public void SetCarModelAI(CarModelAI carModel)
        {

            this.carModel = carModel;
            gameObject.GetComponent<Rigidbody>().mass = carModel.GetMass();
            gameObject.GetComponent<CarController>().throttlePower = carModel.GetThrottle();


            WheelFrictionCurve wfc = new WheelFrictionCurve();
            wfc.extremumSlip = 0.4f;
            wfc.extremumValue = 1f;
            wfc.asymptoteSlip = 0.5f;
            wfc.asymptoteValue = 0.75f;
            wfc.stiffness = carModel.GetStiffness();

            foreach (WheelCollider wheel in gameObject.GetComponentsInChildren<WheelCollider>())
            {

                wheel.sidewaysFriction = wfc;

            }

            gameObject.GetComponent<CarAI>().SetNeuralNetwork(carModel.GetNN());
           



            
        }




       

        private void Start()
        {
            sprintDistance = GetComponent<CarAI>().GetSprintDistance();
            lowerSprintDistance = 10;

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
            CheckColliderDisabled();
            CheckRestartSprintDistance();
            CheckUnfreeze();
            seconds = Time.timeSinceLevelLoad - startRaceTime;

        }

        private void CheckUnfreeze()
        {
            if (unfreezed && Time.timeSinceLevelLoad - lastunfreezed > 5f) FreezeYAxis();
        }



        private void CheckRestartSprintDistance()
        {
            if (Time.timeSinceLevelLoad - TimeLastCrash > 3f && GetComponent<CarAI>().GetSprintDistance() == lowerSprintDistance) SetSprintDistance(sprintDistance);
        }




        private bool startCrashing = false;
        private bool crashed = false;
        private float crashTime;


        

        private void CheckCrash()
        {

            if(gameObject.GetComponentInParent<CarController>().Speed < minVelocity && (lastCheckpoint != null || checkpoints_checked.Count == 1) )
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
                UnfreezeYAxisTemporally();
                Debug.Log("CRASH");
                GoToLastCheckPoint();
                crashed = false;
                startCrashing = false;
                SetSprintDistance(lowerSprintDistance);
                TimeLastCrash = Time.timeSinceLevelLoad;
            }
        }

        private void SetSprintDistance(int distance)
        {
            GetComponent<CarAI>().SetSprintDistance(distance);
        }



        private void GoToLastCheckPoint()
        {
            if (!finished)
            {
                gameObject.transform.position = new Vector3(lastCheckpoint.transform.position.x, GameObject.FindWithTag("Road").transform.position.y + 1, lastCheckpoint.transform.position.z);
                gameObject.transform.rotation = lastCheckpoint.transform.rotation;

                gameObject.transform.Rotate(0, 0, -90);
                gameObject.transform.Rotate(0, 90, 0);


                gameObject.transform.Translate(Vector3.forward * 10);


                OrderCheckPoints ocp = new OrderCheckPoints();
                all_checkpoints.Sort(ocp);



                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                DisableColliderTemporally();
            }
            
        }




        private bool tempDisabled = false;
        private float startedDisabled;
        public float timeColliderDisabledOnCrash = 5f;
        private void DisableColliderTemporally()
        {
            tempDisabled = true;
            startedDisabled = Time.timeSinceLevelLoad;

            foreach(GameObject g in GameObject.FindGameObjectsWithTag("AIDriver"))
            {
                Physics.IgnoreCollision(g.GetComponentInChildren<BoxCollider>(), this.GetComponentInChildren<BoxCollider>(), true);
            }
            Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("PlayerDriver").GetComponentInChildren<BoxCollider>(), gameObject.GetComponentInChildren<BoxCollider>(),true);
        }

        private void CheckColliderDisabled()
        {
            if (tempDisabled)
            {
                if (Time.timeSinceLevelLoad - startedDisabled > timeColliderDisabledOnCrash)
                {
                    foreach (GameObject g in GameObject.FindGameObjectsWithTag("AIDriver"))
                    {
                        Physics.IgnoreCollision(g.GetComponentInChildren<BoxCollider>(), this.GetComponentInChildren<BoxCollider>(), false);
                    }
                    Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("PlayerDriver").GetComponentInChildren<BoxCollider>(), gameObject.GetComponentInChildren<BoxCollider>(), false);
                    tempDisabled = false;
                }
            }
        }





        private bool started = false;
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.name == "CheckStart")
            {
                if (started && checkpoints_not_checked.Count == 0 && currentLap == Race.GetTotalLaps() && finalPosition < 6)
                {
                    finalPosition = Race.GetPosition();
                    seconds = Time.timeSinceLevelLoad - startRaceTime;
                    Debug.Log("Finish " + finalPosition + " " + seconds);
                    currentLap++;
                    
                    checkpoints_checked.Clear();
                    checkpoints_not_checked.AddRange(all_checkpoints);
                    checkpoints_checked.Add(col.gameObject);
                    checkpoints_not_checked.Remove(col.gameObject);
                    lastCheckpointNumber = 0;
                    lastCheckpoint = col.gameObject;
                    finished = true;

                    if (gameObject.tag == "PlayerDriver")
                    {
                        if (finalPosition <= 3)
                        {
                            GameObject.Find("MedalAnimation").GetComponent<Medal>().LoadMedalAnimation(finalPosition);
                        }


                        else GameObject.Find("RaceFinished").transform.localScale = new Vector3(1, 1, 1);
                        GameObject.Find("RaceFinished").GetComponent<FinishedWindow>().SetDriver(Profiles.GetName(), finalPosition, seconds, carModel) ;
                    }
                    else GameObject.Find("RaceFinished").GetComponent<FinishedWindow>().SetDriver("xd", finalPosition, seconds, carModel);
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

        public float GetSeconds() => seconds;


        private bool unfreezed = false;
        private float lastunfreezed;
        private void UnfreezeYAxisTemporally()
        {
            unfreezed = true;
            lastunfreezed = Time.timeSinceLevelLoad;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }


        public void FreezeYAxis()
        {
            
           this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            
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
