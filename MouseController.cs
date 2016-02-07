using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class MouseController : MonoBehaviour {

    public static MouseController Instance { get; protected set; }
    public GhostScript ghost;
    public TileMarkerScript marker;

    Vector2 currTileUnderMouse;
    Vector2 lastTileUnderMouse;

    bool buildHouseMode = false;
    bool buildTileMode = false;
    bool deleteJob = false;

    private int groundLM;
    private int selectableLM;
    private RaycastHit hit;

    Action<Vector2> TileChanged;
    

    // Use this for initialization
    void Start () {
        
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of MouseControl can be created");
        }
        currTileUnderMouse = new Vector2(-1, -1);
        lastTileUnderMouse = currTileUnderMouse;
        groundLM = LayerMask.GetMask("Ground");
        selectableLM = LayerMask.GetMask("Selectable");
    }
	
	// Update is called once per frame
	void Update () {
        // All click management for ghost
        if (buildHouseMode)
        {
            if (Input.GetMouseButtonUp(2))
                ghost.Rotate();
            if (Input.GetMouseButtonUp(0) && ghost.CanBuild())
            {
                ghost.PlaceHouse();
                // On default we use road after building a house
                BuildMode.Instance.OnTileButtonClick(0);
            }
            if (Input.GetMouseButtonUp(1))
            {
                // Go out from build mode
                BuildMode.Instance.OnBuildButtonClick();
            }
        }
        // All clicks for tiles
        if (buildTileMode)
        {
            // Here we decide what we want to do, add jobs or remove them.
            if (Input.GetMouseButtonDown(0) && currTileUnderMouse.x >= 0)
            {
                TileBuildJob job = BuildJobController.Instance.GetJob(currTileUnderMouse);
                deleteJob = (job != null && job.type == "Road");
                
            }

            if (Input.GetMouseButton(0) && currTileUnderMouse.x >= 0)
            {
                if (!deleteJob)
                { // Add job
                    if (marker.CanBuild())
                    {
                        BuildMode.Instance.CreateJob(currTileUnderMouse, "Road");
                    }
                }
                else // Remove job
                {
                    TileBuildJob job = BuildJobController.Instance.GetJob(currTileUnderMouse);
                    if (job != null && job.type == "Road")
                    {
                        BuildJobController.Instance.GetJob(currTileUnderMouse).OnCancell();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(1))
                {
                    BuildMode.Instance.OnBuildButtonClick();

                }
            }
        }
        // All modes are off. Selecting objects and orders.
        if (!buildHouseMode && !buildTileMode)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, selectableLM) &&
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    //Debug.Log("RayCast work");
                    Selecting.Instance.ChangeSelection(hit.transform.GetComponent<ISelectable>());
                    
                }
            }
        }
	}

    void FixedUpdate() {
        if (TileChanged != null) {
            
            lastTileUnderMouse = currTileUnderMouse;
            currTileUnderMouse = TileUnderMouse();
            //Debug.Log("currTileUnderMouse.x" + currTileUnderMouse.x);
            if (currTileUnderMouse != lastTileUnderMouse )
            {
                TileChanged(currTileUnderMouse);  
            }
        }
    }
    // Determine coordinates of tile, pointed by mouse.
    public Vector2 TileUnderMouse()
    {
        Vector2 tileUnderMouse = new Vector2(-1, -1);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLM) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            tileUnderMouse.x = (int)(hit.point.x / MapController.TILE_SIZE);
            tileUnderMouse.y = (int)(hit.point.z / MapController.TILE_SIZE);
        }
        //Debug.Log("x" + tileUnderMouse.x + " y=" + tileUnderMouse.y);
        return tileUnderMouse;
    }

    public void RegisterTileChanged(Action<Vector2> callBack) {
        TileChanged += callBack;
    }

    public void UnregisterTileChanged(Action<Vector2> callBack) {
        TileChanged -= callBack;
    }

    public void TurnOnHouseMode() {
        buildHouseMode = true;
        buildTileMode = false;
    }
    public void TurnOnTileMode() {
        buildHouseMode = false;
        buildTileMode = true;
    }
    public void TurnOffModes() {
        buildHouseMode = false;
        buildTileMode = false;
    }
}
