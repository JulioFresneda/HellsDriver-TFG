using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class Profile 
{
    private string nick;
    private int coins, points, percentage;


    private List<string> modelsUnlocked;
    private List<string> mapsUnlocked;

    public Profile(string nick, bool newprofile)
    {
        if (newprofile)
        {
            modelsUnlocked = new List<string>();
            mapsUnlocked = new List<string>();

            this.nick = nick;
            UnlockBasic();

            
            
            coins = 0;
            points = 0;
            percentage = CalculatePercentageUnlocked();


            PlayerPrefs.SetInt(nick + "_coins", coins);
            PlayerPrefs.SetInt(nick + "_points", points);
        }
        else
        {
            modelsUnlocked = new List<string>();
            mapsUnlocked = new List<string>();
            this.nick = nick;
            LoadUnlockItems();

            percentage = CalculatePercentageUnlocked();

            
            coins = PlayerPrefs.GetInt(nick + "_coins");
            points = PlayerPrefs.GetInt(nick + "_points");


        }
        
    }


    private void LoadUnlockItems()
    {
        List<CarModel> carModels = LoadModels.GetAllCarModels();
        foreach (CarModel c in carModels)
        {
            if(PlayerPrefs.GetString(nick+"_"+c.GetModel(),"Locked") == "Unlocked")
            {
                modelsUnlocked.Add(c.GetModel());
            }
        }

        if (PlayerPrefs.GetString(nick + "_Eight", "Locked") == "Unlocked") mapsUnlocked.Add("Eight");
        if (PlayerPrefs.GetString(nick + "_Dizzy", "Locked") == "Unlocked") mapsUnlocked.Add("Dizzy");
        if (PlayerPrefs.GetString(nick + "_Whirl", "Locked") == "Unlocked") mapsUnlocked.Add("Whirl");
        if (PlayerPrefs.GetString(nick + "_Subway", "Locked") == "Unlocked") mapsUnlocked.Add("Subway");
    }

    public void AddPoints(int points)
    {
        this.points += points;
        PlayerPrefs.SetInt(nick + "_points", this.points);
    }

    public int GetPoints() => points;

    public void AddCoins(int coins)
    {
        this.coins += coins;
        PlayerPrefs.SetInt(nick + "_coins", this.coins);
    }

    public int GetCoins() => coins;

    public bool SpendCoins(int coins)
    {
        if (this.coins < coins) return false;
        else
        {
            this.coins -= coins;
            Debug.Log("SPEND" + coins + nick);
            PlayerPrefs.SetInt(nick + "_coins", this.coins);
            return true;
        }
    }




    public bool IsUnlocked(string name)
    {
        return (modelsUnlocked.Contains(name) || mapsUnlocked.Contains(name));
    }

    public void UnlockModel(string name)
    {
        PlayerPrefs.SetString(nick + "_" + name, "Unlocked");
        modelsUnlocked.Add(name);
        percentage = CalculatePercentageUnlocked();
    }

    public void UnlockMap(string name)
    {
        PlayerPrefs.SetString(nick + "_" + name, "Unlocked");
        mapsUnlocked.Add(name);
        percentage = CalculatePercentageUnlocked();
    }



    private void UnlockBasic()
    {
        List<CarModel> carModels = LoadModels.GetAllCarModels();
        foreach(CarModel c in carModels)
        {
            if((c.GetBrand() == "Duck" && c.GetThrottle() == 6 && c.GetStiffness() == 3) || (c.GetThrottle() < 8 && c.GetBrand() == "Audidas") || c.GetBrand() == "Hoa")
            {
                modelsUnlocked.Add(c.GetModel());
                PlayerPrefs.SetString(nick + "_" + c.GetModel(),"Unlocked");
            }
        }

        mapsUnlocked.Add("Eight");
        mapsUnlocked.Add("Dizzy");

        PlayerPrefs.SetString(nick + "_" + "Eight", "Unlocked");
        PlayerPrefs.SetString(nick + "_" + "Dizzy", "Unlocked");

    }

    private int CalculatePercentageUnlocked()
    {
        PlayerPrefs.SetInt(nick + "_percentage", (int)(100*(mapsUnlocked.Count + modelsUnlocked.Count)/58));
        return (int)(100 * (mapsUnlocked.Count + modelsUnlocked.Count) / 58);
    }


    public int GetPercentageUnlocked() => percentage;




    public List<string> GetModelsUnlocked() => modelsUnlocked;
    public List<string> GetMapsUnlocked() => mapsUnlocked;




    public string GetNick() => nick;



    
}
