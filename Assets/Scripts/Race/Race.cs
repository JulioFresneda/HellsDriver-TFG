using NeuralNet;
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
        private Transform start;

        [SerializeField]
        private string CarAIName;

        private List<RaceDriver> race_drivers = new List<RaceDriver>();

        private static int position = 1;


        // Start is called before the first frame update
        void Start()
        {
            List<Vector3> start_positions = new List<Vector3>();
            float x = start.position.x - 10;
            float z = start.position.z - 10;


            NNToFile ntf;
            SetCarProperties();

            for (int i = 0; i < num_race_drivers; i++)
            {
                start_positions.Add(new Vector3(x + 10 * (i % 3), start.position.y, z + 8 * i));

                race_drivers.Add(Instantiate(carAI, start_positions[i], start.rotation).GetComponent<RaceDriver>());
                
                

                ntf = new NNToFile();
                race_drivers[i].GetComponent<CarAI>().SetNeuralNetwork(ntf.Read(CarAIName + ".txt"));


                

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



        private void SetCarProperties()
        {
            string temp = CarAIName;
            temp = temp.Substring(3);

            string throttlestring = "";
            throttlestring += temp[0];

            if (temp[1] != '_')
            {
                throttlestring += temp[1];
                temp = temp.Substring(4);
            }
            else
            {
                temp = temp.Substring(3);
            }

            string massstring = temp.Substring(0, 4);

            temp = temp.Substring(5);

            string steeringanglestring = "";
            steeringanglestring += temp[0];
            steeringanglestring+= temp[1];

            string stiffnessstring = "";
            stiffnessstring += temp[temp.Length - 1];


            carAI.GetComponent<CarController>().Throttle = int.Parse(throttlestring);
            carAI.GetComponent<Rigidbody>().mass = int.Parse(massstring);


            WheelFrictionCurve wfc = new WheelFrictionCurve();
            wfc.extremumSlip = 0.4f;
            wfc.extremumValue = 1f;
            wfc.asymptoteSlip = 0.5f;
            wfc.asymptoteValue = 0.75f;
            wfc.stiffness = int.Parse(stiffnessstring);

            foreach(WheelCollider wc in carAI.GetComponentsInChildren<WheelCollider>())
            {
                wc.sidewaysFriction = wfc;
            }


        }

        public static int GetPosition()
        {
            position++;
            return position-1;
        }
    }

}
