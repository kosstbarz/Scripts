using UnityEngine;
using System.Collections;

public abstract class Unit {
    protected MoveAgent moveAgent;
    protected PlayerManager player;
    protected ResourceController jobController;
    // Use this for initialization
    public Unit (Vector2 coord, PlayerManager player, ResourceController jobController) {
        this.player = player;
        this.jobController = jobController;
        moveAgent = new MoveAgent(1.5f, coord, player, this);
    }
    public abstract void TimeLap(float time);
}
