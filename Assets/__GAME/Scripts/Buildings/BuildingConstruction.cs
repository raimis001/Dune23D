using System;
using UnityEngine;

public class BuildingConstruction : Building
{
    internal BuildKind currentBuild = BuildKind.None;

    internal bool started = false;
    internal bool ready = false;
    
    float buildTime = 0;
    float currentTime = 0;

    internal float progress => !started ? 0 : ready ? 1 : 1 - currentTime / buildTime;
    internal Action OnBuildingReady;

    [SerializeField]
    bool testing = true;



    public void ChoiceBuilding(BuildKind kind)
    {
        currentBuild = kind;

        ConstructionData construction = Build.construction(kind);

        buildTime = construction.buildTime; 
    }

    public void StartBuild()
    {
        if (started)
            return;

        started = true;
        ready = false;

        currentTime = buildTime;
    }

    public void Reset()
    {
        started = false;
        ready = false;
        currentTime = 0;
    }

    private void Update()
    {
        if (testing && currentBuild == BuildKind.None)
            ChoiceBuilding(BuildKind.Slab);

        if (!started || ready)
            return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ready = true;
            OnBuildingReady?.Invoke();
        }
    }
}
