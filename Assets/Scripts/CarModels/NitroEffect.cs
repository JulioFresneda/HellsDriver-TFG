using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroEffect : MonoBehaviour
{


    private VehicleSystem.CarController controller;
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
        controller = gameObject.GetComponentInParent<VehicleSystem.CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsBoosting() && !playing)
        {

            playing = true;
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        if(!controller.IsBoosting() && playing)
        {
            playing = false;
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }
}
