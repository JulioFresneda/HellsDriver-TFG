using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ExitClick()
    {
        Application.Quit();

    }

    public void MenuClick(string menu)
    {
        SceneManager.LoadScene(menu);
    }

    public void ContinueButton()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
}
