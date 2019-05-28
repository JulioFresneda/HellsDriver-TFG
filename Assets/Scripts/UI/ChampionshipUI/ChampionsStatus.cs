using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionsStatus : MonoBehaviour
{
    public GameObject Eight, Dizzy, Whirl, Subway;
    public Texture EightT, DizzyT, WhirlT, SubwayT;


    private List<GameObject> maps;

    private string currentMap;


    // Start is called before the first frame update
    void Start()
    {
        maps = new List<GameObject>();
        maps.Add(Eight);
        maps.Add(Dizzy);
        maps.Add(Whirl);
        maps.Add(Subway);

        currentMap = PlayerPrefs.GetString("ChampCurrentMap");

        LoadData();


        if (currentMap == "Eight") Eight.GetComponent<RawImage>().texture = EightT;
        if (currentMap == "Dizzy") Eight.GetComponent<RawImage>().texture = DizzyT;
        if (currentMap == "Whirl") Eight.GetComponent<RawImage>().texture = WhirlT;
        if (currentMap == "Subway") Eight.GetComponent<RawImage>().texture = SubwayT;



    }


    private void LoadData()
    {

        

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
            Eight.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampEightTime")/60).ToString() + " min " + (PlayerPrefs.GetFloat("ChampEightTime") % 60).ToString() + " s";
            Eight.transform.Find("CarModel").GetComponent<Text>().text = PlayerPrefs.GetString("ChampEightCarModel");
            Eight.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampEightPoints").ToString() + " points";
        }
        if(currentMap == "Whirl" || currentMap == "Subway" || currentMap == "NoMore")
        {
            Eight.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampDizzyPosition").ToString() + " posicion";
            Eight.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampDizzyTime") / 60).ToString() + " min " + (PlayerPrefs.GetFloat("ChampEightTime") % 60).ToString() + " s";
            Eight.transform.Find("CarModel").GetComponent<Text>().text = PlayerPrefs.GetString("ChampDizzyCarModel");
            Eight.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampDizzyPoints").ToString() + " points";
        }
        if (currentMap == "Subway" || currentMap == "NoMore")
        {
            Eight.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampWhirlPosition").ToString() + " posicion";
            Eight.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampWhirlTime") / 60).ToString() + " min " + (PlayerPrefs.GetFloat("ChampEightTime") % 60).ToString() + " s";
            Eight.transform.Find("CarModel").GetComponent<Text>().text = PlayerPrefs.GetString("ChampWhirlCarModel");
            Eight.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampWhirlPoints").ToString() + " points";
        }
        if(currentMap == "NoMore")
        {
            Eight.transform.Find("Position").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampSubwayPosition").ToString() + " posicion";
            Eight.transform.Find("Time").GetComponent<Text>().text = (PlayerPrefs.GetFloat("ChampSubwayTime") / 60).ToString() + " min " + (PlayerPrefs.GetFloat("ChampEightTime") % 60).ToString() + " s";
            Eight.transform.Find("CarModel").GetComponent<Text>().text = PlayerPrefs.GetString("ChampSubwayCarModel");
            Eight.transform.Find("Points").GetComponent<Text>().text = PlayerPrefs.GetInt("ChampSubwayPoints").ToString() + " points";
        }


    }


}
