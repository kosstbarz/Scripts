using UnityEngine;
using System.Collections.Generic;

public class BuildJobController : MonoBehaviour {

    public static BuildJobController Instance { get; protected set; }

    Queue<HouseBuildJob> houseBuildQueue;
    Queue<TileBuildJob> tileBuildQueue;

    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of BuildJobController can be created");
        }
        houseBuildQueue = new Queue<HouseBuildJob>();
        tileBuildQueue = new Queue<TileBuildJob>();
	}
	
	public void AddHouseJob() {

    }

    public void AddTileJob(Vector2 tile, float time, string type) {
        TileBuildJob job = new TileBuildJob(tile, (coord) => { MapController.Instance.ChangeTile(coord, "Road"); }) 
        
    }
}
