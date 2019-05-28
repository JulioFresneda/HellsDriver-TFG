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
    public double secondsMult = 10f;

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
        secondsOfAdvGO.transform.Find("Mult").GetComponent<Text>().text = "";

        initialPointsGO.transform.Find("Points").GetComponent<Text>().text = "";
        difficultMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        numlapMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        carmodelMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        positionMultGO.transform.Find("Points").GetComponent<Text>().text = "";
        secondsOfAdvGO.transform.Find("Points").GetComponent<Text>().text = "";

        totalCoinsGO.transform.Find("Points").GetComponent<Text>().text = "";
        totalPointsGO.transform.Find("Points").GetComponent<Text>().text = "";

        StartCoroutine(StartAnimation());

    }


   private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
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
        yield return IncrementAnimation(initialPointsGO, 0,1, "Mult");
        yield return IncrementAnimation(initialPointsGO, 0,initialPoints, "Points");

        yield return new WaitForSecondsRealtime(0.2f);

        
        yield return IncrementAnimation(secondsOfAdvGO, 0,secondsMult, "Mult");
        yield return IncrementAnimation(secondsOfAdvGO, finalPoints,finalPoints+SecondsOfAdventage(), "Points");
        finalPoints += SecondsOfAdventage();

        yield return new WaitForSecondsRealtime(0.2f);

        yield return IncrementAnimation(difficultMultGO, 0,DifficultMultiplier(), "Mult");
        yield return IncrementAnimation(difficultMultGO,finalPoints,finalPoints*DifficultMultiplier(), "Points");
        finalPoints *= DifficultMultiplier();

        yield return new WaitForSecondsRealtime(0.2f);


        yield return IncrementAnimation(numlapMultGO, 0, NumLapsMultiplier(), "Mult");
        yield return IncrementAnimation(numlapMultGO, finalPoints, finalPoints * NumLapsMultiplier(), "Points");
        finalPoints *= NumLapsMultiplier();

        yield return new WaitForSecondsRealtime(0.2f);


        yield return IncrementAnimation(carmodelMultGO, 0, CarModelMultiplier(), "Mult");
        yield return IncrementAnimation(carmodelMultGO, finalPoints, finalPoints * CarModelMultiplier(), "Points");
        finalPoints *= CarModelMultiplier();

        yield return new WaitForSecondsRealtime(0.2f);


        yield return IncrementAnimation(positionMultGO, 0, PositionMultiplier(), "Mult");
        yield return IncrementAnimation(positionMultGO, finalPoints, finalPoints * PositionMultiplier(), "Points");
        finalPoints *= PositionMultiplier();

        yield return new WaitForSecondsRealtime(0.2f);


        yield return IncrementAnimation(totalPointsGO, 0, finalPoints, "Points");
        yield return IncrementAnimation(totalCoinsGO, 0, coins, "Points");
        


    }

    private IEnumerator IncrementAnimation(GameObject go, double initial, double number, string find)
    {
        if (initial < number)
        {
            while (initial < number)
            {
                go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)initial * 100) / 100).ToString().Replace(',', '.');
                if (find == "Mult")
                {
                    go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',', '.');
                    initial += multSpeed;
                    yield return new WaitForSecondsRealtime(0.002f);
                }
                else
                {
                    initial += pointsSpeed;
                    yield return new WaitForSecondsRealtime(0.002f);
                }

            }
            go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)number * 100) / 100).ToString().Replace(',', '.');
            if (find == "Mult")
            {
                go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',', '.');


            }
        }
        else
        {
            while (initial > number)
            {
                go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)initial * 100) / 100).ToString().Replace(',', '.');
                if (find == "Mult")
                {
                    go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',', '.');
                    initial -= multSpeed;
                    yield return new WaitForSecondsRealtime(0.002f);
                }
                else
                {
                    initial -= pointsSpeed;
                    yield return new WaitForSecondsRealtime(0.002f);
                }

            }
            go.transform.Find(find).GetComponent<Text>().text = (Mathf.Round((float)number * 100) / 100).ToString().Replace(',', '.');
            if (find == "Mult")
            {
                go.transform.Find(find).GetComponent<Text>().text = "x" + go.transform.Find(find).GetComponent<Text>().text.Replace(',', '.');


            }
        }
        

    
    }


    private double SecondsOfAdventage()
    {
        double points = 0;

        foreach(RaceDriver ai in race.GetRaceDriversAI())
        {
            if (ai.GetSeconds() > race.GetRaceDriverPlayer().GetSeconds()) points += ai.GetSeconds() - race.GetRaceDriverPlayer().GetSeconds();
        }

        return points*secondsMult;
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
