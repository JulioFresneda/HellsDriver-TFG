using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class FinishedWindow : MonoBehaviour
{



    public GameObject driverNames, carModels, driverTimes;

    public PointsManagement pointsManagement;

    void Awake()
    {
        GameObject.Find("RaceFinished").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("PointsManagement").transform.localScale = new Vector3(0, 0, 0);   
    }

    public void SetDriver(string drivername, int finalPosition, float seconds, CarModel carModel)
    {
        driverNames.transform.Find(finalPosition.ToString()).GetComponent<Text>().text = drivername;
        carModels.transform.Find(finalPosition.ToString()).GetComponent<Text>().text = carModel.GetBrand() + " " + carModel.GetModel();
        driverTimes.transform.Find(finalPosition.ToString()).GetComponent<Text>().text = (seconds / 60).ToString()[0] + " min " + (seconds % 60).ToString().Substring(0,2) + " s";


    }

}
