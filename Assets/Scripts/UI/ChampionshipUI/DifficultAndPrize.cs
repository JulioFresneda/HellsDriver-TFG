using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultAndPrize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string difficultSelected;
    public GameObject difficultMultiplier;
    public List<float> difficultMultipliers;

    


    public void DifficultButtonClick(string dif)
    {

        
   
        difficultSelected = dif;

        int i = 0;
        if (dif == "2") i = 1;
        if (dif == "3") i = 2;
        if (dif == "4") i = 3;
        if (dif == "5") i = 4;
        if (dif == "D") i = 5;

        difficultMultiplier.GetComponentInChildren<Text>().text = "x" + difficultMultipliers[i].ToString().Replace(',', '.');

        PlayerPrefs.SetString("difficultSelected", difficultSelected);
        PlayerPrefs.SetFloat("difficultMult", difficultMultipliers[i]);


        if(i == 0)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 200.ToString();
            PlayerPrefs.SetInt("Champ1", 200);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 100.ToString();
            PlayerPrefs.SetInt("Champ2", 100);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 50.ToString();
            PlayerPrefs.SetInt("Champ3", 50);
        }
        if (i == 1)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 300.ToString();
            PlayerPrefs.SetInt("Champ1", 300);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 150.ToString();
            PlayerPrefs.SetInt("Champ2", 150);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 75.ToString();
            PlayerPrefs.SetInt("Champ3", 75);
        }
        if (i == 2)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 500.ToString();
            PlayerPrefs.SetInt("Champ1", 500);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 250.ToString();
            PlayerPrefs.SetInt("Champ2", 250);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 125.ToString();
            PlayerPrefs.SetInt("Champ3", 125);
        }
        if (i == 3)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 1000.ToString();
            PlayerPrefs.SetInt("Champ1", 1000);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 500.ToString();
            PlayerPrefs.SetInt("Champ2", 500);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 250.ToString();
            PlayerPrefs.SetInt("Champ3", 250);
        }
        if (i == 4)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 2000.ToString();
            PlayerPrefs.SetInt("Champ1", 2000);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 1000.ToString();
            PlayerPrefs.SetInt("Champ2", 1000);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 500.ToString();
            PlayerPrefs.SetInt("Champ3", 500);
        }
        if (i == 5)
        {
            prize.transform.Find("1").Find("Text").GetComponentInChildren<Text>().text = 250.ToString();
            PlayerPrefs.SetInt("Champ1", 100);

            prize.transform.Find("2").Find("Text").GetComponentInChildren<Text>().text = 125.ToString();
            PlayerPrefs.SetInt("Champ2", 500);

            prize.transform.Find("3").Find("Text").GetComponentInChildren<Text>().text = 75.ToString();
            PlayerPrefs.SetInt("Champ3", 250);
        }


    }


    public GameObject prize;

}
