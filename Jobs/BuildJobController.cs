using UnityEngine;
using System;
using System.Collections.Generic;

public class BuildJobController : MonoBehaviour {

    public static BuildJobController Instance { get; protected set; }
    public GameObject TileJobMarker;
    public GameObject jobMarkers;

    Queue<HouseBuildJob> houseBuildQueue;
    List<TileBuildJob> tileBuildQueue;
    
    Dictionary<TileBuildJob, GameObject> jobGO;
    Dictionary<Vector2, TileBuildJob> busyTiles;
    public Action<TileBuildJob> cbJobEnded; 

    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of BuildJobController can be created");
        }
        houseBuildQueue = new Queue<HouseBuildJob>();
        tileBuildQueue = new List<TileBuildJob>();
        jobGO = new Dictionary<TileBuildJob, GameObject>();
        cbJobEnded += jobEnded;
        busyTiles = new Dictionary<Vector2, TileBuildJob>();
	}
	
	public void AddHouseJob() {

    }
    // Here new job is created and registred in all dictionaries.
    public void AddTileJob(Vector2 tile, string type) {
        Debug.Log("AddTileJob is run");
        TileBuildJob job = new TileBuildJob(tile, type);
        tileBuildQueue.Add(job);
        GameObject jobMarker = Instantiate(TileJobMarker);
        jobMarker.transform.position = new Vector3(tile.x + MapController.TILE_SIZE/2, MapController.Instance.mapData.GetHeight(tile), tile.y + MapController.TILE_SIZE / 2);
        jobGO.Add(job, jobMarker);
        jobMarker.name = "JOB " + tile.x + " " + tile.y;
        jobMarker.transform.SetParent(jobMarkers.transform);
        busyTiles.Add(tile, job);
    }

    public TileBuildJob takeJob()
    {
        if (tileBuildQueue.Count != 0)
        {
            TileBuildJob j = tileBuildQueue[0];
            tileBuildQueue.RemoveAt(0);
            return j;
        }
        return null;
    }

    // FIX ME: 
    void jobTaken(TileBuildJob job, GameObject worker)
    {

    }

    void jobEnded (TileBuildJob job)
    {
        Destroy(jobGO[job]);
        busyTiles.Remove(job.tile);
        if (tileBuildQueue.Contains(job))
        {
            tileBuildQueue.Remove(job);
        }
    }

    public bool IsBusy(Vector2 tile)
    {
        return busyTiles.ContainsKey(tile);
    }

    public TileBuildJob GetJob(Vector2 tile)
    {
        if (!busyTiles.ContainsKey(tile)) return null;
        return busyTiles[tile];
    }
}
