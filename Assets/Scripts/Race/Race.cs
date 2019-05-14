using NeuralNet;
using Racing;
using System;
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
        private GameObject carAI = null;



        [SerializeField]
        private Transform start = null;

        [SerializeField]
        private string CarAIName = "";

        private List<RaceDriver> race_drivers = new List<RaceDriver>();

        private static int setPosition = 1;

        private List<Tuple<RaceDriver, double>> CarsSorted = null;


        [SerializeField]
        private static int totalLaps = 3;

        public static int GetTotalLaps() => totalLaps;


        // Start is called before the first frame update
        void Start()
        {
            List<Vector3> start_positions = new List<Vector3>();
            float x = start.position.x - 10;
            float z = start.position.z - 10;


            NNToFile ntf;
            //SetCarProperties();

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


            CarsSorted = new List<Tuple<RaceDriver, double>>();
            foreach(RaceDriver r in race_drivers)
            {
                CarsSorted.Add(new Tuple<RaceDriver, double>(r, 0));
            }

        }

        // Update is called once per frame
        void Update()
        {

            for(int i=0; i<CarsSorted.Count; i++)
            { 
                CarsSorted[i] = new Tuple<RaceDriver, double>(CarsSorted[i].Item1, CarsSorted[i].Item1.GetDistanceNextCheckpoint());
            }

            CompareByPosition cbp = new CompareByPosition();
            CarsSorted.Sort(cbp);

            for(int i=0; i<CarsSorted.Count; i++)
            {
                CarsSorted[i].Item1.SetCurrentPosition(i + 1);
            }
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
            setPosition++;
            return setPosition-1;
        }
    }

}


public class CompareByPosition : IComparer<Tuple<RaceDriver,double>>
{
    public int Compare(Tuple<RaceDriver, double> x, Tuple<RaceDriver, double> y)
    {
        if (x.Item1.GetCurrentLap() < y.Item1.GetCurrentLap()) return 1;
        else if (x.Item1.GetCurrentLap() > y.Item1.GetCurrentLap()) return -1;
        else
        {
            if (x.Item1.GetNumCheckpointsChecked() < y.Item1.GetNumCheckpointsChecked()) return 1;
            else if ((x.Item1.GetNumCheckpointsChecked() > y.Item1.GetNumCheckpointsChecked())) return -1;
            else
            {
                if (x.Item2 < y.Item2) return -1;
                else return 1;
            }

        }

    }
}

