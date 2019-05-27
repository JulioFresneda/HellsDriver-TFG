using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profiles
{
    private static List<Profile> profiles;
    private static Profile profileSelected;


  


    public static void LoadProfiles()
    {
       //PlayerPrefs.DeleteAll();
        profiles = new List<Profile>();
        for(int i=0; i<6; i++)
        {
            if(PlayerPrefs.GetString("profile"+i.ToString(),"Nuevo perfil") != "Nuevo perfil")
            {
                profiles.Add(new Profile(PlayerPrefs.GetString("profile" + i.ToString(), "Nuevo perfil"), false));
            }
        }

        if(profiles.Count == 0) PlayerPrefs.SetString("profileSelected", "Null");
        else
        {
            SetProfileSelected(PlayerPrefs.GetString("profileSelected"));
        }
     
    }


    public static void SetProfileSelected(string name)
    {
        foreach(Profile p in profiles)
        {
            if (profileSelected == null || ( p.GetNick() == name && profileSelected.GetNick() != name))
            {
                profileSelected = p;
                PlayerPrefs.SetString("profileSelected", name);
            }
        }
    }

    public static int GetNumberOfProfiles()
    {
        return profiles.Count;
    }

    public static string GetName()
    {
        if (profileSelected != null) return profileSelected.GetNick();
        else return "";
    }

    public static int GetPercentage()
    {
        if (profileSelected != null) return profileSelected.GetPercentageUnlocked();
        else return 0;
    }

    public static int GetCoins()
    {
        if (profileSelected != null) return profileSelected.GetCoins();
        else return 0;
    }

    public static void AddCoins(int coins)
    {
        if (profileSelected != null) profileSelected.AddCoins(coins);
    }

    public static int GetPoints()
    {
        if (profileSelected != null) return profileSelected.GetPoints();
        else return 0;
    }

    public static void AddPoints(int points)
    {
        if (profileSelected != null) profileSelected.AddPoints(points);
    }


    public static void AddNewProfile(string name, int position)
    {
        if(PlayerPrefs.GetString("profile" + position.ToString(), "Nuevo perfil") == "Nuevo perfil" && position < 6)
        {
            PlayerPrefs.SetString("profile" + position.ToString(), name);
            profiles.Add(new Profile(name, true));
            SetProfileSelected(name);
        }
    }

}
