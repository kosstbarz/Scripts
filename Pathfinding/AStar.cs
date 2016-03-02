using UnityEngine;
using System.Collections.Generic;
using Priority_Queue;
using System.Linq;

public class AStar{

    private PathfindingGraph graph;

    public AStar(PathfindingGraph graph)
    {
        this.graph = graph;
    }

	public Queue<Vector2> GetPath(Node start, Node goal)
    {
        List<Node> closedSet = new List<Node>();  // The set of nodes already evaluated
        SimplePriorityQueue<Node> openSet = new SimplePriorityQueue<Node>(); // The set of tentative nodes to be evaluated
        openSet.Enqueue(start, 0);
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();

        Dictionary<Node, float> g_score = new Dictionary<Node, float>();
        
        Dictionary<Node, float> f_score = new Dictionary<Node, float>();

        foreach (Node node in graph.nodes.Values)
        {
            g_score[node] = Mathf.Infinity;
            f_score[node] = Mathf.Infinity;
        }
        g_score[start] = 0f;
        f_score[start] = heuristic_cost_estimate(start, goal);

        while(openSet.Count > 0)
        {
            Node current = openSet.Dequeue();
            
            if(current == goal )
            {
                return reconstruct_path(came_from, current);
            }
            closedSet.Add(current);

            foreach(Node neighbour in current.edges.Keys)
            {
                
                if (closedSet.Contains(neighbour))
                    continue;
                float tentative_g_score = g_score[current] + dist_between(current, neighbour); // length of this path.
                if (openSet.Contains(neighbour) && tentative_g_score >= g_score[neighbour])
                {
                    continue;
                }
                came_from[neighbour] = current;
                g_score[neighbour] = tentative_g_score;
                f_score[neighbour] = g_score[neighbour] + heuristic_cost_estimate(neighbour, goal);
                openSet.Enqueue(neighbour, f_score[neighbour]);
            }
        }
        // Failed to find a path.
        return null;
    }

    private float heuristic_cost_estimate( Node start, Node goal)
    {
        Vector2 dist =  start.place - goal.place;
        return dist.magnitude / 2;
    }

    private Queue<Vector2> reconstruct_path(Dictionary<Node, Node> came_from, Node current)
    {
        Queue<Vector2> path = new Queue<Vector2>();
        path.Enqueue(current.place);
        while(came_from.ContainsKey(current))
        {
            current = came_from[current];
            path.Enqueue(current.place);
        }
        return new Queue<Vector2>( path.Reverse());
    }

    float dist_between(Node curr, Node dest)
    {
        return curr.edges[dest];

        if (curr.place.x == dest.place.x || curr.place.y == dest.place.y)
        {
            return 1f;
        }
        return 1.41421356237f;
    }
}
