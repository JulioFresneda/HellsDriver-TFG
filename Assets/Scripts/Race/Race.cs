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


        public GameObject circuitsManagement;

        public GameObject carPrefab;


   
        private Transform start = null;

    

        private List<RaceDriver> race_drivers = new List<RaceDriver>();
        private List<RaceDriver> race_drivers_AI;
        private RaceDriver race_driver_player;

        private static int setPosition = 1;

        private List<Tuple<RaceDriver, double>> CarsSorted = null;

        private static int totalLaps = 3;


        public CountDown countDown;




        private List<CarModelAI> allCarModels;




        // Start is called before the first frame update
        void Awake()
        {
            circuitsManagement.GetComponent<CircuitsManagement>().InitializeCircuit();
            start = circuitsManagement.GetComponent<CircuitsManagement>().GetStartPosition();


            totalLaps = PlayerPrefs.GetInt("lapNumber");
            allCarModels = LoadModels.GetAllCarModelAIs(PlayerPrefs.GetString("mapname"));

            OrderCarModels ocm = new OrderCarModels();
            allCarModels.Sort(ocm);



            InitializeDrivers();


        }


        // Update is called once per frame
        void Update()
        {

            UpdatePositions();
        }


        private void UpdatePositions()
        {
            for (int i = 0; i < CarsSorted.Count; i++)
            {
                CarsSorted[i] = new Tuple<RaceDriver, double>(CarsSorted[i].Item1, CarsSorted[i].Item1.GetDistanceNextCheckpoint());
            }

            CompareByPosition cbp = new CompareByPosition();
            CarsSorted.Sort(cbp);

            for (int i = 0; i < CarsSorted.Count; i++)
            {
                CarsSorted[i].Item1.SetCurrentPosition(i + 1);
            }
        }
        

        public static int GetPosition()
        {
            setPosition++;
            return setPosition-1;
        }




        private void InitializeDrivers()
        {
            List<Vector3> start_positions = new List<Vector3>();
            float x = start.position.x - 10;
            float z = start.position.z - 10;



            List<CarModelAI> driverCarModels = GenerateDriverCarModels(num_race_drivers - 1, PlayerPrefs.GetString("difficultSelected"));

            for (int i = 0; i < num_race_drivers; i++)
            {
                start_positions.Add(new Vector3(x + 10 * (i % 3), start.position.y, z + 4 * i));

                race_drivers.Add(Instantiate(carPrefab, start_positions[i], start.rotation).GetComponent<RaceDriver>());


                
                

            }

            SetAIDrivers();
            SetPlayerDriver();

          

            gameObject.GetComponent<Cameras>().StartCameras();


            CarsSorted = new List<Tuple<RaceDriver, double>>();
            foreach (RaceDriver r in race_drivers)
            {
                CarsSorted.Add(new Tuple<RaceDriver, double>(r, 0));
            }




            countDown.StartCountDown();

        }


        private void SetAIDrivers()
        {
            List<CarModelAI> AIModels = GenerateDriverCarModels(num_race_drivers - 1, PlayerPrefs.GetString("difficultSelected"));

            race_drivers_AI = new List<RaceDriver>();
            for(int i=0; i<AIModels.Count; i++)
            {
                race_drivers[i].SetCarModelAI(AIModels[i]);
                race_drivers_AI.Add(race_drivers[i]);
                race_drivers[i].GetCar().tag = "AIDriver";
            }

           
        }

        private void SetPlayerDriver()
        {
            race_driver_player = race_drivers[race_drivers.Count - 1];
            race_driver_player.GetCar().GetComponent<CarController>().IsPlayer = true;
            race_driver_player.GetCar().tag = "PlayerDriver";

            foreach(var model in LoadModels.GetAllCarModels())
            {
                if(model.GetModel() == PlayerPrefs.GetString("modelSelected"))
                {
                    race_driver_player.SetCarModel(model);
                }
            }
            


        }



        private List<CarModelAI> GenerateDriverCarModels(int numDriverCarModels, string difficult)
        {
            List<CarModelAI> list = new List<CarModelAI>();

            System.Random rnd = new System.Random();

            List<CarModelAI> temp = new List<CarModelAI>();

            if(difficult == "1")
            {
                temp = allCarModels.GetRange(0, 14);
            }
            if (difficult == "2")
            {
                temp = allCarModels.GetRange(14, 10);
            }
            if (difficult == "3")
            {
                temp = allCarModels.GetRange(24, 10);
            }
            if (difficult == "4")
            {
                temp = allCarModels.GetRange(34, 10);
            }
            if (difficult == "5")
            {
                temp = allCarModels.GetRange(44, 10);
            }


            for (int i = 0; i < numDriverCarModels; i++)
            {
                int r = rnd.Next(temp.Count);
                list.Add(temp[r]);
                temp.RemoveAt(r);
            }











            return list;


        }


        public static int GetTotalLaps() => totalLaps;








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

