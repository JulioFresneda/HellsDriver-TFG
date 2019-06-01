using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{

    private GameObject playercamera, minimap;
    private GameObject mapBorder;

    [SerializeField]
    private Vector3 cameraPosition = new Vector3(0,4,-12);

    [SerializeField]
    private float cameraRotation = 0f;

    private GameObject player;
    public void StartCameras()
    {
        playercamera = GameObject.FindGameObjectWithTag("MainCamera");
        minimap = GameObject.Find("MinimapCamera");
        mapBorder = GameObject.Find("MapBorder");

        player = GameObject.FindGameObjectWithTag("PlayerDriver");
        playercamera.transform.SetParent(player.transform);
        playercamera.transform.localPosition = cameraPosition;
        playercamera.transform.localRotation = new Quaternion(cameraRotation, 0, 0, 0);

        minimap.transform.position = new Vector3(player.transform.position.x, player.transform.position.y+300, player.transform.position.z);
        minimap.transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        minimap.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 300, player.transform.position.z);
        minimap.transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0);
        mapBorder.transform.rotation = Quaternion.Euler(0,0, -180.0f +player.transform.eulerAngles.y);
    }
}
