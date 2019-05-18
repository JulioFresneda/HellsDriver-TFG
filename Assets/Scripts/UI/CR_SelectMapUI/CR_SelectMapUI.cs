using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CR_SelectMapUI : MonoBehaviour
{

    private string mapSelected = "Eight";
    private int lapNumberSelected = 1;
    private string difficultSelected = "2";




    private void Start()
    {
        PlayerPrefs.SetString("mapname", "Eight");
        PlayerPrefs.SetInt("lapNumber", 1);
        PlayerPrefs.SetString("difficultSelected", "2");
    }


    public void MapButtonClick(Button b)
    {
        mapSelected = b.GetComponentInChildren<Text>().text;
        PlayerPrefs.SetString("mapname", mapSelected);
     
    }

    public void LapNumberButtonClick(Button b)
    {
        lapNumberSelected = int.Parse(b.GetComponentInChildren<Text>().text);
        PlayerPrefs.SetInt("lapNumber", lapNumberSelected);
    
    }

    public void DifficultButtonClick(Button b)
    {
        if (b.GetComponentInChildren<Text>().text[0] == 'D') difficultSelected = "D";
        else
        {
            difficultSelected = b.GetComponentInChildren<Text>().text;
        }

        PlayerPrefs.SetString("difficultSelected", difficultSelected);

    }

    public void SelectCarButtonClick(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
