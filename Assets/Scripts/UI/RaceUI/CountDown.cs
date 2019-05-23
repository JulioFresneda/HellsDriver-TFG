using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{

   
    public float timeUntilCountdown = 1f;

    public Texture g3, g2, g1, go;

    public RawImage countdown;

    void Start()
    {

    }


    public void StartCountDown()
    {
        MakeKinematic(true);
        StartCoroutine( StartCountDownAnimation(timeUntilCountdown));


    }



    private void MakeKinematic(bool kinematic)
    {
        List<GameObject> cars = new List<GameObject>();
        foreach (GameObject aicar in GameObject.FindGameObjectsWithTag("AIDriver")) cars.Add(aicar);
        cars.Add(GameObject.FindGameObjectWithTag("PlayerDriver"));

        foreach (GameObject car in cars)
        {
            car.GetComponent<Rigidbody>().isKinematic = kinematic;
        }
    }


  


    private IEnumerator StartCountDownAnimation(float time)
    {
        
        yield return new WaitForSecondsRealtime(time);

        countdown.texture = g2;

        yield return new WaitForSecondsRealtime(1);

        countdown.texture = g1;

        yield return new WaitForSecondsRealtime(1);

        countdown.texture = go;

        yield return new WaitForSecondsRealtime(0.5f);



        MakeKinematic(false);

        gameObject.SetActive(false);

    }



}
