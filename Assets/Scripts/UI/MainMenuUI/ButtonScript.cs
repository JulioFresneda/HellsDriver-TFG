using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{

    public bool PermanentSelect = false;
    private bool selected = false;


    
    public void Desselect()
    {
        if (selected)
        {
            selected = false;
            gameObject.GetComponent<RawImage>().texture = TextureNotHighlighted;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Texture TextureNotHighlighted;
    public Texture TextureSelected;
    public Texture TextureHighligted;
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!(selected && PermanentSelect)) gameObject.GetComponent<RawImage>().texture = TextureHighligted;
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponent<RawImage>().texture = TextureSelected;
        selected = true;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("MapButton"))
        {
            if (g.name != this.name) g.GetComponent<ButtonScript>().Desselect();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!(selected && PermanentSelect)) gameObject.GetComponent<RawImage>().texture = TextureNotHighlighted;
    }
}
