using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateButtonsManagement : MonoBehaviour
{

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GoBackClickButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartClickButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
