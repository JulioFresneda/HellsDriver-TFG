using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectProfileClick : MonoBehaviour
{
    public void OnProfileClick(Text text)
    {
        if(text.text != "Nuevo perfil")
        {
            Profiles.SetProfileSelected(text.text);
            gameObject.GetComponentInParent<PermanentButtonsAdmin>().NewSelection(gameObject.name);
            GameObject.Find("SelectProfile").transform.localScale = new Vector3(0, 0, 0);
            GameObject.Find("Profile").GetComponent<ProfileDataUpdater>().UpdateData();
            GameObject.Find("InputNick").transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            GameObject.Find("InputNick").transform.localScale = new Vector3(1, 1, 1);
        }
    }


    
}
