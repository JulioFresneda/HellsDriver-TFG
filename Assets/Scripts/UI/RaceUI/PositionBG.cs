using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionBG : MonoBehaviour
{
    public GameObject positionBG;
    public Texture bgfirst, bgnotfirst;

    bool changetofirst = true;

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<Text>().text == "1" && changetofirst )
        {
            changetofirst = false;
            positionBG.GetComponent<RawImage>().texture = bgfirst;
        }
        if(this.GetComponent<Text>().text != "1" && !changetofirst)
        {
            changetofirst = true;
            positionBG.GetComponent<RawImage>().texture = bgnotfirst;
        }
    }
}
