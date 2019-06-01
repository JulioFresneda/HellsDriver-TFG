using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMapUI : MonoBehaviour
{

    private string mapSelected = "Eight";
    private int lapNumberSelected = 1;
    private string difficultSelected = "2";


    public GameObject difficultMultiplier, lapNumberMultiplier;
    public List<float> difficultMultipliers, lapNumberMultipliers;

    private void Start()
    {
        PlayerPrefs.SetString("CurrentMap", "Eight");
        PlayerPrefs.SetInt("lapNumber", 1);
        PlayerPrefs.SetString("difficultSelected", "2");
    }


    public void MapButtonClick(string map)
    {
        mapSelected = map;
        PlayerPrefs.SetString("CurrentMap", mapSelected);
        Debug.Log(mapSelected);
     
    }

    public void LapNumberButtonClick(int number)
    {
        lapNumberSelected = number;
        PlayerPrefs.SetInt("lapNumber", lapNumberSelected);
  


        int i = number - 1;

        string ln = lapNumberMultipliers[i].ToString();

        
        lapNumberMultiplier.GetComponentInChildren<Text>().text = "x" + ln.Replace(',', '.'); 

        PlayerPrefs.SetFloat("lapNumberMult", lapNumberMultipliers[i]);
    }

    public void DifficultButtonClick(string dif)
    {
        
        difficultSelected = dif;

        int i = 0;
        if (dif == "2") i = 1;
        if (dif == "3") i = 2;
        if (dif == "4") i = 3;
        if (dif == "5") i = 4;
        if (dif == "D") i = 5;

        difficultMultiplier.GetComponentInChildren<Text>().text = "x" + difficultMultipliers[i].ToString().Replace(',','.');

        PlayerPrefs.SetString("difficultSelected", difficultSelected);
        PlayerPrefs.SetFloat("difficultMult", difficultMultipliers[i]);

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
