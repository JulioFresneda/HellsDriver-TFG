using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class RaijinRotation : MonoBehaviour
{

    public float constant = 1;
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, 1 * GetComponentInParent<CarController>().Speed * constant));
    }
}
