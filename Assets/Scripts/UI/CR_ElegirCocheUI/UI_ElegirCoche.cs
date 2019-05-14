using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;






public class UI_ElegirCoche : MonoBehaviour
{

    [SerializeField]
    private GameObject camera = null;

    [SerializeField]
    private Button Bclass = null;

    [SerializeField]
    private Button Aclass = null;

    [SerializeField]
    private Button Xclass = null;

    [SerializeField]
    private Button left = null;

    [SerializeField]
    private Button right = null;


    private List<VehicleSystem.CarModel> TierX, TierA, TierB;
    private int selection_position = 0;
    private int selectio_tier = 0;


    private bool moving_right = false;
    private bool moving_left = false;
    private double last_x_position;
    private float displacement_speed;


    // Start is called before the first frame update
    void Start()
    {
        displacement_speed = 100f;

        TierX = new List<VehicleSystem.CarModel>();
        TierA = new List<VehicleSystem.CarModel>();
        TierB = new List<VehicleSystem.CarModel>();

        //AQUÍ SE LEEN LOS COCHES
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving_right)
        {
            if(last_x_position + 600 > camera.transform.position.x)
            {
                camera.transform.position += new Vector3(displacement_speed, 0, 0);
            }
            else
            {
                moving_right = false;
            }
        }
        else if (moving_left)
        {
            if (last_x_position - 600 < camera.transform.position.x)
            {
                camera.transform.position -= new Vector3(displacement_speed, 0, 0);
            }
            else
            {
                moving_left = false;
            }
        }
    }



    public void RightClick()
    {
        if (!moving_right)
        {
            selection_position++;
            last_x_position = camera.transform.position.x;
            moving_right = true;
        }
        
    }

    public void LeftClick()
    {
        if (!moving_left && selection_position > 0)
        {
            selection_position--;
            last_x_position = camera.transform.position.x;
            moving_left = true;
        }

    }
}
