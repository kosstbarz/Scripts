using UnityEngine;
using System.Collections;

public class Order {

    public Resource resource;
    public PositionScript position;

    public Order(Resource resource, PositionScript position)
    {
        this.resource = resource;
        this.position = position;
    }

    public Vector2 GetPoint()
    {
        return position.GetTileBeforeEnter();
    }
}
