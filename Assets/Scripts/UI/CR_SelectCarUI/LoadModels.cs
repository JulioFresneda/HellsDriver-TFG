using NeuralNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class LoadModels 
{


    public static List<CarModelAI> GetAllCarModelAIs(string mapname)
    {
        List<CarModelAI> listcmai = new List<CarModelAI>();
        foreach(CarModel model in GetAllCarModels())
        {
            listcmai.Add(GetModelAI(model, mapname));
        }
        return listcmai;
    }



    private static List<Tuple<int, int, int>> GetAllPossibleCarValues()
    {
        List<Tuple<int, int, int>>  carValues = new List<Tuple<int, int, int>>();

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

        return carValues;
    }






    public static List<CarModel> GetAllCarModels()
    {
        

        List<CarModel> carModels;
        carModels = new List<CarModel>();
        foreach (var value in GetAllPossibleCarValues())
        {
            if (value.Item1 > 11)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Raijin", "R" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2000) carModels.Add(new CarModel("Poseidon", "P" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("Leviathan", "L" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
            else if (value.Item1 > 8)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Valkyria", "V" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2000) carModels.Add(new CarModel("Xlynx", "X" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("DJED", "D" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
            else if (value.Item1 > 5)
            {
                if (value.Item2 == 1500) carModels.Add(new CarModel("Duck", "Du" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2000) carModels.Add(new CarModel("Audidas", "A" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
                else if (value.Item2 == 2500) carModels.Add(new CarModel("Hoa", "H" + value.Item1 + value.Item3, value.Item2, value.Item3, value.Item1));
            }
        }

        return carModels;
    }


    public static CarModelAI GetModelAI(string CarAIName, string mapname)
    {

        string rute = "AIs/" + mapname + "AI/";

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


    public static CarModelAI GetModelAI(CarModel carModel, string mapname)
    {

        string rute = "AIs/" + mapname + "AI/";

        string CarAIName = "car" + carModel.GetThrottle() + "_" + carModel.GetMass() + "_" + carModel.GetStiffness() + ".txt";





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
        return new CarModelAI(carModel.GetBrand(), carModel.GetModel(), int.Parse(massstring), int.Parse(stiffnessstring), int.Parse(throttlestring), mapname, nn.GetFitness(), nn);


    }
}
