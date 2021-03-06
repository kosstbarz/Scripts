﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(PositionScript))]
[RequireComponent(typeof(JobInfo))]
public class GhostScript : MonoBehaviour {

    
    public float MAX_SLOPE = 2f;
    public int BORDER = 3;
    public int number; // Number of house in objects array

    Vector2 currTile;
    Renderer renderer;
    PositionScript positionScript;
    JobInfo jobInfo;
    Vector2 leftCorner;

    // Use this for initialization
    void Start() {
        //GameObject gameScript = GameObject.Find("GameScript");
        name = "Ghost";
        renderer = GetComponentInChildren<Renderer>();
        renderer.material = BuildMode.Instance.ghostGreen;
        renderer.receiveShadows = false;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        renderer.gameObject.layer = LayerMask.NameToLayer("Ghost");
        positionScript = GetComponent<PositionScript>();
        leftCorner = positionScript.LeftCorner();
        jobInfo = GetComponent<JobInfo>();
        MouseController.Instance.RegisterTileChanged(ChangePosition);
        MouseController.Instance.ghost = this;
        MouseController.Instance.TurnOnHouseMode();
    }

    void ChangePosition(Vector2 coord)
    {
        currTile = coord;
        if(currTile.x >= 0)
        {
            positionScript.EnterTile = coord;
            Vector3 coordCenter = new Vector3(coord.x + MapController.TILE_SIZE/2, MapController.Instance.mapData.GetHeight(coord), coord.y + MapController.TILE_SIZE/2);
            renderer.material = CanBuild() ? BuildMode.Instance.ghostGreen : BuildMode.Instance.ghostRed;
            transform.position = coordCenter;
        }
         
    }

    public void Rotate()
    {
        Vector3 axis = new Vector3(0f, 1f);
        transform.Rotate(axis, -90f);
        positionScript.Rotate();
    }

    public bool CanBuild()
    {
        if (currTile.x < 0) return false;

        List<Vector2> tiles = positionScript.GetTiles();
        foreach (Vector2 curr in tiles)
        {
            int x = (int)curr.x;
            int y = (int)curr.y;
            if (!MapController.Instance.mapData.tileData[x, y].walkable)
            {
                //Debug.Log("Tile " + i + ", " + j + " is not walkable");
                return false;
            }
            if (x < BORDER || y < BORDER || x > MapController.Instance.mapData.xSize - BORDER || y > MapController.Instance.mapData.ySize - BORDER)
            {
                //Debug.Log("Too close to border of world");
                return false;
            }
        }
        
        return MapController.Instance.mapData.GetSlope(positionScript.LeftCorner(), positionScript.XLength(), positionScript.YLength()) < MAX_SLOPE;
    }

    public void AddJob()
    {
        BuildJobController.Instance.AddHouseJob(jobInfo.jobTime, positionScript, number);
        List<Vector2> tiles = positionScript.GetTiles();
        foreach (Vector2 curr in tiles)
        {
            MapController.Instance.ChangeTile(curr, "Unknown"); //FIXME
        }

    }

    void OnDestroy()
    {
        MouseController.Instance.UnregisterTileChanged(ChangePosition);
        MouseController.Instance.ghost = null;
        MouseController.Instance.TurnOnTileMode();
    }

    
}
