using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour
{

  

    public void OnClick(GameObject g)
    {
        g.transform.localScale = Vector3.zero;
    }
}
