using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleSystem;

public class BuyModel : MonoBehaviour
{
    public Text buyText;
    public SelectModelManagement smm;


    private bool canBuy = false;

    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        
        originalColor = buyText.color;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }


    public void BuyWindow(CarModel cm)
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        if (cm.GetPrize() <= Profiles.GetCoins())
        {
            buyText.text = "Puede comprar el modelo " + cm.GetModel() + " por " + cm.GetPrize() + " monedas. Tiene " + Profiles.GetCoins() + " monedas.";
            
            canBuy = true;
            buyText.color = originalColor;
            this.transform.Find("Buy").gameObject.GetComponentInChildren<Text>().color = originalColor;

        }
        else
        {
            buyText.text = "No puede comprar el modelo " + cm.GetModel() + " por " + cm.GetPrize() + " monedas. No tiene suficientes monedas.";
            canBuy = false;
            buyText.color = Color.red;
            this.transform.Find("Buy").gameObject.GetComponentInChildren<Text>().color = Color.red;
        }
    }

    public void CloseWindow()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }


    public void ConfirmBuy()
    {
        CarModel cm = smm.GetCarModelSelected();
        if (canBuy && Profiles.SpendCoins(cm.GetPrize()))
        {
            Profiles.UnlockModel(cm.GetModel());

            gameObject.transform.localScale = new Vector3(0, 0, 0);
            canBuy = false;
            smm.LoadLockedButtonTextures();
        }
    }
   

    
}
