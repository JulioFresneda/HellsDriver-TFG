using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{

    public bool PermanentSelect = false;

    [SerializeField]
    private bool selected = false;

    public Texture TextureNotHighlighted;
    public Texture TextureSelected;
    public Texture TextureHighligted;


    [SerializeField]
    private PermanentButtonsAdmin PermanentButtonsAdmin;

   

    void Start()
    {
        if (selected)
        {
            gameObject.GetComponent<RawImage>().texture = TextureSelected;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!(selected && PermanentSelect)) gameObject.GetComponent<RawImage>().texture = TextureHighligted;
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponent<RawImage>().texture = TextureSelected;
        if (PermanentButtonsAdmin != null) PermanentButtonsAdmin.NewSelection(gameObject.name);
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!(selected && PermanentSelect)) gameObject.GetComponent<RawImage>().texture = TextureNotHighlighted;
    }


    public bool IsSelected() => selected;

    public void Desselect()
    {
        if (selected)
        {
            selected = false;
            gameObject.GetComponent<RawImage>().texture = TextureNotHighlighted;
        }

    }

    public void Select()
    {
        if (!selected)
        {
            gameObject.GetComponent<RawImage>().texture = TextureSelected;
            if (PermanentButtonsAdmin != null) PermanentButtonsAdmin.GetComponent<PermanentButtonsAdmin>().NewSelection(gameObject.name);
            selected = true;
        }
    }

    
}
