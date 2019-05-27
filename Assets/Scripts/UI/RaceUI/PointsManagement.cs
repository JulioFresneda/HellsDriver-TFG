using Racing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsManagement : MonoBehaviour
{
    public GameObject initialPointsGO, difficultMultGO, numlapMultGO, carmodelMultGO, positionMultGO, totalPointsGO, totalCoinsGO, secondsOfAdvGO;
    public Race race;

    private double initialPoints = 100;
    private double finalPoints;
    private List<double> positionMultipliers;

    private int coins = 0;


    public double multSpeed = 0.01;
    public double pointsSpeed = 1;


    private List<bool> bools;

    private void Start()
    {
        positionMultipliers = new List<double>();

        positionMultipliers.Add(3);
        positionMultipliers.Add(2);
        positionMultipliers.Add(1);
        positionMultipliers.Add(0.9);
        positionMultipliers.Add(0.75);
        positionMultipliers.Add(0.5);
    }

    public void CalculatePoints()
    {
        finalPoints = initialPoints;
        finalPoints += SecondsOfAdventage();
        finalPoints *= DifficultMultiplier();
        finalPoints *= NumLapsMultiplier();
        finalPoints *= CarModelMultiplier();
        finalPoints *= PositionMultiplier();
        coins = Coins();

        Profiles.AddPoints((int)finalPoints);
        Profiles.AddCoins(coins);

        SetAnimation();

    }


    private void SetAnimation()
    {
        initialPointsGO.transform.Find("Mult").GetComponent<Text>().text = "";
        difficultMultGO.transform.Find("Mult").GetComponent<Text>().text = "";
        numlapMultGO.transform.Find("Mult").GetComponent<Text>().text = "";
        carmodelMultGO.transform.Find("Mult").GetComponent<Text>().text = "";
        positionMultGO.transform.Find("Mult").GetComponent<Text>().text = "";

        initialPointsGO.transform.Find("Points").GetComponent<Text>().text = "";
        difficultMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        numlapMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        carmodelMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        positionMultGO.transform.Find("Points").GetComponent<Text>().text = "";


        StartCoroutine(StartAnimation());

    }

    private IEnumerator StartAnimation()
    {
        double finalPoints = initialPoints;

        bools = new List<bool>();
        for(int i=0; i<7; i++)
        {
            bools.Add(false);
        }


        bools[0] = true;
        StartCoroutine(IncrementAnimation(initialPointsGO, 0,1, "Mult",0));
        StartCoroutine(IncrementAnimation(initialPointsGO, 0,initialPoints, "Points", 0));

        yield return new WaitForSecondsRealtime(0.5f);

        
        StartCoroutine(IncrementAnimation(secondsOfAdvGO, 0,1, "Mult", 1));
        StartCoroutine(IncrementAnimation(secondsOfAdvGO, finalPoints,finalPoints+SecondsOfAdventage(), "Points",  1));
        finalPoints += SecondsOfAdventage();

        yield return new WaitForSecondsRealtime(0.5f);

        StartCoroutine(IncrementAnimation(difficultMultGO, 0,DifficultMultiplier(), "Mult", 2));
        StartCoroutine(IncrementAnimation(difficultMultGO,finalPoints,finalPoints*DifficultMultiplier(), "Points", 2));
        finalPoints *= DifficultMultiplier();

        yield return new WaitForSecondsRealtime(0.5f);


        StartCoroutine(IncrementAnimation(numlapMultGO, 0, NumLapsMultiplier(), "Mult", 3));
        StartCoroutine(IncrementAnimation(numlapMultGO, finalPoints, finalPoints * NumLapsMultiplier(), "Points", 3));
        finalPoints *= NumLapsMultiplier();

        yield return new WaitForSecondsRealtime(0.5f);

    
        StartCoroutine(IncrementAnimation(carmodelMultGO, 0, CarModelMultiplier(), "Mult", 4));
        StartCoroutine(IncrementAnimation(carmodelMultGO, finalPoints, finalPoints * CarModelMultiplier(), "Points",4));
        finalPoints *= CarModelMultiplier();

        yield return new WaitForSecondsRealtime(0.5f);

  
        StartCoroutine(IncrementAnimation(positionMultGO, 0, PositionMultiplier(), "Mult", 5));
        StartCoroutine(IncrementAnimation(positionMultGO, finalPoints, finalPoints * PositionMultiplier(), "Points", 5));
        finalPoints *= PositionMultiplier();

        yield return new WaitForSecondsRealtime(0.5f);


        StartCoroutine(IncrementAnimation(totalPointsGO, 0, finalPoints, "Points", 6));
        StartCoroutine(IncrementAnimation(totalCoinsGO, 0, coins, "Points", 6));
        


    }

    private IEnumerator IncrementAnimation(GameObject go, double initial, double number, string find, int boolpos)
    {
        while (!bools[boolpos]) { }
        while (initial < number)
        {
            go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)initial*100)/100).ToString().Replace(',','.');
            if (find == "Mult")
            {
                go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',','.');
                initial += multSpeed;

            }
            else initial += pointsSpeed;

            yield return new WaitForSecondsRealtime(0.1f);
        }
        go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)number * 100) / 100).ToString().Replace(',', '.');
        if (find == "Mult")
        {
            go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',', '.');
 

        }

        bools[boolpos+1] = true;
    
    }


    private double SecondsOfAdventage()
    {
        double points = 0;

        foreach(RaceDriver ai in race.GetRaceDriversAI())
        {
            if (ai.GetSeconds() > race.GetRaceDriverPlayer().GetSeconds()) points += ai.GetSeconds() - race.GetRaceDriverPlayer().GetSeconds();
        }

        return points;
    }

    private double DifficultMultiplier()
    {
        return PlayerPrefs.GetFloat("difficultMult",1f);
    }

    private double NumLapsMultiplier()
    {
        return PlayerPrefs.GetFloat("lapNumberMult",1f);
    }

    private double CarModelMultiplier()
    {
        return PlayerPrefs.GetFloat("modelMult", 1f);
    }

    private double PositionMultiplier()
    {
        return positionMultipliers[race.GetRaceDriverPlayer().GetFinalPosition() - 1];
    }

    private int Coins()
    {
        return Mathf.RoundToInt((float)finalPoints / 10);
    }


  
}
