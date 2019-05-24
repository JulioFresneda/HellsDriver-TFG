using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medal : MonoBehaviour
{

    public GameObject goldenMedal, silverMedal, bronzeMedal;
    public GameObject camera;
    public GameObject directionalLight;

    private GameObject medal;

    private bool startAnimation = false;

    private bool loadhorns = false;
    private int position;

    private bool finishAnimation = false;


    // Start is called before the first frame update
    void Start()
    {
        
        goldenMedal.transform.localScale = new Vector3(0, 0, 0);
        silverMedal.transform.localScale = new Vector3(0, 0, 0);
        bronzeMedal.transform.localScale = new Vector3(0, 0, 0);
    }


    public void LoadMedalAnimation(int position)
    {
        this.position = position;
        if (position == 1) medal = goldenMedal;
        else if (position == 2) medal = silverMedal;
        else if (position == 3) medal = bronzeMedal;

        foreach (var fire in medal.GetComponentsInChildren<ParticleSystem>())
        {
            fire.transform.localScale = new Vector3(0, 0, 0);
        }

        medal.transform.SetParent(camera.transform);
        
        medal.transform.localPosition = new Vector3(-0.3f, 5f, 19f);

        medal.transform.rotation = new Quaternion(0, 0, 0, 0);
        directionalLight.transform.rotation = medal.transform.rotation;

        startAnimation = true;
        
    }

    public void Update()
    {
        if (startAnimation)
        {
            if (medal.transform.localScale.x < 0.1f)
            {
                medal.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
                foreach (var fire in medal.GetComponentsInChildren<ParticleSystem>())
                {
                    fire.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                }
            }

            

            if(medal.transform.localScale.x < 0.1f || Mathf.Abs(medal.transform.localEulerAngles.y) > 2)
            {
                
                Debug.Log(medal.transform.localEulerAngles.y);
                medal.transform.Rotate(new Vector3(0, 1, 0));
                
            }
            else
            {
                medal.transform.localEulerAngles = new Vector3(0, 0, 0);
                if (position != 3) FinishAnimation();
            }

            directionalLight.transform.rotation = medal.transform.rotation;



        }
        else if (finishAnimation)
        {
            directionalLight.transform.rotation = medal.transform.rotation;
            if (medal.transform.localPosition.z > 0) medal.transform.localPosition -= new Vector3(0, 0, 0.5f);
            else GameObject.Find("RaceFinished").transform.localScale = new Vector3(1, 1, 1);
        }
    }



    private void FinishAnimation()
    {
        StartCoroutine(Wait(1));
    }


    private IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(1);
        
        startAnimation = false;
        finishAnimation = true;

    }

    
}
