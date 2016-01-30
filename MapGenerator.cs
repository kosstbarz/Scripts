using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MapController))]
public class MapGenerator : MonoBehaviour {

    public GameObject map;
    public int xSize;
    public int ySize;

    MapController mapController;
    MapData data;
    const int textureSize = 512;

    float tileSize;

    int textureWidth;
    
    LandscapeGenerator landGenerator;

	// Use this for initialization
	void Start () {
        mapController = GetComponent<MapController>();
        textureWidth = mapController.texture.width;
        
        tileSize = MapController.TILE_SIZE;

    }
	

    public void GenerateMap()
    {
        data = new MapData(xSize, ySize);
        mapController.mapData = data;
        if (landGenerator == null) landGenerator = new LandscapeGenerator(xSize, ySize);
        GenerateData();
        GenerateMesh();
        
    }

    protected void GenerateData()
    {
        // All tiles set to grass and height set with random.
        for (int y = 0; y < ySize + 1; y++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                if (y < ySize && x < xSize) data.tileData[x, y] = mapController.mapData.GetType("Grassland");
                data.heightData[x, y] = Random.Range(1f, 1.3f);
            }
        }
        
        landGenerator.FullGenerate();
    }
    protected void GenerateMesh()
    {
                
        int vertNumber = xSize * ySize * 4;
        Vector3[] vertices = new Vector3[vertNumber];
        Vector3[] normals = new Vector3[vertNumber];
        Vector2[] uv = new Vector2[vertNumber];
        int[] triangles = new int[xSize * ySize * 6];

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                int tileTypeNum = (int)data.tileData[x, y].name;
               
                vertices[(y * xSize + x) * 4]     = new Vector3(tileSize * x, data.heightData[x, y], tileSize * y);
                vertices[(y * xSize + x) * 4 + 1] = new Vector3(tileSize * (x + 1), data.heightData[x + 1, y], tileSize * y);
                vertices[(y * xSize + x) * 4 + 2] = new Vector3(tileSize * x, data.heightData[x, y + 1], tileSize * (y + 1));
                vertices[(y * xSize + x) * 4 + 3] = new Vector3(tileSize * (x + 1), data.heightData[x + 1, y + 1], tileSize * (y + 1));

                normals[(y * xSize + x) * 4] = Vector3.up;
                normals[(y * xSize + x) * 4 + 1] = Vector3.up; ;
                normals[(y * xSize + x) * 4 + 2] = Vector3.up; ;
                normals[(y * xSize + x) * 4 + 3] = Vector3.up; ;

                //Debug.Log("uvs[" + tileTypeNum + "][0] ");
                for (int i = 0; i < 4; i++)
                {
                    uv[(y * xSize + x) * 4 + i] = mapController.uvs[tileTypeNum][i];
                }
                             

                triangles[(y * xSize + x) * 6] = (y * xSize + x) * 4;
                triangles[(y * xSize + x) * 6 + 1] = (y * xSize + x) * 4 + 2;
                triangles[(y * xSize + x) * 6 + 2] = (y * xSize + x) * 4 + 3;
                triangles[(y * xSize + x) * 6 + 3] = (y * xSize + x) * 4;
                triangles[(y * xSize + x) * 6 + 4] = (y * xSize + x) * 4 + 3;
                triangles[(y * xSize + x) * 6 + 5] = (y * xSize + x) * 4 + 1;
            }
        }

        // Create mesh
        Mesh mesh = new Mesh();
        mesh.name = "mapMesh";
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        // Assign mesh to map object
        if (!map)
        {
            map = new GameObject("Map", typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer));
        }
        MeshFilter filter = map.GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
        MeshRenderer renderer = map.GetComponent<MeshRenderer>();
        //renderer.sharedMaterial = new Material()
        renderer.sharedMaterial.SetTexture(0, mapController.texture);
        MeshCollider collider = map.GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        map.layer  = LayerMask.NameToLayer("Ground");
        mapController.mesh = mesh;
        mapController.uv = uv;

    }

    
}
