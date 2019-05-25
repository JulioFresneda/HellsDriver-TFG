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

        foreach(Profile p in profiles)
        {
            if (PlayerPrefs.GetString("profileSelected") == p.GetNick()) profileSelected = p;
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
