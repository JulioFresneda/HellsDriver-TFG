using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class Canvas : MonoBehaviour
{
    public GameObject car;

    private Text[] texts;


    // Update is called once per frame
    void Update()
    {
        texts = this.GetComponentsInChildren<Text>();
        foreach( Text t in texts)
        {
            if(t.name == "Speed")
            {
                t.text = "Speed: " + Mathf.RoundToInt(car.GetComponent<CarController>().Speed) + " kmh";
            }
            else if(t.name == "Dist0")
            {
                t.text = "Dist 0: " + car.GetComponent<CarRaycast>().GetDistance(0);
            }
            else if (t.name == "Dist1")
            {
                t.text = "Dist 1: " + car.GetComponent<CarRaycast>().GetDistance(1);
            }
            else if (t.name == "Dist2")
            {
                t.text = "Dist 2: " + car.GetComponent<CarRaycast>().GetDistance(2);
            }
            else if (t.name == "Dist3")
            {
                t.text = "Dist 3: " + car.GetComponent<CarRaycast>().GetDistance(3);
            }
            else if (t.name == "Dist4")
            {
                t.text = "Dist 4: " + car.GetComponent<CarRaycast>().GetDistance(4);
            }
            else if (t.name == "Dist5")
            {
                t.text = "Dist 5: " + car.GetComponent<CarRaycast>().GetDistance(5);
            }
            else if (t.name == "Dist6")
            {
                t.text = "Dist 6: " + car.GetComponent<CarRaycast>().GetDistance(6);
            }
        }

        
    }
}
