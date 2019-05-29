using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenData : MonoBehaviour
{

    [SerializeField]
    private GameObject CarMinimapIconPrefab = null;

    [SerializeField]
    private Material AICarMinimapIconMaterial = null;

    [SerializeField]
    private Material PlayerCarMinimapIconMaterial = null;


    private List<GameObject> AICars;
    private List<GameObject> AICarsIcons;

    private GameObject PlayerCar;
    private GameObject PlayerCarIcon;

    // Start is called before the first frame update
    void Initialize()
    {
        

        AICars = new List<GameObject>();
        AICars.AddRange(GameObject.FindGameObjectsWithTag("AIDriver"));

        PlayerCar = GameObject.FindGameObjectWithTag("PlayerDriver");


        AICarsIcons = new List<GameObject>();

        for(int i=0; i<AICars.Count; i++)
        {
            AICarsIcons.Add(Instantiate(CarMinimapIconPrefab));
            AICarsIcons[AICarsIcons.Count - 1].GetComponent<Renderer>().material = AICarMinimapIconMaterial;
            AICarsIcons[AICarsIcons.Count - 1].GetComponent<Transform>().localScale = new Vector3(50, 50, 50);
            AICarsIcons[AICarsIcons.Count - 1].layer = LayerMask.NameToLayer("Minimap");
            AICarsIcons[AICarsIcons.Count - 1].transform.SetParent(AICars[AICarsIcons.Count - 1].transform);
            AICarsIcons[AICarsIcons.Count - 1].transform.localPosition = new Vector3(0, 250, -20);
            AICarsIcons[AICarsIcons.Count - 1].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            AICarsIcons[AICarsIcons.Count - 1].GetComponent<MeshRenderer>().receiveShadows = false;
        }
        PlayerCarIcon = Instantiate(CarMinimapIconPrefab);
        PlayerCarIcon.GetComponent<Renderer>().material = PlayerCarMinimapIconMaterial;
        PlayerCarIcon.GetComponent<Renderer>().GetComponent<Transform>().localScale = new Vector3(50, 50, 50);
        PlayerCarIcon.layer = LayerMask.NameToLayer("Minimap");

        PlayerCarIcon.transform.SetParent(PlayerCar.transform);
        PlayerCarIcon.transform.localPosition = new Vector3(0, 280, -20);

        PlayerCarIcon.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        PlayerCarIcon.GetComponent<MeshRenderer>().receiveShadows = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCar == null) Initialize();
        else
        {
            
            PlayerCarIcon.transform.rotation = PlayerCar.transform.rotation;

            for (int i = 0; i < AICars.Count; i++)
            {

                AICarsIcons[i].transform.rotation = AICars[i].transform.rotation;
 
            }
        }

        
    }
}
