using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(PositionScript))]
[RequireComponent(typeof(ResourceController))]
public class Store : House {

    PlayerManager player;
    List<Serf> serfList;

    void Start()
    {
        player = GameObject.Find("GameScript").GetComponent<PlayerManager>();
        serfList = new List<Serf>();
        selected = false;
    }

    public void CreateSerf()
    {
        Serf serf = player.AddSerf(GetComponent<PositionScript>().GetTileBeforeEnter() + MoveAgent.TILE_CENTER, GetComponent<ResourceController>());
        serfList.Add(serf);
        Debug.Log("CreateSerf");
        if (someChanges != null) someChanges();
    }

    
    public override void OnBuild()
    {
        GetComponent<ResourceController>().lookForAgents();
        Debug.Log("OnBuild: store");
    }
    public override Dictionary<Resource, int> GetResources()
    {
        
        return GetComponent<ResourceController>().resourceList;
    }
    public string GetWorkerName()
    {
        return "Serf";
    }
    public  string GetWorkerNumber()
    {
        return serfList.Count.ToString();
    }
    public override void TimeLap(float time) { }
}
