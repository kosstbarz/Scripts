using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class TileType {

    public Types name;
    public bool walkable;
    public float speed;
    public TileType(Types name, bool walkable, float speed)
    {
        this.name = name;
        this.walkable = walkable;
        this.speed = speed;
    }  
   
}

public enum Types
{
    Grassland,
    Road,
    Mountain,
    Water,
    Swamp,
    Unknown,
    House
}

