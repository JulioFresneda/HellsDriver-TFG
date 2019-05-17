using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
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
        gameObject.GetComponent<RawImage>().texture = TextureHighligted;
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponent<RawImage>().texture = TextureSelected;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<RawImage>().texture = TextureNotHighlighted;
    }
}
