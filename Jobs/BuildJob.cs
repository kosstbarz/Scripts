using UnityEngine;
using System.Collections;

public abstract class BuildJob {

    public float fullTime;
    public float timeLeft;
    public Vector2 tile; // Tile where worker go to finish job.
    public string type;
    public abstract void OnComplete();
    
    public void OnCancell()
    {
        BuildJobController.Instance.cbJobEnded(this);
    }

    public void DoWork(float workTime)
    {
        //Debug.Log("Work start with time " + timeLeft);
        timeLeft -= workTime;
        //Debug.Log("Work finish with time " + timeLeft);
        if (timeLeft <= 0f)
        {
            OnComplete();
        }
    }
}
