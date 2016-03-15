using UnityEngine;
using System.Collections.Generic;

// Every player in multiplayer should have one manager.
// It contains information about all units, buildings and so on.
public class PlayerManager : MonoBehaviour {

    public List<House> houseList;
    public List<ResourceController> storeList;
    Dictionary<Unit, GameObject> unitList;
    //Dictionary<Serf, GameObject> serfList;
    public GameObject builder_prefab;
    public GameObject serf_prefab;
    public GameObject characters;
    
    // Use this for initialization
	void Start () {
        unitList = new Dictionary<Unit, GameObject>();
        //serfList = new Dictionary<Serf, GameObject>();
        houseList = new List<House>();
        storeList = new List<ResourceController>();
    }

    public void AddBuilder(Vector2 position)
    {
        //Debug.Log(position.x + " " + position.y);
        Builder unit = new Builder(position, this, null); // FIXME
        GameObject unit_go = Instantiate(builder_prefab);
        unit_go.transform.SetParent(characters.transform);
        unit_go.transform.position = new Vector3(position.x, MapController.Instance.mapData.GetHeight(position)+1f, position.y);
        unitList.Add(unit, unit_go);
    }
    public Serf AddSerf(Vector2 position, ResourceController resController)
    {
        //Debug.Log(position.x + " " + position.y);
        Serf serf = new Serf(position, this, resController);
        GameObject unit_go = Instantiate(serf_prefab);
        unit_go.transform.SetParent(characters.transform);
        unit_go.transform.position = new Vector3(position.x, MapController.Instance.mapData.GetHeight(position) + 1f, position.y);
        unitList.Add(serf, unit_go);
        return serf;
    }

    public void MoveUnit(Unit unit, Vector2 coord)
    {
        unitList[unit].transform.position = new Vector3(coord.x, MapController.Instance.mapData.GetExactHeight(coord), coord.y);
    }

    public void AddHouse(House house)
    {
        houseList.Add(house);
        Debug.Log("Player get new house registered. Is it null? " + (house == null).ToString());
    }
    public void AddStore(ResourceController house)
    {
        Debug.Log("Player get new store registered");
        storeList.Add(house);
    }

    void FixedUpdate()
    {
        foreach(Unit u in unitList.Keys)
        {
            u.TimeLap(Time.fixedDeltaTime);
        }
        foreach(House h in houseList)
        {
            h.TimeLap(Time.fixedDeltaTime);
        }
        
    }
}
