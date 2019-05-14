using Racing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPositionAndLap : MonoBehaviour
{

    [SerializeField]
    private Text PositionText = null;

    [SerializeField]
    private Text LapText = null;

    private GameObject PlayerCar;

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
            PositionText.text = PlayerCar.GetComponent<RaceDriver>().GetCurrentPosition().ToString();
            LapText.text = PlayerCar.GetComponent<RaceDriver>().GetCurrentLap().ToString() + "/" + Race.GetTotalLaps();
        }
    }
}
