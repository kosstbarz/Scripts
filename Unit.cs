using UnityEngine;
using System.Collections.Generic;

public class Unit {
    Vector2 TILE_CENTER = new Vector2(0.5f, 0.5f);
    PlayerManager player;
    int health;
    float normalSpeed = 1f;
    float speed = 1f;
    public Vector2 Pos { get; protected set; }
    Vector2 destination;
    Vector2 nextPoint;
    Queue<Vector2> path;
    BuildJob myJob;

    public Unit(Vector2 coord, PlayerManager player)
    {
        Pos = coord;
        nextPoint = Pos - TILE_CENTER;
        destination = Pos - TILE_CENTER;
        this.player = player;
    }

    // This method makes all time dependent stuff.
    public void TimeLap(float time)
    {
        speed = normalSpeed * MapController.Instance.mapData.tileData[(int)Pos.x, (int)Pos.y].speed;
        if (myJob == null)
        {
            myJob = BuildJobController.Instance.takeJob();
            BuildJobController.Instance.cbJobEnded += JobEnded;
            
            if (myJob != null)
            {
                destination = myJob.tile;
                path = PathfindingManager.Instance.GetPath(Pos - TILE_CENTER, destination);
                
                Debug.Log("Destination changed");
            }
        }
        if ((nextPoint + TILE_CENTER - Pos).magnitude <= speed * time)
        {
            // Unit has enought time to reach next point
            Move(nextPoint + TILE_CENTER);
        }
        else // Unit go in direction to nextPoint
        {
            Vector2 direction = nextPoint + TILE_CENTER - Pos;
            direction.Normalize();
            Vector2 newPos = Pos + direction * speed * time;
            Move(newPos);
        }
        if (nextPoint.Equals(Pos - TILE_CENTER))
        {
            if (path != null && path.Count > 0)
            {
                nextPoint = path.Dequeue();
                Debug.Log("New point dequeued" + nextPoint);
            }
            
        }
        if (destination.Equals(Pos - TILE_CENTER) && myJob != null)
        {
            myJob.DoWork(time);
        }
        
    }

    void Move(Vector2 coord)
    {
        Pos = coord;
        player.MoveUnit(this, coord);
    }

    public string GetHealth()
    {
        return health.ToString();
    }

    void JobEnded(BuildJob j)
    {
        if (j == myJob)
        {
            myJob = null;
            Debug.Log("Job ended!");
        }
    }
}
