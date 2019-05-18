using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CR_SelectMapUI : MonoBehaviour
{

    private string mapSelected = "";
    private int lapNumberSelected = 1;
    private string difficultSelected = "2";




    // Update is called once per frame
    void Update()
    {
        string mapname = PlayerPrefs.GetString("mapname");
        if (mapname != null) Debug.Log(mapname);
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
