using NeuralNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class MenuCar : MonoBehaviour
{

    [SerializeField]
    GameObject car = null;

    [SerializeField]
    string CarAIName = "car0__2000_3";


    // Start is called before the first frame update
    void Start()
    {
        NNToFile ntf = new NNToFile();
        car.GetComponent<CarAI>().SetNeuralNetwork(ntf.Read(CarAIName + ".txt"));
    }

}
