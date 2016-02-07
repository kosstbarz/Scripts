using UnityEngine;
using System;
using System.Collections.Generic;

public class TileBuildJob {

    float fullTime;
    float timeLeft;
    public string type;
    public Vector2 tile;
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

    public void OnComplete()
    {
        MapController.Instance.ChangeTile(tile, type);
        BuildJobController.Instance.cbJobEnded(this);
        
    }
    public void OnCancell()
    {
        BuildJobController.Instance.cbJobEnded(this);
    }
   
    public void DoWork(float workTime)
    {
        Debug.Log("Work start with time " + timeLeft);
        timeLeft -= workTime;
        Debug.Log("Work finish with time " + timeLeft);
        if (timeLeft <= 0f)
        {
            OnComplete();
        }
    }
}
