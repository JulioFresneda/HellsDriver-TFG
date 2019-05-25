using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoMenuButton : MonoBehaviour
{

    public GameObject Button;
    public GameObject FinishPanel;

    public void OnClickButton(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
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
