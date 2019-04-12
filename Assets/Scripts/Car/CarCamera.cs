using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{

    private GameObject playercamera;
    private GameObject minimap;

    // Start is called before the first frame update
    void Start()
    {
        
        if (this.gameObject.tag == "PlayerDriver")
        {
            playercamera = GameObject.FindGameObjectWithTag("MainCamera");
            minimap = GameObject.FindGameObjectWithTag("Minimap");

            playercamera.transform.SetParent(gameObject.transform);
            playercamera.transform.localPosition = new Vector3(0, 3, -8);
            playercamera.transform.localRotation = new Quaternion(0, 0, 0, 0);

            minimap.transform.SetParent(gameObject.transform);
            minimap.transform.localPosition = new Vector3(0, 300, 0);
            minimap.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);


        }

        
    }
    


}
