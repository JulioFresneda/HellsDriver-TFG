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

    public GameObject carMultiplier;

    private string GameMode;


    public Texture modelButton, lockedModelButton, modelButtonH, lockedModelButtonH, modelButtonS, lockedModelButtonS;

    private bool isChampionship;


    public BuyModel buyModel;


    public StageManagement stageManagement;


    private void Start()
    {
        GameMode = PlayerPrefs.GetString("GameMode");
        if (GameMode == "Championship") isChampionship = true;
        Profiles.LoadProfiles();

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

        UpdateCarMultiplier();

        CheckChampionship();
    }

    private void CheckChampionship()
    {
        if (isChampionship)
        {
            LoadLockedButtonTextures();
        }
    }



    public bool IsChampionship() => isChampionship;

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
        ModelSelected(selectedModels[0].GetModel());

        if (isChampionship)
        {
            if (!Profiles.IsUnlocked(selectedModels[0].GetModel())) buyModel.BuyWindow(selectedModels[0]);
            else buyModel.CloseWindow();
            LoadLockedButtonTextures();
        }

        Debug.Log(PlayerPrefs.GetString("modelSelected"));
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

        UpdateCarMultiplier();
    }

    public List<CarModel> GetSelectedModels() => selectedModels; 



    public void ModelHighlighted(string modelName, bool highlighted)
    {

        if (highlighted)
        {
            for (int i = 0; i < selectedModels.Count; i++)
            {
                if (selectedModels[i].GetModel() == modelName)
                {
                    modelFeatures.UpdateFeatures(selectedModels[i]);
                    stageManagement.ChangeStageCar(selectedModels[i].GetModel());
                }
            }

        }

        else
        {
            modelFeatures.UpdateFeatures(selectedModels[modelSelected]);
            stageManagement.ChangeStageCar(selectedModels[modelSelected].GetModel());
        }
        
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


        UpdateCarMultiplier();

        if (GameMode == "FastRace" || (GameMode == "Championship" && Profiles.IsUnlocked(modelName)))
        {
            PlayerPrefs.SetString("modelSelected", modelName);
            if (GameMode == "Championship") buyModel.CloseWindow();
            
        }
        else
        {
            buyModel.BuyWindow(selectedModels[modelSelected]);
        }

        stageManagement.ChangeStageCar(selectedModels[modelSelected].GetModel());

     

        
    }


    public void LoadLockedButtonTextures()
    {
        for(int i=0; i<modelButtons.Count; i++)
        {
            if (!Profiles.IsUnlocked(selectedModels[i].GetModel()))
            {
                if(i == 0) modelButtons[0].GetComponent<RawImage>().texture = lockedModelButtonS;
                else modelButtons[i].GetComponent<RawImage>().texture = lockedModelButton;

                modelButtons[i].GetComponent<ButtonScript>().TextureNotHighlighted = lockedModelButton;
                modelButtons[i].GetComponent<ButtonScript>().TextureHighligted= lockedModelButtonH;
                modelButtons[i].GetComponent<ButtonScript>().TextureSelected = lockedModelButtonS;
            }
            else
            {
                if (i == 0) modelButtons[0].GetComponent<RawImage>().texture = modelButtonS;
                else modelButtons[i].GetComponent<RawImage>().texture = modelButton;

                modelButtons[i].GetComponent<ButtonScript>().TextureNotHighlighted = modelButton;
                modelButtons[i].GetComponent<ButtonScript>().TextureHighligted = modelButtonH;
                modelButtons[i].GetComponent<ButtonScript>().TextureSelected = modelButtonS;
            }
        }

        
    }


    private void UpdateCarMultiplier()
    {

        CarModel cm = selectedModels[modelSelected];
        float throttleMult = 2.25f - (cm.GetThrottle() - 6) * 0.25f;

        float stiffnessMult = 1 + 0.5f*(4 - cm.GetStiffness());
        float massMult = 0.75f;
        if (cm.GetMass() == 2000) massMult = 1f;
        if (cm.GetMass() == 2500) massMult = 1.5f;


        float multiplier = throttleMult * stiffnessMult * massMult;
        multiplier = (Mathf.Round(100 * multiplier) / 100);


        carMultiplier.GetComponentInChildren<Text>().text = "x"+multiplier.ToString().Replace(',','.');
        PlayerPrefs.SetFloat("modelMult", multiplier);
    }


    public string GetModelSelected()
    {
        return selectedModels[modelSelected].GetModel();
    }

    public CarModel GetCarModelSelected() => selectedModels[modelSelected];

    public bool IsModelSelectedUnlocked()
    {
        return Profiles.IsUnlocked(selectedModels[modelSelected].GetModel());
    }



    
}




