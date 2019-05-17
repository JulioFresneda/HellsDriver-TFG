using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{



    





    public void CarreraRapida()
    {
        SceneManager.LoadScene("CarreraRapidaScene");
    }

    public void Campeonato()
    {
        SceneManager.LoadScene("CampeonatoScene");
    }

    public void ModoHistoria()
    {
        SceneManager.LoadScene("ModoHistoriaScene");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("OpcionesScene");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
