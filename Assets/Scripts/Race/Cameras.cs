using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{

    private GameObject playercamera, minimap;


    private GameObject player;
    public void StartCameras()
    {
        playercamera = GameObject.FindGameObjectWithTag("MainCamera");
        minimap = GameObject.Find("MinimapCamera");

        player = GameObject.FindGameObjectWithTag("PlayerDriver");
        playercamera.transform.SetParent(player.transform);
        playercamera.transform.localPosition = new Vector3(0, 3, -8);
        playercamera.transform.localRotation = new Quaternion(0, 0, 0, 0);

        minimap.transform.position = new Vector3(player.transform.position.x, 300, player.transform.position.z);
        minimap.transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        minimap.transform.position = new Vector3(player.transform.position.x, 300, player.transform.position.z);
        minimap.transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0);
    }
}
