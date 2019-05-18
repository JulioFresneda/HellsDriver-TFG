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


    public void MapButtonClick(string map)
    {
        mapSelected = map;
        PlayerPrefs.SetString("mapname", mapSelected);
        Debug.Log(mapSelected);
     
    }

    public void LapNumberButtonClick(int number)
    {
        lapNumberSelected = number;
        PlayerPrefs.SetInt("lapNumber", lapNumberSelected);
        Debug.Log(lapNumberSelected);
    }

    public void DifficultButtonClick(string dif)
    {
        
        difficultSelected = dif;
        

        PlayerPrefs.SetString("difficultSelected", difficultSelected);
        Debug.Log(difficultSelected);
    }

    public void SelectCarButtonClick(string scene)
    {
        SceneManager.LoadScene(scene);
    }


    public void GoBackButtonClick(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
