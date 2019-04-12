using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing
{
    public class RaceDriver : MonoBehaviour
    {
        

        [SerializeField]
        private GameObject car;

        private string drivername;
        private int position;
        private float seconds;

        private float startRaceTime;

        private List<GameObject> checkpoints;
        private List<GameObject> checkpoints_checked;


        public void SetPosition(int p) => position = p;
        public void SetSeconds(float t) => seconds = t;
        public void SetName(string n) => drivername = n;

        public GameObject GetCar() => car;

        private void Start()
        {
            position = -1;
            seconds = 0;
            startRaceTime = Time.timeSinceLevelLoad;

            checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("CheckPoint"));
            checkpoints_checked = new List<GameObject>();
        }


        private bool started = false;
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.name == "CheckStart")
            {
                if (started && checkpoints.Count == 0)
                {
                    position = Race.GetPosition();
                    seconds = Time.timeSinceLevelLoad - startRaceTime;
                    Debug.Log("Finish " + position + " " + seconds);
                }
                else
                {
                    Debug.Log("Started");
                    started = true;
                    if (!checkpoints_checked.Contains(col.gameObject))
                    {
                        checkpoints_checked.Add(col.gameObject);
                    }
                    if (checkpoints.Contains(col.gameObject)) checkpoints.Remove(col.gameObject);
                }
                
            }
            else if(col.gameObject.tag == "CheckPoint")
            {
                if (!checkpoints_checked.Contains(col.gameObject))
                {
                    checkpoints_checked.Add(col.gameObject);
                }
                if (checkpoints.Contains(col.gameObject)) checkpoints.Remove(col.gameObject);
            }

        }
    }

    

}
