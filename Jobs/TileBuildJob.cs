using UnityEngine;
using System;

public class TileBuildJob : BuildJob {

    Vector2 tile;



    public TileBuildJob (Vector2 tile, float time, Action onComplete) : base(time, onComplete)
    {
        this.tile = tile;
    }
}
