using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PositionScript))]
public class GhostScript : MonoBehaviour {

    
    public float MAX_SLOPE = 2f;
    public int BORDER = 3;
    public int number; // Number of house in objects array

    Vector2 currTile;
    Renderer renderer;
    PositionScript positionScript;
    Vector2 leftCorner;

    // Use this for initialization
    void Start() {
        //GameObject gameScript = GameObject.Find("GameScript");
        name = "Ghost";
        renderer = GetComponentInChildren<Renderer>();
        renderer.material = BuildScript.Instance.ghostGreen;
        renderer.receiveShadows = false;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        renderer.gameObject.layer = LayerMask.NameToLayer("Ghost");
        positionScript = GetComponent<PositionScript>();
        leftCorner = positionScript.LeftCorner();
        MouseController.Instance.RegisterTileChanged(ChangePosition);
        MouseController.Instance.ghost = this;
        MouseController.Instance.TurnOnHouseMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangePosition(Vector2 coord)
    {
        currTile = coord;
        if(currTile.x >= 0)
        {
            positionScript.EnterTile = coord;
            Vector3 coordCenter = new Vector3(coord.x + MapController.TILE_SIZE/2, MapController.Instance.mapData.GetHeight(coord), coord.y + MapController.TILE_SIZE/2);
            renderer.material = CanBuild() ? BuildScript.Instance.ghostGreen : BuildScript.Instance.ghostRed;
            transform.position = coordCenter;
        }
         
    }

    public void Rotate()
    {
        Vector3 axis = new Vector3(0f, 1f);
        transform.Rotate(axis, -90f);
        positionScript.Rotate();
    }

    public bool CanBuild()
    {
        if (currTile.x < 0) return false;
        int x = (int)currTile.x;
        int y = (int)currTile.y;
        
        leftCorner = positionScript.LeftCorner();
        int xLength = positionScript.XLength();
        int yLength = positionScript.YLength();
        for (int i = (int)leftCorner.x; i < leftCorner.x + xLength; i++)
        {
            for (int j = (int)leftCorner.y; j < leftCorner.y + yLength; j++)
            {
                if (!MapController.Instance.mapData.tileData[i, j].walkable)
                {
                    //Debug.Log("Tile " + i + ", " + j + " is not walkable");
                    return false;
                }
                if (i < BORDER || j < BORDER || i > MapController.Instance.mapData.xSize - BORDER || j > MapController.Instance.mapData.ySize - BORDER)
                {
                    //Debug.Log("Too close to border of world");
                    return false;
                }
            }
        }
        return MapController.Instance.mapData.GetSlope(positionScript.LeftCorner(), positionScript.XLength(), positionScript.YLength()) < MAX_SLOPE;
    }

    public void PlaceHouse()
    {
        GameObject newHouse = Instantiate(BuildScript.Instance.objects[number]);
        newHouse.GetComponent<PositionScript>().SynchronizePosition(positionScript);
        newHouse.transform.position = gameObject.transform.position;
        newHouse.transform.rotation = gameObject.transform.rotation;
        int xLength = positionScript.XLength();
        int yLength = positionScript.YLength();

        for (int i = (int)leftCorner.x; i < leftCorner.x + xLength; i++)
        {
            for (int j = (int)leftCorner.y; j < leftCorner.y + yLength; j++)
            {
                //Debug.Log("coord " + i + ", " + j);
                MapController.Instance.ChangeTile(new Vector2(i, j), "Unknown");
            }
        }
    }
    void OnDestroy()
    {
        MouseController.Instance.UnregisterTileChanged(ChangePosition);
        MouseController.Instance.ghost = null;
        MouseController.Instance.TurnOnTileMode();
    }
}
