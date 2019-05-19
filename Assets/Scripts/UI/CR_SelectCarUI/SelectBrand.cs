using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBrand : MonoBehaviour
{

    public GameObject Left, Center, Right;


    private List<string> brands;

    private int position;

    [SerializeField]
    private Transform transformSelected;

    [SerializeField]
    private Transform transformNotSelected;

    // Start is called before the first frame update
    void Start()
    {
        brands = new List<string>();
        brands.Add("Duck");
        brands.Add("Audidas");
        brands.Add("Hoa");
        brands.Add("Valkyria");
        brands.Add("XLynx");
        brands.Add("DJED");
        brands.Add("Raijin");
        brands.Add("Poseidon");
        brands.Add("Leviathan");

        position = 0;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
