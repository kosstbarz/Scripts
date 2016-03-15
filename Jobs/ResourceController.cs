using UnityEngine;
using System.Collections.Generic;

public class ResourceController : MonoBehaviour {

    public const float RADIUS = 20f;
    List<Order> lackOrderList;
    List<Order> overOrderList;
    public Dictionary<Resource, int> resourceList; // Resorces in store/wagon
    Dictionary<Resource, int> reservedResources;
    public PositionScript position;

    // Use this for initialization
    void OnEnable() {
        lackOrderList = new List<Order>();
        overOrderList = new List<Order>();
        resourceList = new Dictionary<Resource, int>();
        reservedResources = new Dictionary<Resource, int>();
        position = GetComponent<PositionScript>();
        
    }

    public void AddResource(Resource res)
    {
        if (resourceList.ContainsKey(res))
        {
            resourceList[res] += 1;
        } else
        {
            resourceList[res] = 1;
        }
        if (GetComponent<House>().someChanges != null) GetComponent<House>().someChanges();
    }

    public void TakeResource(Resource res)
    {
        if (!reservedResources.ContainsKey(res) || reservedResources[res] == 0 || !resourceList.ContainsKey(res) || resourceList[res] == 0)
        {
            Debug.LogError("TakeResource: Resource wasnt reserved or doesnt exist in store.");
        }
        reservedResources[res]--;
        resourceList[res]--;
        if (GetComponent<House>().someChanges != null) GetComponent<House>().someChanges();
    }
    int notReservedResourceNumber(Resource res)
    {
        int result = 0;
        if (resourceList.ContainsKey(res)){
            result = resourceList[res];
        }
        if (reservedResources.ContainsKey(res)){
            result -= reservedResources[res];
        }
        if (result < 0) {
            Debug.LogError("Number of reserved resources more then existing.");
        }
        return result;
    }

    bool TryReserveResource(Resource res)
    {
        if (notReservedResourceNumber(res) > 0)
        {
            if (reservedResources.ContainsKey(res)){
                reservedResources[res] += 1;
            } else
            {
                reservedResources[res] = 1;
            }
            
            Debug.Log("Reserved " + reservedResources[res] + " of " + res.ToString());
            return true;
        }
        return false;
    }
    // Adds order to correct list and returns it in order to agent will be able to
    // remove order in case it will be taken by another store.
    public Order AddOrder(Order order, bool isLack) 
    {
        if (isLack)
        {
            lackOrderList.Add(order);
        } else
        {
            overOrderList.Add(order);
        }
        return order;
    }
    // Agent asks to remove its order because it was taken by another store.
    public void RemoveOrder(Order order, bool isLack)
    {
        if (isLack)
        {
            lackOrderList.Remove(order);
        }
        else
        {
            overOrderList.Remove(order);
        }
    }
    // Serf asks store to make job for him.
    public DeliveryJob MakeDeliveryJob (Vector2 serfPoint)
    {
        float bestDist = 100f;
        Order bestOrder = null;
        DeliveryJob job = null;
        foreach(Order order in overOrderList)
        {
            if ((serfPoint - order.GetPoint()).magnitude < bestDist)
            {
                bestOrder = order;
                bestDist = (serfPoint - order.GetPoint()).magnitude;
            }
        }
        if (bestOrder == null) // There are no overOrders. Look for lackOrder in order to sutisfy it from store.
        {
            bestDist = 100f;
            Debug.Log("****Try to make job from store!");
            Debug.Log(lackOrderList.Count);
            foreach (Order order in lackOrderList)
            {
                Debug.Log(notReservedResourceNumber(order.resource) > 0);
                Debug.Log((serfPoint - order.GetPoint()).magnitude < bestDist);
                if (notReservedResourceNumber(order.resource) > 0 && 
                    (serfPoint - order.GetPoint()).magnitude < bestDist )
                {
                    Debug.Log("****Order is the best!");
                    bestOrder = order;
                    bestDist = (serfPoint - order.GetPoint()).magnitude;
                }
            }
            if (bestOrder == null) // No lackOrders is found.
            {
                return null;
            }
            // Job is made from store to lackOrder.
            Debug.Log("****Job from store is made!!!");
            job = new DeliveryJob(bestOrder.resource, position, bestOrder.position);
            closeOrder(true, bestOrder, job);
            TryReserveResource(bestOrder.resource);
            return job;
        }
        bestDist = 100f;
        Order bestLackOrder = null;
        foreach (Order order in lackOrderList)
        {
            if (order.resource == bestOrder.resource && (serfPoint - order.GetPoint()).magnitude < bestDist)
            {
                bestLackOrder = order;
                bestDist = (serfPoint - order.GetPoint()).magnitude;
            }
        }
        if( bestLackOrder == null)
        {
            // Job is made from overOrder to store.
            job = new DeliveryJob(bestOrder.resource, bestOrder.position, position);
            closeOrder(false, bestOrder, job);
            return job;
        }
        job = new DeliveryJob(bestOrder.resource, bestOrder.position, bestLackOrder.position);
        closeOrder(false, bestOrder, job);
        closeOrder(true, bestLackOrder, job);
        return job;
    }

    public void lookForAgents()
    {
        List<House> houses = GameObject.Find("GameScript").GetComponent<PlayerManager>().houseList;
        //Debug.Log("lookForAgents: houses " + (houses == null).ToString());
        //Debug.Log("lookForAgents: houses length" + (houses.Count).ToString());
        foreach (House h in houses)
        {
            //Debug.Log("lookForAgents: ra "+ (ra == null).ToString());
            //Debug.Log("lookForAgents: position" + (position == null).ToString());
            if ((h.GetComponent<PositionScript>().GetTileBeforeEnter() - position.GetTileBeforeEnter()).magnitude < RADIUS)
            {
                ResourceAgent[] agentList = h.GetComponents<ResourceAgent>();
                foreach(ResourceAgent ra in agentList)
                {
                    ra.AddStore(this);
                }
                
            }
        }
    }

    void closeOrder(bool isLack, Order order, DeliveryJob job)
    {
        if (isLack)
        {
            lackOrderList.Remove(order);
            
        } else
        {
            overOrderList.Remove(order);
        }
        DeliveryJob.lookForAgent(order.position, order.resource).OrderIsTaken(order, job, isLack);
    }
}

public enum Resource
{
    Log,
    Stone,
    Board
}
