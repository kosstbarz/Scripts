using UnityEngine;
using System;
using System.Collections.Generic;


public class BuildJobController : MonoBehaviour {

    public static BuildJobController Instance { get; protected set; }
    public GameObject TileJobMarker;
    public GameObject jobMarkers;

    List<HouseBuildJob> houseBuildQueue;
    List<TileBuildJob> tileBuildQueue;
    
    Dictionary<BuildJob, GameObject> jobGO;
    Dictionary<Vector2, BuildJob> busyTiles;
    public Action<BuildJob> cbJobEnded; 

    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of BuildJobController can be created");
        }
        houseBuildQueue = new List<HouseBuildJob>();
        tileBuildQueue = new List<TileBuildJob>();
        jobGO = new Dictionary<BuildJob, GameObject>();
        cbJobEnded += jobEnded;
        busyTiles = new Dictionary<Vector2, BuildJob>();
	}
	
	public void AddHouseJob(float time, PositionScript position, int number) {
        HouseBuildJob job = new HouseBuildJob(time, position, number);
        houseBuildQueue.Add(job);
        List<Vector2> tiles = position.GetTiles();
        foreach (Vector2 curr in tiles)
        {
            busyTiles.Add(curr, job);
        }
    }
    // Here new job is created and registred in all dictionaries.
    public void AddTileJob(Vector2 tile, string type) {
        //Debug.Log("AddTileJob is run");
        TileBuildJob job = new TileBuildJob(tile, type);
        tileBuildQueue.Add(job);
        GameObject jobMarker = Instantiate(TileJobMarker);
        jobMarker.transform.position = new Vector3(tile.x + MapController.TILE_SIZE/2, MapController.Instance.mapData.GetHeight(tile), tile.y + MapController.TILE_SIZE / 2);
        jobGO.Add(job, jobMarker);
        jobMarker.name = "JOB " + tile.x + " " + tile.y;
        jobMarker.transform.SetParent(jobMarkers.transform);
        busyTiles.Add(tile, job);
    }

    public BuildJob takeJob()
    {
        if (tileBuildQueue.Count != 0)
        {
            BuildJob j = tileBuildQueue[0];
            tileBuildQueue.RemoveAt(0);
            return j;
        }
        if (houseBuildQueue.Count != 0)
        {
            BuildJob j = houseBuildQueue[0];
            houseBuildQueue.RemoveAt(0);
            return j;
        }
        return null;
    }

    // FIX ME: 
    void jobTaken(TileBuildJob job, GameObject worker)
    {

    }

    void jobEnded (BuildJob job)
    {
        if ( jobGO.ContainsKey(job)) Destroy(jobGO[job]);
        Debug.Log("Job ended!");

        if (job.GetType().Equals(typeof(TileBuildJob))){
            busyTiles.Remove(job.tile);
            TileBuildJob tileJob = (TileBuildJob) job;
            if (tileBuildQueue.Contains(tileJob))
            {
                tileBuildQueue.Remove(tileJob);
            }
        } else
        {
            List<Vector2> tiles = ((HouseBuildJob)job).position.GetTiles();
            foreach(Vector2 tile in tiles)
            {
                busyTiles.Remove(tile);
            }
        }
        
    }

    public bool IsBusy(Vector2 tile)
    {
        return busyTiles.ContainsKey(tile);
    }

    public BuildJob GetJob(Vector2 tile)
    {
        if (!busyTiles.ContainsKey(tile)) return null;
        return busyTiles[tile];
    }

    public void InstantiateHouse(int number, PositionScript position)
    {
        GameObject newHouse = Instantiate(BuildMode.Instance.objects[number]);
        newHouse.GetComponent<PositionScript>().SynchronizePosition(position);
        Vector2 coord = position.EnterTile;
        Vector3 coordCenter = new Vector3(coord.x + MapController.TILE_SIZE / 2, MapController.Instance.mapData.GetHeight(coord), coord.y + MapController.TILE_SIZE / 2);
        newHouse.transform.position = coordCenter;
        Vector3 forward = new Vector3(position.ToUp.x, 0f, position.ToUp.y);
        newHouse.transform.rotation = Quaternion.LookRotation(forward);
    }
}
