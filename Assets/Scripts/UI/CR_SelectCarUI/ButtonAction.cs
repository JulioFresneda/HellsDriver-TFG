using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{


    private ButtonScript buttonScript;
    public SelectModelManagement selectModelUI;


    private void Start()
    {
        buttonScript = this.GetComponentInParent<ButtonScript>();
    }






    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (!buttonScript.IsSelected())
        {
            
            selectModelUI.ModelHighlighted(gameObject.GetComponentInChildren<Text>().text, true);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!buttonScript.IsSelected())
        {
            selectModelUI.ModelSelected(gameObject.GetComponentInChildren<Text>().text);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!buttonScript.IsSelected())
        {
            selectModelUI.ModelHighlighted(gameObject.GetComponentInChildren<Text>().text, false);
        }
    }
}
