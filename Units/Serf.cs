using UnityEngine;
using System.Collections.Generic;

public class Serf : Unit {

    DeliveryJob myJob;

    Resource cargo;
    bool hasCargo;
    //ResourceController jobController; // Can't be null. Represent store (or wagon in future), which serf is attached to.

    public Serf(Vector2 coord, PlayerManager player, ResourceController jobController) : base(coord, player, jobController)
    {

    }
    // This method makes all time dependent stuff.
    public override void TimeLap(float time)
    {
        
        if (myJob == null)
        {
            if (LookForJob())
            {
                //Debug.Log("Serf " + this + " find a job!");
                moveAgent.destination = myJob.GetDistination();
                moveAgent.path = PathfindingManager.Instance.GetPath(moveAgent.Pos - MoveAgent.TILE_CENTER, moveAgent.destination);
            }
        }
        

        if (moveAgent.destination.Equals(moveAgent.Pos - MoveAgent.TILE_CENTER) && myJob != null)
        {
            if (myJob.DoWork(this))
            {
                JobEnded();
                
            } else
            {
                moveAgent.destination = myJob.GetDistination();
                moveAgent.path = PathfindingManager.Instance.GetPath(moveAgent.Pos - MoveAgent.TILE_CENTER, moveAgent.destination);
            }
            return;
        }
        moveAgent.TimeLap(time);
    }
    private bool LookForJob()
    {
        
        myJob = jobController.MakeDeliveryJob(moveAgent.Pos);
        return myJob != null;
    }


    void JobEnded()
    {
        myJob = null;
        //Debug.Log("Delivery job ended!");
    }

    public void TakeCargo(Resource res)
    {
        cargo = res;
        hasCargo = true;
    }
    public void ReturnCargo()
    {
        hasCargo = false;
    }
}
