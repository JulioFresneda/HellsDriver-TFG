using NeuralNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class MenuCar : MonoBehaviour
{

    [SerializeField]
    GameObject[] cars = null;

    [SerializeField]
    string CarAIName = "car0__2000_3";


    // Start is called before the first frame update
    void Start()
    {
        NNToFile ntf = new NNToFile();
        foreach(GameObject c in cars)
        {
            c.GetComponent<CarAI>().SetNeuralNetwork(ntf.Read(CarAIName + ".txt"));
        }
        
    }

}
