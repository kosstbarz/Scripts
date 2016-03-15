using UnityEngine;
using System.Collections.Generic;
using System;

public class Sawmill : ProcessHouse {

    List<Unit> carpenterList; // FIXME
    private float timer;
    const float TIME_TO_MAKE_BOARD = 6f;
    private bool logInWork;
    ResourceAgent logAgent;
    ResourceAgent boardAgent;

    public override void OnBuild()
    {
        logInWork = false;
        carpenterList = new List<Unit>();
        ResourceAgent[] agentList = GetComponents<ResourceAgent>();
        if (agentList.Length != 2)
        {
            Debug.LogError("Number of resource agents doesnt equals two.");
        }
        foreach (ResourceAgent r in agentList)
        {
            if (r.myRes == Resource.Log)
            {
                logAgent = r;
                logAgent.currCount = 0;
                Debug.Log("OnEnable Sawmill: log " + logAgent.currCount.ToString());
            }
            if (r.myRes == Resource.Board)
            {
                boardAgent = r;
                boardAgent.currCount = 0;
                Debug.Log("OnEnable Sawmill: board " + boardAgent.currCount.ToString());
            }
        }

        logAgent.OnBuild();
        boardAgent.OnBuild();
    }

    public override Dictionary<Resource, int> GetResources()
    {
        Dictionary<Resource, int> dic = new Dictionary<Resource, int>();
        dic[logAgent.myRes] = logAgent.currCount;
        dic[boardAgent.myRes] = boardAgent.currCount;
        return dic;
    }
    public override void TimeLap(float time) {

        if (!logInWork && logAgent.currCount > 0)
        {
            Debug.Log("TimeLap Sawmill: log " + logAgent.currCount.ToString());
            logAgent.TakeResource();
            logInWork = true;
        } 
         if (logInWork) {
            timer += time;
            if (timer > TIME_TO_MAKE_BOARD)
                {
                timer = 0f;
                boardAgent.BringResource();
                logInWork = false;
            }
        }
    }
    public override string GetWorkerName()
    {
        return "Carpenter";
    }
    public override string GetWorkerNumber()
    {
        return carpenterList.Count.ToString();
    }

}
