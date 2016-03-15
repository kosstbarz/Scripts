using UnityEngine;
using System.Collections.Generic;
using System;

public class Lumberjack : ExtractiveHouse {

    // Use this for initialization
    List<Serf> woodcutters; // FIXME
    private float timer;
    const float TIME_TO_GET_RESOURCE = 10;
    void OnEnable()
    {
        woodcutters = new List<Serf>();
    }

    public override void OnBuild()
    {
        GetComponent<ResourceAgent>().OnBuild();
    }


    public override Dictionary<Resource, int> GetResources()
    {
        ResourceAgent resAg = GetComponent<ResourceAgent>();
        Dictionary<Resource, int> dic = new Dictionary<Resource, int>();
        dic[resAg.myRes] = resAg.currCount;
        return dic;
    }
    public override string GetWorkerName()
    {
        return "Woodcutter";
    }
    public override string GetWorkerNumber()
    {
        return woodcutters.Count.ToString();
    }
    public override void TimeLap(float time) {
        timer += time;
        if (timer > TIME_TO_GET_RESOURCE)
        {
            timer = 0f;
            GetComponent<ResourceAgent>().BringResource();
        }
    }
}
