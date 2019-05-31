using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLightsManagement : MonoBehaviour
{
    public GameObject carLight;



    // Start is called before the first frame update
    void Start()
    {
 
        GameObject.FindGameObjectWithTag("PlayerDriver");


        carLight.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerDriver").transform);
        carLight.transform.localPosition = Vector3.zero;
        carLight.transform.localRotation = Quaternion.identity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
