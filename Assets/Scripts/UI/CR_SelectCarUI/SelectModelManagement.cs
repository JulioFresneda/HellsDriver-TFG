using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class SelectModelManagement : MonoBehaviour
{
    public List<GameObject> modelButtons;

    public List<CarModel> carModels;
    private List<CarModel> selectedModels;

    private int brandSelected;
    private int modelSelected;

    private List<string> brands;


    public ModelFeatures modelFeatures;


    private void Start()
    {
        brands = new List<string>();
        brands.Add("Duck");
        brands.Add("Audidas");
        brands.Add("Hoa");
        brands.Add("Valkyria");
        brands.Add("Xlynx");
        brands.Add("DJED");
        brands.Add("Raijin");
        brands.Add("Poseidon");
        brands.Add("Leviathan");


        brandSelected = 0;
        modelSelected = 0;

        carModels = LoadModels.GetAllCarModels();
        UpdateModelsList();
        modelFeatures.UpdateFeatures(selectedModels[0]);
        ModelSelected(selectedModels[0].GetModel());
        PlayerPrefs.SetString("modelSelected", selectedModels[0].GetModel());
    }


    public void ChangeBrandSelection(int selected)
    {
        brandSelected = selected;
        UpdateModelsList();
        modelFeatures.UpdateFeatures(selectedModels[0]);
        foreach(GameObject b in modelButtons)
        {
            b.GetComponent<ButtonScript>().Desselect();
        }
        modelButtons[0].GetComponent<ButtonScript>().Select();
    }


    private void UpdateModelsList()
    {
        Debug.Log(brandSelected);
        selectedModels = new List<CarModel>();
        foreach(CarModel model in carModels)
        {
            if (model.GetBrand() == brands[brandSelected]) selectedModels.Add(model);
        }

        for(int i=0; i<selectedModels.Count; i++)
        {
            modelButtons[i].GetComponentInChildren<Text>().text = selectedModels[i].GetModel();
        }
    }

    public List<CarModel> GetSelectedModels() => selectedModels; 



    public void ModelHighlighted(string modelName, bool highlighted)
    {
        
        if (highlighted)
        {
            for (int i = 0; i < selectedModels.Count; i++)
            {
                if (selectedModels[i].GetModel() == modelName) modelFeatures.UpdateFeatures(selectedModels[i]);
            }
            
        }

        else modelFeatures.UpdateFeatures(selectedModels[modelSelected]);
        
    }


    public void ModelSelected(string modelName)
    {
        for (int i = 0; i < selectedModels.Count; i++)
        {
            if (selectedModels[i].GetModel() == modelName)
            {
                modelFeatures.UpdateFeatures(selectedModels[i]);
                modelSelected = i;
            }
        }

        Debug.Log(modelName);
        PlayerPrefs.SetString("modelSelected", modelName);
    }

    
}




