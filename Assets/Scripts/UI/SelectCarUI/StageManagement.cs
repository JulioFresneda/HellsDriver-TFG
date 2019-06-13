using Racing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleSystem;

public class StageManagement : MonoBehaviour
{
    public GameObject stage;
    public float rotationVelocity = 0.01f;

    public List<GameObject> carModelPrefabs;

    public Transform carTransform;
    public Transform parentTransform;

    private GameObject carStage;


    // Update is called once per frame
    void Update()
    {
        stage.transform.Rotate(new Vector3(0, 0, rotationVelocity));
    }


    public void ChangeStageCar(string model)
    {
        foreach(GameObject car in carModelPrefabs)
        {
            if (car.name == model) ChangeStageCar(car);
        }
    }

    private void ChangeStageCar(GameObject model)
    {
        if (carStage != null) Destroy(carStage);
        carStage = Instantiate(model,parentTransform);

        foreach (var ray in carStage.GetComponentsInChildren<CarRaycast>()) Destroy(ray);
        foreach (var susp in carStage.GetComponentsInChildren<CarSuspension>()) Destroy(susp);
        foreach (var wheel in carStage.GetComponentsInChildren<WheelCollider>()) Destroy(wheel);
        Destroy(carStage.GetComponent<Rigidbody>());
        Destroy(carStage.GetComponent<CarAI>());
        Destroy(carStage.GetComponent<CarController>());
        Destroy(carStage.GetComponent<RaceDriver>());




        
        carStage.GetComponent<Transform>().localScale = carTransform.localScale;
        carStage.GetComponent<Transform>().localPosition = new Vector3(0, 43, 0);
        carStage.GetComponent<Transform>().localEulerAngles = new Vector3(0, 216, 0);


    }
}
