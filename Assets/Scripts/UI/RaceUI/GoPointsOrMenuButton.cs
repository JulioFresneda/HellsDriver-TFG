using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoPointsOrMenuButton : MonoBehaviour
{

    public GameObject Button;
    public GameObject FinishPanel;
    public GameObject PointsPanel;

    private bool gomenu = false;

    public void OnClickButton(string scene)
    {
        if (gomenu)
        {
            if(PlayerPrefs.GetString("GameMode") == "Championship")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ChampionshipStatus");
            }
            else UnityEngine.SceneManagement.SceneManager.LoadScene(scene);

        }
        else
        {
            gomenu = true;
            if (PlayerPrefs.GetString("GameMode") == "Championship") gameObject.GetComponentInChildren<Text>().text = "Continuar";
            else gameObject.GetComponentInChildren<Text>().text = "Ir al menu";
            PointsPanel.transform.localScale = new Vector3(1, 1, 1);
            PointsPanel.GetComponent<PointsManagement>().CalculatePoints();
        }
    }

    void Start()
    {
        Button.transform.localScale = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (FinishPanel.transform.localScale.x > 0 && Button.transform.localScale.x == 0) Button.transform.localScale = new Vector3(2, 2, 2);
    }
}
