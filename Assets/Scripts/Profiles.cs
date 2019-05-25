using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profiles
{
    private static List<Profile> profiles;
    private static Profile profileSelected;


  


    public static void LoadProfiles()
    {
        profiles = new List<Profile>();
        for(int i=0; i<6; i++)
        {
            if(PlayerPrefs.GetString("profile"+i.ToString(),"Nuevo perfil") != "Nuevo perfil")
            {
                profiles.Add(new Profile(PlayerPrefs.GetString("profile" + i.ToString(), "Nuevo perfil"), false));
            }
        }
    }


    public static void SetProfileSelected(string name)
    {
        foreach(Profile p in profiles)
        {
            if (p.GetNick() == name) profileSelected = p;
        }
    }

    public static int GetNumberOfProfiles()
    {
        return profiles.Count;
    }

    public static string GetName()
    {
        return profileSelected.GetNick();
    }

    public static int GetPercentage()
    {
        return profileSelected.GetPercentageUnlocked();
    }

    public static int GetCoins()
    {
        return profileSelected.GetCoins();
    }

    public static int GetPoints()
    {
        return profileSelected.GetPoints();
    }


    public static void AddNewProfile(string name, int position)
    {
        if(PlayerPrefs.GetString("profile" + position.ToString(), "Nuevo perfil") == "Nuevo perfil" && position < 6)
        {
            PlayerPrefs.SetString("profile" + position.ToString(), name);
            profiles.Add(new Profile(name, true));
        }
    }

}
