using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChampionsStatus : MonoBehaviour
{
    public GameObject Eight, Dizzy, Whirl, Subway;
    public Texture EightT, DizzyT, WhirlT, SubwayT;


    private List<GameObject> maps;

    private string currentMap;

    public Button startButton;


    // Start is called before the first frame update
    void Start()
    {
        maps = new List<GameObject>();
        maps.Add(Eight);
        maps.Add(Dizzy);
        maps.Add(Whirl);
        maps.Add(Subway);


        currentMap = PlayerPrefs.GetString("CurrentMap");

        LoadData();


        if (currentMap == "Eight") Eight.GetComponent<RawImage>().texture = EightT;
        if (currentMap == "Dizzy") Dizzy.GetComponent<RawImage>().texture = DizzyT;
        if (currentMap == "Whirl") Whirl.GetComponent<RawImage>().texture = WhirlT;
        if (currentMap == "Subway") Subway.GetComponent<RawImage>().texture = SubwayT;
        if(currentMap == "NoMore")
        {
            startButton.GetComponentInChildren<Text>().text = "Premios";
            
        }


    }


    public void StartButtonClick()
    {
        if (currentMap != "NoMore") SceneManager.LoadScene("SelectCar");
        else SceneManager.LoadScene("ChampionshipAwards");
    }


    private void LoadData()
    {

        Debug.Log("CURRENT MAP " + currentMap);

        foreach (GameObject g in maps)
        {
            foreach (Text t in g.GetComponentsInChildren<Text>())
            {
                t.text = "···";
            }
        }
      
        if(currentMap == "Dizzy" || currentMap == "Whirl" || currentMap == "Subway" || currentMap == "NoMore")
        {
            Eight.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampEightPosition").ToString() + " posicion";
            Eight.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampEightTime") / 60).ToString()[0] + " min " + (PlayerPrefs.GetFloat("ChampEightTime") % 60).ToString().Substring(0, 2) + " s";
            Eight.transform.Find("CarModel").GetComponent<Text>().text = "Modelo " + PlayerPrefs.GetString("ChampEightCarModel");
            Eight.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampEightPoints").ToString() + " puntos";
        }
        if(currentMap == "Whirl" || currentMap == "Subway" || currentMap == "NoMore")
        {
            Dizzy.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampDizzyPosition").ToString() + " posicion";
            Dizzy.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampDizzyTime") / 60).ToString()[0] + " min " + (PlayerPrefs.GetFloat("ChampDizzyTime") % 60).ToString().Substring(0, 2) + " s";
            Dizzy.transform.Find("CarModel").GetComponent<Text>().text = "Modelo " + PlayerPrefs.GetString("ChampDizzyCarModel");
            Dizzy.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampDizzyPoints").ToString() + " puntos";
        }
        if (currentMap == "Subway" || currentMap == "NoMore")
        {
            Whirl.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampWhirlPosition").ToString() + " posicion";
            Whirl.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampWhirlTime") / 60).ToString()[0] + " min " + (PlayerPrefs.GetFloat("ChampWhirlTime") % 60).ToString().Substring(0, 2) + " s";
            Whirl.transform.Find("CarModel").GetComponent<Text>().text = "Modelo " + PlayerPrefs.GetString("ChampWhirlCarModel");
            Whirl.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampWhirlPoints").ToString() + " puntos";
        }
        if(currentMap == "NoMore")
        {
            Subway.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampSubwayPosition").ToString() + " posicion";
            Subway.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampSubwayTime") / 60).ToString()[0] + " min " + (PlayerPrefs.GetFloat("ChampSubwayTime") % 60).ToString().Substring(0, 2) + " s";
            Subway.transform.Find("CarModel").GetComponent<Text>().text = "Modelo " + PlayerPrefs.GetString("ChampSubwayCarModel");
            Subway.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampSubwayPoints").ToString() + " puntos";
        }


    }


}
