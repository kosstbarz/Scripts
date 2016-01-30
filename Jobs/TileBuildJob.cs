using UnityEngine;
using System.Collections;

public class TileBuildJob : BuildJob {

    Vector2 tile;

    public TileBuildJob (Vector2 tile, float time) : base(time)
    {
        this.tile = tile;
    }
}
