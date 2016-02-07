using UnityEngine;
using System.Collections.Generic;

// Every player in multiplayer should have one manager.
// It contains information about all units, buildings and so on.
public class PlayerManager : MonoBehaviour {

    Dictionary<Unit, GameObject> unitList;
    public GameObject unit_prefab;
    public GameObject characters;
    
    // Use this for initialization
	void Start () {
        unitList = new Dictionary<Unit, GameObject>();
        
	}
	
    public void AddUnit(Vector2 position)
    {
        Debug.Log(position.x + " " + position.y);
        Unit unit = new Unit(position, this);
        GameObject unit_go = Instantiate(unit_prefab);
        unit_go.transform.SetParent(characters.transform);
        unit_go.transform.position = new Vector3(position.x, MapController.Instance.mapData.GetHeight(position)+1f, position.y);
        unitList.Add(unit, unit_go);
    }

	public void MoveUnit(Unit unit, Vector2 coord)
    {
        unitList[unit].transform.position = new Vector3(coord.x, MapController.Instance.mapData.GetExactHeight(coord), coord.y);
    }

    void FixedUpdate()
    {
        foreach(Unit u in unitList.Keys)
        {
            u.TimeLap(Time.fixedDeltaTime);
        }
    }
}
