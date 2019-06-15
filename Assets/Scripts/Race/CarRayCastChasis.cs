using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRayCastChasis : MonoBehaviour
{
    private List<GameObject> cars;
    private List<GameObject> colliders;
    public void LoadChasis()
    {
        cars = new List<GameObject>();
        cars.Add(GameObject.FindGameObjectWithTag("PlayerDriver"));
        cars.AddRange(GameObject.FindGameObjectsWithTag("AIDriver"));

        colliders = new List<GameObject>();

        foreach(GameObject g in cars)
        {
            GameObject parent = Instantiate(new GameObject(), this.transform);
            colliders.Add(Instantiate(g.transform.Find("ChasisCollider").gameObject, parent.transform));
            colliders[colliders.Count - 1].gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cars != null)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                colliders[i].transform.position = cars[i].transform.position + new Vector3(0, 100, 0);
                colliders[i].transform.rotation = cars[i].transform.rotation;
            }
        }
        
    }
}
