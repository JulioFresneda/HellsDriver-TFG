using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    public string FastRaceScene, ChampionshipScene, GarageScene, CreditsScene;
    



    

    public void FastRace()
    {
        PlayerPrefs.SetString("GameMode", "FastRace");
        SceneManager.LoadScene(FastRaceScene);
  
    }

    public void Championship()
    {
        PlayerPrefs.SetString("GameMode", "Championship");
        SceneManager.LoadScene(ChampionshipScene);
    }

    public void Garage()
    {
        SceneManager.LoadScene(GarageScene);
    }

    public void Credits()
    {
        SceneManager.LoadScene(CreditsScene);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
