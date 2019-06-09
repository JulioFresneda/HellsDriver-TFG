using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Awards : MonoBehaviour
{


    private int eightPosition, dizzyPosition, whirlPosition, subwayPosition;
    private int finalPosition, finalAward;

    public Text eightText, dizzyText, whirlText, subwayText;
    public Text finalCategoryText, coinsText;

    public Medal medal;

    // Start is called before the first frame update
    void Start()
    {
        CalculateAwards();    
    }

    private void CalculateAwards()
    {
        Profiles.LoadProfiles();

        eightPosition = PlayerPrefs.GetInt("ChampEightPosition");
        dizzyPosition = PlayerPrefs.GetInt("ChampDizzyPosition");
        whirlPosition = PlayerPrefs.GetInt("ChampWhirlPosition");
        subwayPosition = PlayerPrefs.GetInt("ChampSubwayPosition");

        eightText.text = eightPosition.ToString();
        dizzyText.text = dizzyPosition.ToString();
        whirlText.text = whirlPosition.ToString();
        subwayText.text = subwayPosition.ToString();

        int posweight = (eightPosition + dizzyPosition + whirlPosition + subwayPosition);
        if (posweight <= 4) finalPosition = 1;
        else if (posweight <= 8) finalPosition = 2;
        else if (posweight <= 12) finalPosition = 3;
        else finalPosition = 4;

        finalAward = PlayerPrefs.GetInt("Champ" + finalPosition.ToString());
        if (finalPosition == 1) finalCategoryText.text = "Oro";
        if (finalPosition == 2) finalCategoryText.text = "Plata";
        if (finalPosition == 3) finalCategoryText.text = "Bronce";
        if (finalPosition > 3) finalCategoryText.text = "Cobre";

        if (finalPosition == 1)
        {
            coinsText.text = PlayerPrefs.GetInt("Champ1").ToString() + " monedas";
            Profiles.AddCoins(PlayerPrefs.GetInt("Champ1"));
        }
        if (finalPosition == 2)
        {
            coinsText.text = PlayerPrefs.GetInt("Champ2").ToString() + " monedas";
            Profiles.AddCoins(PlayerPrefs.GetInt("Champ2"));
        }
        if (finalPosition == 3)
        {
            coinsText.text = PlayerPrefs.GetInt("Champ3").ToString() + " monedas";
            Profiles.AddCoins(PlayerPrefs.GetInt("Champ3"));
        }
        
        if (finalPosition > 3) coinsText.text = "Sin premio";
       
     
        StartCoroutine(PositionAnimation());

    }


    private IEnumerator PositionAnimation()
    {
        float maxalpha = eightText.color.a;

        Color color = eightText.color;
        Color catcolor = eightText.color;
        if (finalPosition == 1) ColorUtility.TryParseHtmlString("FEFF00", out catcolor);
        if(finalPosition == 2) ColorUtility.TryParseHtmlString("CBCBCB", out catcolor);
        if (finalPosition == 3) ColorUtility.TryParseHtmlString("#614100", out catcolor);
        if (finalPosition > 3) ColorUtility.TryParseHtmlString("B78E57", out catcolor);
        Color coinscolor = coinsText.color;

        color.a = 0f;
        catcolor.a = 0f;
        coinscolor.a = 0f;

        eightText.color = color;
        dizzyText.color = color;
        whirlText.color = color;
        subwayText.color = color;
        finalCategoryText.color = catcolor;
        coinsText.color = coinscolor;

        yield return new WaitForSecondsRealtime(0.5f);

        float vel = 0.01f;

        while (eightText.color.a < maxalpha)
        {
            color = eightText.color;
            color.a += vel;
            eightText.color = color;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        while (dizzyText.color.a < maxalpha)
        {
            color = dizzyText.color;
            color.a += vel;
            dizzyText.color = color;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        while (whirlText.color.a < maxalpha)
        {
            color = whirlText.color;
            color.a += vel;
            whirlText.color = color;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        while (subwayText.color.a < maxalpha)
        {
            color = subwayText.color;
            color.a += vel;
            subwayText.color = color;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        if(finalPosition <= 3) medal.LoadMedalAnimation(finalPosition);

        while (finalCategoryText.color.a < maxalpha)
        {
            catcolor = finalCategoryText.color;
            catcolor.a += vel;
            finalCategoryText.color = catcolor;
            yield return new WaitForSecondsRealtime(0.001f);

        }
        while (coinsText.color.a < maxalpha)
        {
            coinscolor = coinsText.color;
            coinscolor.a += vel;
            coinsText.color = coinscolor;
            yield return new WaitForSecondsRealtime(0.001f);

        }
    }
}
