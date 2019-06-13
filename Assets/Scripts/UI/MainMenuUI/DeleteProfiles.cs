using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteProfiles : MonoBehaviour
{


    public List<GameObject> profiles;
    public ProfileDataUpdater profileDataUpdater;

    public void UpdateDeleteButtons()
    {
        foreach(GameObject p in profiles)
        {
            if (p.GetComponentInChildren<Text>().text != "Nuevo perfil" && p.GetComponentInChildren<Text>().text != Profiles.GetName()) p.GetComponent<Transform>().Find("Delete").transform.localScale = Vector3.one;
            else p.GetComponent<Transform>().Find("Delete").transform.localScale = Vector3.zero;
        }
    }


    public void OnClickDeleteButton(int position)
    {
        Profiles.RemoveProfile(profiles[position].GetComponentInChildren<Text>().text,position);
        UpdateDeleteButtons();
        profileDataUpdater.Start();
    }
}
