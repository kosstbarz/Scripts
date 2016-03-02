using UnityEngine;
using System.Collections.Generic;

public class HouseBuildJob : BuildJob
{

    public PositionScript position;
    public int houseNumber;

    public HouseBuildJob(float time, PositionScript position, int number)
    {
        fullTime = timeLeft = time;
        this.position = position;
        houseNumber = number;
        tile = position.EnterTile - position.ToUp.normalized;
    }
    public override void OnComplete()
    {
        BuildJobController.Instance.InstantiateHouse(houseNumber, position);
        List<Vector2> tiles = position.GetTiles();
        foreach (Vector2 curr in tiles)
        {
            MapController.Instance.ChangeTileWithoutGraph(curr, "House"); 
        }
        BuildJobController.Instance.cbJobEnded(this);
    }
}
