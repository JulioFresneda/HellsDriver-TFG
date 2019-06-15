using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentButtonsAdmin : MonoBehaviour
{




    [SerializeField]
    private List<Button> buttons = new List<Button>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NewSelection(string button)
    {
      
        foreach(Button b in buttons)
        {
            if (b.gameObject.name != button && b.GetComponent<ButtonScript>().IsSelected())
            {
                b.GetComponent<ButtonScript>().Desselect();
            }
            
            
        }
    }


}
