using UnityEngine;
using System.Collections.Generic;

public class PathfindingManager : MonoBehaviour {

    public static PathfindingManager Instance { get; protected set; }

    PathfindingGraph graph; // Set of nodes, connected with edges
    AStar astar; // Calculation of path on graph

	// Use this for initialization
	void Start () {
        if (Instance == null){
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of PathfindingManager can be created");
        }
        graph = new PathfindingGraph();
        
    }
	
    public Queue<Vector2> GetPath(Vector2 start, Vector2 goal)
    {
        if (graph.nodes == null)
        {
            BuildGraph();
            astar = new AStar(graph);
        }
        Vector2 begin = new Vector2(Mathf.Round(start.x), Mathf.Round(start.y));
        return astar.GetPath(graph.nodes[begin], graph.nodes[goal]);
    }
	
    public void UpdateGraph(Vector2 point)
    {
        if (graph.nodes == null)
        {
            BuildGraph();
            astar = new AStar(graph);
            return;
        }
        MapData data = MapController.Instance.mapData;
        List<Vector2> neighbors = data.GetNeighbors(point);
        TileType type = data.tileData[(int)point.x, (int)point.y];
        if (!type.walkable) // Tile became unwalkable
        {
            Node deleting = graph.nodes[point];
            graph.nodes.Remove(point);
            List<Node> notDiagNeighbors = new List<Node>();
            Node nei;
            foreach (Vector2 neighbor in neighbors)
            {
                if (graph.nodes.TryGetValue(neighbor, out nei))
                {
                    graph.nodes[neighbor].edges.Remove(deleting);
                    bool diag = (point.x - neighbor.x) * (point.y - neighbor.y) != 0f;
                    if (!diag) notDiagNeighbors.Add(graph.nodes[neighbor]);
                }
            }
            // In order to avoid diagonal moves near unwalkable tiles.
            foreach (Node n in notDiagNeighbors)
            {
                foreach (Node n2 in notDiagNeighbors)
                {
                    n.edges.Remove(n2);
                }
            }
        }
        else // Tile is walkable. Need to change edges
        {
            if (!graph.nodes.ContainsKey(point))
            {
                graph.nodes.Add(point, new Node(point));
            }
            Node n = graph.nodes[point];
            Node nei;
            foreach (Vector2 neighbor in neighbors)
            {
                if (graph.nodes.TryGetValue(neighbor, out nei))
                {
                    n.edges[nei] = 1 / data.tileData[(int)neighbor.x, (int)neighbor.y].speed;

                    nei.edges[n] = 1 / data.tileData[(int)n.place.x, (int)n.place.y].speed;
                }
            }
        }
    }

    void BuildGraph()
    {
        graph.nodes = new Dictionary<Vector2, Node>();
        MapData data = MapController.Instance.mapData;
        // Nodes added to dictionary
        for (int y = 0; y < data.tileData.GetLength(1); y++)
        {
            for (int x = 0; x < data.tileData.GetLength(0); x++)
            {
                if (data.tileData[x,y].walkable)
                {
                    Vector2 v = new Vector2(x, y);
                    graph.nodes.Add(v, new Node(v));
                }
            }
        }
        // Edges added to nodes
        foreach (Node node in graph.nodes.Values)
        {
            List<Vector2> neighbors = data.GetNeighbors(node.place);
            foreach (Vector2 neighbor in neighbors)
            {
                int nX = (int) neighbor.x;
                int nY = (int) neighbor.y;
                bool diag = (nX - node.place.x) * (nY - node.place.y) != 0f;
                bool diag_check = !diag || (data.tileData[nX, (int)node.place.y].walkable && data.tileData[(int)node.place.x, nY].walkable);
                if (data.tileData[nX, nY].walkable && diag_check) {
                    node.edges.Add(graph.nodes[neighbor], 1 / data.tileData[nX, nY].speed);
                }
            }
            
        }    
        
        
    }
}
