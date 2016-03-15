using UnityEngine;
using System.Collections.Generic;


public class ResourceAgent : MonoBehaviour
{

    public int neededCount;
    public int currCount; //{ get; protected set; }
    public Resource myRes; //One agent controls one resource.
    List<ResourceController> storeList; //List of strores nearby to give orders.
    PlayerManager player;
    List<Order> myOrders;
    List<DeliveryJob> myJobs;

    void OnEnable()
    {
        storeList = new List<ResourceController>();
        player = GameObject.Find("GameScript").GetComponent<PlayerManager>();
        myOrders = new List<Order>();
        myJobs = new List<DeliveryJob>();
        currCount = 10;
        
    }

    public void OnBuild()
    {
        lookForStores();
        updateOrders();
    }
    void lookForStores()
    {
        foreach (ResourceController rc in player.storeList)
        {
            //Debug.Log((rc.position.GetTileBeforeEnter() - position.GetTileBeforeEnter()).magnitude.ToString());
            //Debug.Log(rc.position.GetTileBeforeEnter());
            //Debug.Log(position.GetTileBeforeEnter());
            if ((rc.position.GetTileBeforeEnter() - GetComponent<PositionScript>().GetTileBeforeEnter()).magnitude < ResourceController.RADIUS)
            {
                storeList.Add(rc);
                Debug.Log("lookForStores: added store. Is it null? " + (rc == null).ToString());
            }
        }
    }

    public void AddStore(ResourceController store)
    {
        if (!storeList.Contains(store))
        {
            storeList.Add(store);
            foreach(Order order in myOrders)
            {
                store.AddOrder(order, neededCount != 0);
                //Debug.Log("AddStore: order added");
            }
        }
    }
    public void OrderIsTaken(Order order, DeliveryJob job, bool isLack)
    {
        foreach (ResourceController rc in storeList)
        {
            rc.RemoveOrder(order, isLack);
        }
        myOrders.Remove(order);
        myJobs.Add(job);
    }
    public void JobIsEnded(DeliveryJob job)
    {
        Debug.Log("JobIsEnded: " + myRes + " Left jobs " + myJobs.Count.ToString());
        myJobs.Remove(job);
        Debug.Log("JobIsEnded: " + myRes + " Left jobs " + myJobs.Count.ToString());
        Debug.Log("JobIsEnded: " + myRes + " Left orders " + myOrders.Count.ToString());
    }
    void MakeOrder(bool isLack)
    {
        Order order = new Order(myRes, GetComponent<PositionScript>());
        foreach (ResourceController rc in storeList)
        {
            rc.AddOrder(order, isLack);
        }
        myOrders.Add(order);
    }

    void updateOrders()
    {
        if (neededCount == 0)
        {
            int over = currCount - myOrders.Count - myJobs.Count;
            for (int i = 0; i < over; i++)
            {
                MakeOrder(false);
                Debug.Log("updateOrders: Over order is made.");
            }
        } else
        {
            int lack = neededCount - currCount - myOrders.Count - myJobs.Count;
            for (int i = 0; i < lack; i++)
            {
                MakeOrder(true);
                Debug.Log("updateOrders: Lack order is made.");
            }
        }
    }
    public void TakeResource()
    {
        if (currCount > 0)
        {
            currCount--;
            if (GetComponent<House>().someChanges != null) GetComponent<House>().someChanges();
            updateOrders();
        }
        else
        {
            Debug.LogError("TakeResource: There is no resource in house.");
        }
    }
    public void BringResource()
    {
        currCount++;
        Debug.Log("BringResource: " + myRes + " now " + currCount);
        updateOrders();
        if (GetComponent<House>().someChanges != null) GetComponent<House>().someChanges();
    }
}
