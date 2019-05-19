using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandSelected : MonoBehaviour
{

    private int brandSelected = 0;

    public List<GameObject> flames;


    public void ChangeSelection(int selected)
    {
        brandSelected = selected;
        ChangeFlameColors();
    }



    private void Start()
    {
        ChangeFlameColors();
    }



    private void ChangeFlameColors()
    {
        Color color = Color.black ;
      

        if (brandSelected == 0) color = Color.cyan;
        if (brandSelected == 1) color = new Color(64, 64, 64); // Grey
        if (brandSelected == 2) color = Color.green;
        if (brandSelected == 3) color = new Color(255, 255, 0); // Yellow
        if (brandSelected == 4) color = new Color(80, 0, 0); // Dark red
        if (brandSelected == 5) color = Color.magenta;
        if (brandSelected == 6) color = Color.white;
        if (brandSelected == 7) color = Color.blue;
        if (brandSelected == 8) color = Color.red;


        foreach(GameObject f in flames)
        {
            ParticleSystem ps = f.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule ma = ps.main;
            ma.startColor = color;
        }
       
    }
}
