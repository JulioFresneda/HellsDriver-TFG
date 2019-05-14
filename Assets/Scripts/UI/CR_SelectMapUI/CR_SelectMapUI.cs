using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CR_SelectMapUI : MonoBehaviour
{

    private string mapSelected = "";
    private int lapNumberSelected = 1;
    private int difficultSelected = 2;
    private bool dinamicallyDifficult = false;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MapButtonClick(Button b)
    {
        mapSelected = b.GetComponentInChildren<Text>().text;
     
    }

    public void LapNumberButtonClick(Button b)
    {
        lapNumberSelected = int.Parse(b.GetComponentInChildren<Text>().text);
    
    }

    public void DifficultButtonClick(Button b)
    {
        if (b.GetComponentInChildren<Text>().text[0] == 'D') dinamicallyDifficult = true;
        else
        {
            dinamicallyDifficult = false;
            difficultSelected = int.Parse(b.GetComponentInChildren<Text>().text);
        }

    }
}
