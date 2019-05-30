using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitsManagement : MonoBehaviour
{

    private GameObject selectedCircuit;



    public Transform GetStartPosition()
    {
        
        return selectedCircuit.transform.Find("StartPosition").transform;

    }
}
