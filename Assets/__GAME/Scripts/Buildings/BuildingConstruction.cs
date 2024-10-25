using UnityEngine;

public class BuildingConstruction : Building
{
    BuildKind currentBuild = BuildKind.None;

    bool started = false;
    bool ready = false;
    float buildTime = 0;
    float currentTime = 0;

    float progress => !started ? 0 : ready ? 1 : 1 - currentTime / buildTime;

    public void StartBuild(BuildKind kind)
    {
        started = true;
        ready = false;

        currentBuild = kind;
        buildTime = 5; //TODO read build time
        currentTime = buildTime;
    }

    private void Update()
    {
        if (!started || ready)
            return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            ready = true;
    }
}
