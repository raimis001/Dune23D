using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitHarvester : Unit
{
    [SerializeField]
    GameObject harvestAnim;
    [SerializeField]
    TMP_Text infoText;


    float reactionTime = 2;
    Vector3Int harvestCell;
    float harvestingTime;
    float _storage;
    internal float storage
    {
        get => _storage;
        set
        {
            _storage = value;
            infoText.text = (Mathf.Clamp(_storage, 0, 1) * 100f).ToString("0") + "\n%";
        }
    }

    BuildingRefinery refinery;

    private void Start()
    {
        harvestAnim.SetActive(false);
        storage = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (status == 0)
        {
            reactionTime -= Time.deltaTime;
            if (reactionTime > 0)
                return;

            if (Harvest.GetHarvestCell(this,out harvestCell))
            {
                MoveToPosition(harvestCell);
                status = 1;
            }
        }

        if (status == 5)
        {
            harvestingTime -= Time.deltaTime;
            if (harvestingTime > 0)
                return;

            harvestingTime = Random.Range(5f, 6f);

            bool cont = Harvest.TryHarvest(this, out float value);
            
            storage += value;
            
            if (storage >= 1)

            {
                storage = 1;
                //TODO return to base
                harvestingTime = 0;
                status = 6;
                harvestAnim.SetActive(false);
                Harvest.EndHarvest(this);
                return;
            }

            if (!cont)
            {
                if (Harvest.GetHarvestCell(this, out harvestCell))
                {
                    MoveToPosition(harvestCell);
                    status = 1;
                } else
                {
                    reactionTime = 2;
                    status = 0;
                }

            }

        }

        if (status == 6)
        {
            BuildingRefinery target = FindRefinery();
            if (target == null)
                return;

            MoveToPosition(target.harvesterPoint.position);


            if (HasDestination())
            {
                status = 7;
                transform.SetPositionAndRotation(target.harvesterPoint.position, target.harvesterPoint.rotation);
                target.Unload(this);
            }

            return;
        }

        if (status == 7)
        {
            if (storage > 0)
                return;

            status = 0;
        }
    }

    protected override void OnStop()
    {
        Vector3Int cell = MoveGrid.GetCell(transform.position);

        Debug.LogFormat("Stop at {0} dest: {1}", cell, harvestCell);

        if (!Harvest.CanHarvestCell(transform.position))
            return;

        StartHarvesting();
    }

    void StartHarvesting()
    {
        status = 5;
        harvestingTime = 5;
        harvestAnim.SetActive(true);
    }

    BuildingRefinery FindRefinery()
    {
        if (refinery)
            return refinery;

        BuildingRefinery[] buildings = FindObjectsByType<BuildingRefinery>(FindObjectsSortMode.None);

        if (buildings.Length == 0)
            return null;

        List<BuildingRefinery> list = new();

        foreach (BuildingRefinery b in buildings)
        {
            if (b.race != race)
                continue;

            list.Add(b);
        }

        list.AddRange(buildings);

        Vector3 pos = transform.position;
        list.Sort((a, b) =>
        {
            float distanceToA = Vector3.Distance(pos, a.transform.position);
            float distanceToB = Vector3.Distance(pos, b.transform.position);

            return distanceToA.CompareTo(distanceToB);
        });

        refinery = list[0];

        return refinery;
    }
}
