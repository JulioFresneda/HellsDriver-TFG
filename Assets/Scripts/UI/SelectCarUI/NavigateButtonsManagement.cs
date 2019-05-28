using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateButtonsManagement : MonoBehaviour
{

    public SelectModelManagement smm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GoBack()
    {
        if (smm.IsChampionship()) SceneManager.LoadScene("ChampionshipStatus");
        else SceneManager.LoadScene("SelectMap");
    }

    public void StartClickButton(string scene)
    {
        if(!smm.IsChampionship() || smm.IsModelSelectedUnlocked()) SceneManager.LoadScene(scene);
    }
}
