using UnityEngine;
using System.Collections.Generic;

public class BuildJobController : MonoBehaviour {

    Queue<HouseBuildJob> houseBuildQueue;
    Queue<TileBuildJob> tileBuildQueue;

    // Use this for initialization
    void Start () {
        houseBuildQueue = new Queue<HouseBuildJob>();
        tileBuildQueue = new Queue<TileBuildJob>();
	}
	
	public void AddHouseJob() {

    }

    public void AddTileJob() {

    }
}
