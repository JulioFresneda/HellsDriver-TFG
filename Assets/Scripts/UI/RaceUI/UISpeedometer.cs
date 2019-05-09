using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class UISpeedometer : MonoBehaviour
{
    private double CurrentNitro;
    private double MaxNitro;


    private GameObject PlayerCar;

    [SerializeField]
    private Transform NitroBar = null;

    [SerializeField]
    private Transform ClockHand = null;

    [SerializeField]
    private Transform DigitalSM = null;


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

            double speed = Mathf.Abs(PlayerCar.GetComponent<CarController>().Speed);
            ClockHand.transform.rotation = Quaternion.Euler(0, 0, (float)-(speed * 0.75 - 135));

            DigitalSM.GetComponent<Text>().text = Mathf.RoundToInt((float)speed) + " Km/h";

        }
    }
}
