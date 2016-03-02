using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MapGenerator))]
public class BuildMode : MonoBehaviour {

    public static BuildMode Instance { get; protected set; }
    public GameObject buildPanel;
    public GameObject[] objects;
    public Material ghostGreen;
    public Material ghostRed;

    GameObject ghost;
    TileMarkerScript tileMarkerScript;

    bool buildPanelActive;

    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of BuildScript can be created");
        }
        buildPanelActive = buildPanel.activeSelf;
        tileMarkerScript = GameObject.Find("TileMarker").GetComponent<TileMarkerScript>();
    }

    public void OnBuildButtonClick()
    {
        buildPanel.SetActive(!buildPanelActive);
        buildPanelActive = buildPanel.activeSelf;
        if (buildPanelActive == false)
        {
            Destroy(ghost);
            tileMarkerScript.enabled = false;
        }
    }

    public void OnTileButtonClick(int number)
    {
        Destroy(ghost);
        tileMarkerScript.enabled = true;
        tileMarkerScript.currentObject = number;
    }

    public void OnHouseButtonClick(int number)
    {
        tileMarkerScript.enabled = false;
        Destroy(ghost);
        if (number < objects.Length)
        {
            ghost = Instantiate(objects[number]);
            ghost.AddComponent<GhostScript>();
            ghost.GetComponent<GhostScript>().number = number;
            
        } else
        {
            Debug.LogError("House with number " + number + " not found.");
        }
    }

     
    public void BuildRoad(Vector2 coord)
    {
        MapController.Instance.ChangeTile(coord, "Road");
        
    }
}
