using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitsManagement : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> circuits = new List<GameObject>();

    private GameObject selectedCircuit;

    public void InitializeCircuit()
    {
        string selectedCircuitName = PlayerPrefs.GetString("mapname");
        foreach (GameObject c in circuits)
        {
            if (c.name.Contains(selectedCircuitName)) selectedCircuit = c;
            else c.SetActive(false);
        }
    }

    public Transform GetStartPosition()
    {
        
        return selectedCircuit.transform.Find("StartPosition").transform;

    }
}
