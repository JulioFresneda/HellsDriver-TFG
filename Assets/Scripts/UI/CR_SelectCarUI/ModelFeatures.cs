using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class ModelFeatures : MonoBehaviour
{
    public RawImage power, grip, mass;

    public List<Texture> featureIndicators;


    public void UpdateFeatures(CarModel model)
    {
        power.texture = featureIndicators[model.GetThrottle() - 6];

        if (model.GetStiffness() == 2) grip.texture = featureIndicators[2];
        if (model.GetStiffness() == 3) grip.texture = featureIndicators[5];
        if (model.GetStiffness() == 4) grip.texture = featureIndicators[8];

        if (model.GetMass() == 1500) mass.texture = featureIndicators[2];
        if (model.GetMass() == 2000) mass.texture = featureIndicators[5];
        if (model.GetMass() == 2500) mass.texture = featureIndicators[8];

      
        
    }
}
