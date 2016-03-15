using UnityEngine;
using System.Collections.Generic;

public class MoveAgent {

    public static Vector2 TILE_CENTER = new Vector2(0.5f, 0.5f);

    public float normalSpeed;
    float speed;
    public Vector2 Pos { get; protected set; } // Real position
    public Vector2 destination; // Coordinate of tile (corner coord)
    Vector2 nextPoint; // Coordinate of tile (corner coord)
    public Queue<Vector2> path;
    PlayerManager player;
    Unit unit;

    public MoveAgent (float speed, Vector2 position, PlayerManager player, Unit unit)
    {
        normalSpeed = speed;
        Pos = position;
        nextPoint = Pos - TILE_CENTER;
        destination = Pos - TILE_CENTER;
        this.player = player;
        this.unit = unit;
    }

    public float GetSpeed()
    {
        return normalSpeed * MapController.Instance.mapData.tileData[(int)Pos.x, (int)Pos.y].speed;
    }

    public void TimeLap(float time)
    {
        speed = GetSpeed();
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
                //Debug.Log("New point dequeued" + nextPoint);
            }

        }
    }

    void Move(Vector2 coord)
    {
        Pos = coord;
        player.MoveUnit(unit, coord);
    }

}
