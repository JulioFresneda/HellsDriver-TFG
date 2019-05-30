using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManagement : MonoBehaviour
{

    public GameObject EightLight, DizzyLight, WhirlLight, SubwayLight;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("mapname") == "Eight")
        {
            EightLight.SetActive(true);
            DizzyLight.SetActive(false);
            WhirlLight.SetActive(false);
            SubwayLight.SetActive(false);
        }
        if (PlayerPrefs.GetString("mapname") == "Dizzy")
        {
            DizzyLight.SetActive(true);
            EightLight.SetActive(false);
            WhirlLight.SetActive(false);
            SubwayLight.SetActive(false);
        }
        if (PlayerPrefs.GetString("mapname") == "Whirl")
        {
            WhirlLight.SetActive(true);
            DizzyLight.SetActive(false);
            EightLight.SetActive(false);
            SubwayLight.SetActive(false);
        }
        if (PlayerPrefs.GetString("mapname") == "Subway")
        {
            SubwayLight.SetActive(true);
            DizzyLight.SetActive(false);
            WhirlLight.SetActive(false);
            EightLight.SetActive(false);
        }

    }

    
}
