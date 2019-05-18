using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;






public class UI_UseButtons : MonoBehaviour
{

    [SerializeField]
    private GameObject camera_selection = null;

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


    private List<VehicleSystem.CarModelAI> TierX, TierA, TierB;
    private int selection_position = 0;
    private int selection_tier = 0;


    private bool moving_right = false;
    private bool moving_left = false;
    private double last_x_position;
    private float displacement_speed;


    private Vector3 initial_position;


    // Start is called before the first frame update
    void Start()
    {
        displacement_speed = 100f;
        initial_position = camera_selection.transform.position;

        TierX = new List<VehicleSystem.CarModelAI>();
        TierA = new List<VehicleSystem.CarModelAI>();
        TierB = new List<VehicleSystem.CarModelAI>();

        //AQUÍ SE LEEN LOS COCHES
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving_right)
        {
            if(last_x_position + 600 > camera_selection.transform.position.x)
            {
                camera_selection.transform.position += new Vector3(displacement_speed, 0, 0);
            }
            else
            {
                moving_right = false;
            }
        }
        else if (moving_left)
        {
            if (last_x_position - 600 < camera_selection.transform.position.x)
            {
                camera_selection.transform.position -= new Vector3(displacement_speed, 0, 0);
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
            last_x_position = camera_selection.transform.position.x;
            moving_right = true;
        }
        
    }

    public void LeftClick()
    {
        if (!moving_left && selection_position > 0)
        {
            selection_position--;
            last_x_position = camera_selection.transform.position.x;
            moving_left = true;
        }

    }


    public void TierB_Button()
    {
        if (selection_tier != 0)
        {
            selection_tier = 0;
            camera_selection.transform.position = initial_position;
            selection_position = 0;
        }
    }

    public void TierA_Button()
    {
        if (selection_tier != 1)
        {
            selection_tier = 1;
            camera_selection.transform.position = initial_position + new Vector3(0,0,600);
            selection_position = 0;
        }
    }

    public void TierX_Button()
    {
        if (selection_tier != 2)
        {
            selection_tier = 2;
            camera_selection.transform.position = initial_position + new Vector3(0, 0, 1200);
            selection_position = 0;
        }
    }


    public void StartRaceButton()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("mapname")+"Circuit");
    }
}
