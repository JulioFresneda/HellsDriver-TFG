using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class SpeedometerUI : MonoBehaviour
{

    private double CurrentNitro;
    private double MaxNitro;


    private GameObject PlayerCar;

    [SerializeField]
    private Transform NitroBar;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCar == null) PlayerCar = GameObject.FindGameObjectWithTag("PlayerDriver");
        else
        {
            CurrentNitro = PlayerCar.GetComponent<CarController>().Boost;
            MaxNitro = PlayerCar.GetComponent<CarController>().MaxBoost;

            NitroBar.GetComponent<Image>().fillAmount = (float)CurrentNitro / (float)MaxNitro;
        }
    }
}
