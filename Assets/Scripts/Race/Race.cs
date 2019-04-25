using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;


namespace Racing
{
    public class Race : MonoBehaviour
    {
        [SerializeField]
        private int num_race_drivers = 10;

        [SerializeField]
        private GameObject carAI;

        [SerializeField]
        public GameObject carPlayer;


        [SerializeField]
        private Transform start;

        private List<RaceDriver> race_drivers = new List<RaceDriver>();

        private static int position = 1;


        // Start is called before the first frame update
        void Start()
        {
            List<Vector3> start_positions = new List<Vector3>();
            float x = start.position.x - 10;
            float z = start.position.z - 10;



            for (int i = 0; i < num_race_drivers; i++)
            {
                start_positions.Add(new Vector3(x + 10 * (i % 3), start.position.y, z + 5 * i));

                if (i < num_race_drivers - 1)
                {
                    race_drivers.Add(Instantiate(carAI, start_positions[i], start.rotation).GetComponent<RaceDriver>());
                }
                else race_drivers.Add(Instantiate(carPlayer, start_positions[i], start.rotation).GetComponent<RaceDriver>());
            }

            race_drivers[num_race_drivers - 1].GetCar().GetComponent<CarController>().IsPlayer = true;
            race_drivers[num_race_drivers - 1].GetCar().tag = "PlayerDriver";

            foreach (RaceDriver g in race_drivers)
            {
                if (g.GetCar().tag != "PlayerDriver") g.GetCar().tag = "AIDriver";
                //g.GetCar().transform.Find("MinimapCar").transform.localScale = new Vector3(50f, 50f, 50f);
            }

            gameObject.GetComponent<Cameras>().StartCameras();

        }

        // Update is called once per frame
        void Update()
        {

        }





        public static int GetPosition()
        {
            position++;
            return position-1;
        }
    }

}
