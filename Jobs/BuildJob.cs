using System;

public abstract class BuildJob {

    float time;

    Action onComplete;

    public BuildJob (float time, Action onComplete)
    {
        this.time = time;
        this.onComplete = onComplete;
    }
}
