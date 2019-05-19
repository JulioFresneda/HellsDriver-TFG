using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBrand : MonoBehaviour
{

    public GameObject Left, Center, Right, Hidden, HiddenLeft, Aux;


    private List<string> brands;

    private int position;

    private Vector3 positionCenter;
    private Vector3 positionLeft;
    private Vector3 positionRight;
    private Vector3 positionHiddenRight;
    private Vector3 positionHiddenLeft;
    


    public Texture Duck, Audidas, Hoa, Valkyria, XLynx, DJED, Raijin, Poseidon, Leviathan;

    private List<Texture> brandTextures;


    private bool goright = false;
    private bool goleft = false;



    private float scaleConstant;

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

        brandTextures = new List<Texture>();
        brandTextures.Add(Duck);
        brandTextures.Add(Audidas);
        brandTextures.Add(Hoa);
        brandTextures.Add(Valkyria);
        brandTextures.Add(XLynx);
        brandTextures.Add(DJED);
        brandTextures.Add(Raijin);
        brandTextures.Add(Poseidon);
        brandTextures.Add(Leviathan);

        positionCenter = new Vector3( Center.transform.position.x,Center.transform.position.y,Center.transform.position.z);
        positionLeft = new Vector3(Left.transform.position.x, Left.transform.position.y, Left.transform.position.z);
        positionRight = new Vector3(Right.transform.position.x, Right.transform.position.y, Right.transform.position.z);
        positionHiddenRight = new Vector3(Hidden.transform.position.x, Hidden.transform.position.y, Hidden.transform.position.z);
        positionHiddenLeft = new Vector3(HiddenLeft.transform.position.x, HiddenLeft.transform.position.y, HiddenLeft.transform.position.z);




        position = 0;
        Left.SetActive(false);
        Center.GetComponent<RawImage>().texture = brandTextures[0];
        Right.GetComponent<RawImage>().texture = brandTextures[1];


        Center.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

        scaleConstant = (float)5 / (Center.transform.position.x - Left.transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            GoRight();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            GoLeft();
        }





        if (goright)
        {
            if (Center.transform.position.x > positionLeft.x || Center.transform.localScale.x > 1 )
            {
                if(Center.transform.position.x > positionLeft.x)
                {
                    Left.transform.position -= new Vector3(10, 0, 0);
                    Center.transform.position -= new Vector3(10, 0, 0);
                    Right.transform.position -= new Vector3(10, 0, 0);
                    Hidden.transform.position -= new Vector3(10, 0, 0);
                }

                if(Center.transform.localScale.x > 1)
                {
                    Center.transform.localScale -= new Vector3(scaleConstant, scaleConstant, scaleConstant);
                    Right.transform.localScale += new Vector3(scaleConstant, scaleConstant, scaleConstant);
                }
                    

            }
            else
            {
                Left.transform.position = positionHiddenRight;
                if (position == 0) Left.SetActive(true);

                Aux = Left;
                Left = Hidden;
                Hidden = Aux;

                Aux = Left;
                Left = Right;
                Right = Aux;

                Aux = Left;
                Left = Center;
                Center = Aux;

                goright = false;
                position++;
                UpdatePosition();
             
            }
        }
        else if (goleft)
        {
            if (Center.transform.position.x < positionRight.x || Center.transform.localScale.x > 1)
            {

                if (Center.transform.position.x < positionRight.x)
                {
                    Left.transform.position += new Vector3(10, 0, 0);
                    Center.transform.position += new Vector3(10, 0, 0);
                    Right.transform.position += new Vector3(10, 0, 0);
                    Hidden.transform.position += new Vector3(10, 0, 0);
                }
                if(Center.transform.localScale.x > 1)
                {
                    Center.transform.localScale -= new Vector3(scaleConstant, scaleConstant, scaleConstant);
                    Left.transform.localScale += new Vector3(scaleConstant, scaleConstant, scaleConstant);
                }
                
            }
            else
            {
                if (position == brandTextures.Count - 1) Right.SetActive(true);

                Aux = Left;
                Left = Hidden;
                Hidden = Aux;

                Aux = Hidden;
                Hidden = Center;
                Center = Aux;

                Aux = Hidden;
                Hidden = Right;
                Right = Aux;

                

                goleft = false;
                position--;
                UpdatePosition();
                
            }
        }
    }


    private void GoRight()
    {
        
        if(!goright && !goleft)
        {
            


            if (position < brandTextures.Count - 1)
            {

               


                goright = true;
                goleft = false;

                if (position < brandTextures.Count - 2) Hidden.GetComponent<RawImage>().texture = brandTextures[position + 2];

                if (position == brandTextures.Count - 2) Hidden.SetActive(false);
                
                

            }

            
        }
        
        
    }

    private void GoLeft()
    {


        
        if(!goright && !goleft)
        {
            if (position > 0)
            {
                Hidden.transform.position = positionHiddenLeft;
                if (position > 1) Hidden.GetComponent<RawImage>().texture = brandTextures[position - 2];
                else Hidden.SetActive(false);

                goleft = true;
                goright = false;



                

            }
        }


       

        

    }




    private void UpdatePosition()
    {

        gameObject.GetComponentInParent<BrandSelected>().ChangeSelection(position);
    }




}
