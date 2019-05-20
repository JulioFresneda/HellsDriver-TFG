using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class SelectModelUI : MonoBehaviour
{
    public List<GameObject> modelButtons;

    public List<CarModel> carModels;
    private List<CarModel> selectedModels;

    private int brandSelected;

    private List<string> brands;


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
        gameObject.GetComponentInChildren<LoadModels>().LoadCarModels();
        carModels = gameObject.GetComponentInChildren<LoadModels>().GetCarModels();
        ChangeModels();
    }


    public void ChangeSelection(int selected)
    {
        brandSelected = selected;
        ChangeModels();
    }


    private void ChangeModels()
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



}
