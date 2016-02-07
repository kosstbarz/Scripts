using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour {

    public static MapController Instance { get; protected set; }

    public const float TILE_SIZE = 1;
    public Texture2D texture;
    public int textureSize;
    
    public Mesh mesh;
    // Copy of UVs for all points of the mesh.
    public Vector2[] uv;

    // UV coordinates of each texture from "texture"
    public Dictionary<int, Vector2[]> uvs = new Dictionary<int, Vector2[]>();
    
    int numberOfTileTypes;
    int textureInRow;

    // Pure map data without graphics.
    public MapData mapData { get; set; }


    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of MouseControl can be created");
        }
        mapData = new MapData(50, 50);
        numberOfTileTypes = mapData.tileTypes.Length;
        textureInRow = texture.width / textureSize;
        CalculateUV();
        GetComponent<MapGenerator>().GenerateMap();
        GetComponent<PlayerManager>().AddUnit(new Vector2(25f, 25f));

    }
	
    public void CalculateUV()
    {
        for (int i = 0; i < numberOfTileTypes; i++)
        {
            Vector2[] arr = new Vector2[4];
            arr[0] = new Vector2((float)(textureSize * (i % textureInRow)) / texture.width, textureSize * ((float)(i / textureInRow)) / texture.height);
            arr[1] = new Vector2((float)(textureSize * (i % textureInRow + 1)) / texture.width, textureSize * ((float)(i / textureInRow)) / texture.height);
            arr[2] = new Vector2((float)(textureSize * (i % textureInRow)) / texture.width, textureSize * ((float)(i / textureInRow + 1)) / texture.height);
            arr[3] = new Vector2((float)(textureSize * (i % textureInRow + 1)) / texture.width, textureSize * ((float)(i / textureInRow + 1)) / texture.height);
            uvs.Add(i, arr);
            
        }
        Debug.Log("Calculate UV");
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeTile(Vector2 tileCoord, string name)
    {
        TileType type = mapData.GetType(name);
        mapData.SetTile(tileCoord, type);
        int x = (int)tileCoord.x;
        int y = (int)tileCoord.y;
        int t = (int)type.name;
        
        for (int i = 0; i < 4; i++)
        {
            
            uv[(y * mapData.xSize + x) * 4 + i] = uvs[t][i];
        }
        mesh.uv = uv;

    }
}
