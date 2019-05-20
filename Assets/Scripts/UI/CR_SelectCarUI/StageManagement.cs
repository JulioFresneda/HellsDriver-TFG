using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagement : MonoBehaviour
{
    public GameObject stage;
    public float rotationVelocity = 0.01f;
    // Update is called once per frame
    void Update()
    {
        stage.transform.Rotate(new Vector3(0, 0, rotationVelocity));
    }
}
