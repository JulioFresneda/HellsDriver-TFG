using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{

    private GameObject playercamera, minimap;
    // Start is called before the first frame update
    public void StartCameras()
    {
        playercamera = GameObject.FindGameObjectWithTag("MainCamera");
        minimap = GameObject.Find("MinimapCamera");

        playercamera.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerDriver").transform);
        playercamera.transform.localPosition = new Vector3(0, 3, -8);
        playercamera.transform.localRotation = new Quaternion(0, 0, 0, 0);

        minimap.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerDriver").transform);
        minimap.transform.localPosition = new Vector3(0, 300, 0);
        minimap.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
