using UnityEngine;
using System;
using System.Collections.Generic;

public class TileBuildJob : BuildJob {


    static Dictionary<string, float> jobsTime;

    static TileBuildJob (){
        jobsTime = new Dictionary<string, float>();
        jobsTime.Add("Road", 2f);
    }

    public TileBuildJob(Vector2 tile, string type)
    {
        this.tile = tile;
        fullTime = jobsTime[type];
        this.type = type;
        timeLeft = fullTime;
        Debug.Log("Full time " + fullTime + ", timeleft " + timeLeft);
    }

    public override void OnComplete()
    {
        MapController.Instance.ChangeTile(tile, type);
        BuildJobController.Instance.cbBuildJobEnded(this);

    }
}
