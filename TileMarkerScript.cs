using UnityEngine;
using System.Collections;

public class TileMarkerScript : MonoBehaviour {

    public int currentObject;
    public float MAX_SLOPE;

    Vector2 mouseTile;
    Vector3 pos;
    Projector projector;

    void Awake()
    {
        projector = GetComponentInChildren<Projector>();
        mouseTile = new Vector2(-1, -1);
        
    }

    // Use this for initialization
    void Start () {
        MouseController.Instance.marker = this;
    }
	void OnEnable()
    {
        projector.enabled = true;
        MouseController.Instance.RegisterTileChanged(ChangePosition);
        MouseController.Instance.TurnOnTileMode();
        
    }
	void ChangePosition (Vector2 coord) {
        mouseTile = coord;
        if (coord.x >= 0)
        {
            
            pos.Set(mouseTile.x + MapController.TILE_SIZE / 2, 0, mouseTile.y + MapController.TILE_SIZE / 2);

            gameObject.transform.position = pos;
            projector.material.color = CanBuild() ? Color.green : Color.red;
            projector.enabled = true;
        } else
        {
            projector.enabled = false;
        }
	}

    void OnDisable()
    {
        pos.Set(-1, 0, -1);
        projector.enabled = false;
        MouseController.Instance.UnregisterTileChanged(ChangePosition);
        MouseController.Instance.TurnOffModes();
    }

    public bool CanBuild()
    {
        if (mouseTile.x < 0) return false;
        if (!MapController.Instance.mapData.tileData[(int)mouseTile.x, (int)mouseTile.y].walkable) return false;
        if (MapController.Instance.mapData.tileData[(int)mouseTile.x, (int)mouseTile.y].name == Types.Road) return false;
        if (MapController.Instance.mapData.GetSlope(mouseTile, 1, 1) > MAX_SLOPE) return false;

        return true;
    }
}
