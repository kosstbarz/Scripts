using UnityEngine;
using System.Collections.Generic;

public class Builder : Unit{

    BuildJob myJob;

    public Builder(Vector2 coord, PlayerManager player, ResourceController jobController) : base(coord, player, jobController)
    {

    }
    
    // This method makes all time dependent stuff.
    public override void TimeLap(float time)
    {
        if (myJob == null)
        {
            myJob = BuildJobController.Instance.takeBuildJob();
            
            if (myJob != null)
            {
                moveAgent.destination = myJob.tile;
                moveAgent.path = PathfindingManager.Instance.GetPath(moveAgent.Pos - MoveAgent.TILE_CENTER, moveAgent.destination);
                
                Debug.Log("Destination changed");
            }
        }

        if (moveAgent.destination.Equals(moveAgent.Pos - MoveAgent.TILE_CENTER) && myJob != null) // Unit reached final point and ready to doWork.
        {
            if (myJob.DoWork(time))
            {
                JobEnded();
            }
        }
        moveAgent.TimeLap(time);
    }

    void JobEnded()
    {
        myJob = null;
        //Debug.Log("Job ended!");
        
    }
}
