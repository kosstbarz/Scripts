using UnityEngine;
using System.Collections;

public class DeliveryJob {

	public Resource resource;
    public PositionScript startPosition;
    public PositionScript endPosition;
    bool cargoIsTaken = false;

    public DeliveryJob(Resource res, PositionScript start, PositionScript end)
    {
        this.resource = res;
        this.startPosition = start;
        this.endPosition = end;
    }
    public static ResourceAgent lookForAgent(PositionScript pos, Resource resource)
    {
        ResourceAgent[] agentList = pos.GetComponents<ResourceAgent>();
        if (agentList.Length == 0)
        {
            return null;
        }
        foreach (ResourceAgent ra in agentList)
        {
            if (ra.myRes == resource)
            {
                return ra;
            }
        }
        Debug.LogError("No suitable ResourceAgent found");
        return null;
    }
    public Vector2 GetDistination()
    {
        if (cargoIsTaken)
        {
            return endPosition.GetTileBeforeEnter();
        }
        //Debug.Log("Start point for serf is " + startPosition.GetTileBeforeEnter());
        return startPosition.GetTileBeforeEnter();
    }
    public void CargoIsTaken()
    {
        cargoIsTaken = true;
    }

    public bool DoWork(Serf serf)
    {
        if (!cargoIsTaken)
        {
            serf.TakeCargo(resource);
            ResourceAgent ra = lookForAgent(startPosition, resource);
            if (ra == null)
            {
                ResourceController rc = startPosition.GetComponent<ResourceController>();
                rc.TakeResource(resource);
            } else
            {
                ra.TakeResource();
            }
            //Debug.Log("DoWork: Resource is taken");
            cargoIsTaken = true;
            return false;
        } else {
            OnComplete(serf);
            //Debug.Log("DoWork: Resource job is finished");
            return true;
        }
    }

    void OnComplete(Serf serf)
    {
        serf.ReturnCargo();
        ResourceAgent ra = lookForAgent(endPosition, resource);   
        if (ra == null)
        {
            ResourceController rc = endPosition.GetComponent<ResourceController>();
            rc.AddResource(resource);
        }
        else
        {
            ra.BringResource();
            ra.JobIsEnded(this);// This should be executed in case of job is ended unsuccessfuly.
        }
        ra = lookForAgent(startPosition, resource);
        if (ra != null)
        {
            ra.JobIsEnded(this);// This should be executed in case of job is ended unsuccessfuly.
        }
       
    } 
    
}
