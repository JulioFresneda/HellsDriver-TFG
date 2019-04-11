using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class Race : MonoBehaviour
{
    [SerializeField]
    private int num_race_drivers = 10;

    [SerializeField]
    private GameObject carAI;

    [SerializeField]
    public GameObject carPlayer;


    [SerializeField]
    private Transform start;

    private List<GameObject> race_drivers = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
      
        

        List<Vector3> start_positions = new List<Vector3>();
        float x = start.position.x - 10;
        float z = start.position.z - 10;



        for(int i=0; i<num_race_drivers; i++)
        {
            start_positions.Add(new Vector3(x+ 10 * (i % 3), start.position.y, z+5*i));
     
            if(i<num_race_drivers-1) race_drivers.Add(Instantiate(carAI, start_positions[i],start.rotation));   
            else race_drivers.Add(Instantiate(carPlayer, start_positions[i], start.rotation));
        }

        race_drivers[num_race_drivers - 1].GetComponent<CarController>().IsPlayer = true;
        race_drivers[num_race_drivers - 1].tag = "PlayerDriver";

        foreach (GameObject g in race_drivers)
        {
            if (g.tag != "PlayerDriver") g.tag = "AIDriver";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
