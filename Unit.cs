using UnityEngine;


public class Unit {

    PlayerManager player;
    int health;
    float speed = 0.5f;
    public Vector2 Pos { get; protected set; }
    public float X { get; protected set; }
    public float Y { get; protected set; }
    Vector2 destination;
    TileBuildJob myJob;

    public Unit(Vector2 coord, PlayerManager player)
    {
        Pos = coord;
        destination = Pos;
        this.player = player;
    }

    // This method makes all time dependent stuff.
    public void TimeLap(float time)
    {
        
        if (myJob == null)
        {
            myJob = BuildJobController.Instance.takeJob();
            BuildJobController.Instance.cbJobEnded += JobEnded;
            if (myJob != null)
            {
                destination = myJob.tile;
                Debug.Log("Destination changed");
            }
        }
        if ((destination - Pos).magnitude <= speed * time)
        {
            // Unit has enought time to reach destination
            Move(destination);
        } else // Unit go in direction to destination
        {
            Vector2 direction = destination - Pos;
            direction.Normalize();
            Vector2 newPos = Pos + direction * speed * time;
            Move(newPos);
        }
        if(destination.Equals(Pos) && myJob != null)
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

    void JobEnded(TileBuildJob j)
    {
        if (j == myJob)
        {
            myJob = null;
        }
    }
}
