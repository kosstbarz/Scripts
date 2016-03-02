using UnityEngine;
using System.Collections.Generic;

public class Node {

    public Vector2 place;
    public Dictionary<Node, float> edges;

    public Node(Vector2 place)
    {
        this.place = place;
        edges = new Dictionary<Node, float>();
    }
}
