using NeuralNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class LoadModels : MonoBehaviour
{


    public string mapname;
    private string rute;

    private List<CarModel> carModels;
    private List<Tuple<int, int, int>> carValues;


    // Start is called before the first frame update
    public void LoadCarModels()
    {
        /*
        carModels = new List<CarModelAI>();
        LoadCarValues();
        rute = "AIs/" + mapname + "AI/";

        foreach(var value in carValues)
        {
            carModels.Add(GetCarProperties("car" + value.Item1 + "_" + value.Item2 + "_" + value.Item3 + ".txt"));
        }

        OrderCarModels ocm = new OrderCarModels();
        carModels.Sort(ocm);
        */

        mapname = PlayerPrefs.GetString("mapname");

        LoadCarValues();

        carModels = new List<CarModel>();
        foreach(var value in carValues)
        {
            if(value.Item1 > 11)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Raijin", "R" + value.Item1 + value.Item3,value.Item2,value.Item3,value.Item1));
                else if(value.Item2 == 2000) carModels.Add(new CarModel("Poseidon", "P" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("Leviathan", "L" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
            else if (value.Item1 > 8)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Valkyria", "V" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2000) carModels.Add(new CarModel("Xlynx", "X" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("DJED", "D" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
            else if(value.Item1 > 5)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Duck", "Du" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2000) carModels.Add(new CarModel("Audidas", "A" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("Hoa", "H" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void LoadCarValues()
    {
        carValues = new List<Tuple<int, int, int>>();

        List<int> throttleList = new List<int>();
        for (int i = 6; i < 15; i++) throttleList.Add(i);

        List<int> massList = new List<int>();
        massList.Add(1500);
        massList.Add(2000);
        massList.Add(2500);


        List<int> sidewaysFrictionList = new List<int>();
        sidewaysFrictionList.Add(3);
        sidewaysFrictionList.Add(4);


        foreach (int t in throttleList)
        {
            foreach (int m in massList)
            {

                foreach (int sf in sidewaysFrictionList)
                {
                    carValues.Add(new Tuple<int, int, int>(t, m, sf));
                }

            }
        }
    }



    public List<CarModel> GetCarModels() => carModels;


    private CarModelAI GetCarProperties(string CarAIName)
    {
        string temp = CarAIName;
        temp = temp.Substring(3);

        string throttlestring = "";
        throttlestring += temp[0];

        if (temp[1] != '_')
        {
            throttlestring += temp[1];
            temp = temp.Substring(3);
        }
        else
        {
            temp = temp.Substring(2);
        }

        string massstring = temp.Substring(0, 4);

        temp = temp.Substring(5);


        string stiffnessstring = "";
        stiffnessstring += temp[0];

        NNToFile ntf = new NNToFile();
        NeuralNetwork nn = ntf.Read(rute + CarAIName);
        return new CarModelAI("", "", int.Parse(massstring), int.Parse(stiffnessstring), int.Parse(throttlestring), mapname, nn.GetFitness(), nn);


    }
}
