using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

class HarvestClass
{
    public UnitHarvester harvester;
    public float value;
}

public class Harvest : MonoBehaviour
{
    static Harvest instance;

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    Tilemap grid;

    [SerializeField]
    TileBase refTile;


    [SerializeField]
    Transform harvester;

    List<Vector3Int> tiles = new();
    static Dictionary<Vector3Int, HarvestClass> harvestTiles = new();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //if (!Mouse.current.leftButton.wasPressedThisFrame)
        //    return;

        //FindHarvestTile(harvester.position);
    }

    bool FindHarvestTile(Vector3 pos, out Vector3Int cellPos)
    {
        /*
        Debug.Log(harvester.position);

        Vector3Int cell = tilemap.WorldToCell(pos);
        Debug.Log(cell);
        Vector3Int gridCell = grid.WorldToCell(pos);
        Debug.Log(gridCell);

        if (tilemap.HasTile(cell))
        {
            TileBase tile = tilemap.GetTile(cell);
            Debug.Log(tile ? tile.name : "none");
            tilemap.SetTile(cell, null);
        } else
        {
            tilemap.SetTile(cell, refTile);
        }
        */

        cellPos = Vector3Int.zero;
        Vector3Int harvestCell = tilemap.WorldToCell(pos);

        tiles.Clear();
        foreach (Vector3Int cell in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(cell);
            if (tile)
                tiles.Add(cell);
        }

        if (tiles.Count < 1)
            return false;

        tiles.Sort((a, b) =>
        {
            float distanceToA = Vector3Int.Distance(harvestCell, a);
            float distanceToB = Vector3Int.Distance(harvestCell, b);

            return distanceToA.CompareTo(distanceToB);
        });

        cellPos = tiles[0];
        return true;
    }

    public static bool GetHarvestCell(UnitHarvester harvester, out Vector3Int cell)
    {
        return instance.FindHarvestTile(harvester.transform.position, out cell);
    }
    public static bool CanHarvestCell(Vector3 pos)
    {
        Vector3Int cell = instance.tilemap.WorldToCell(pos);
        if (!instance.tilemap.HasTile(cell))
            return false;

        return true;
    }

    public static bool TryHarvest(UnitHarvester harvester, out float value)
    {
        Vector3Int cell = instance.tilemap.WorldToCell(harvester.transform.position);

        if (!harvestTiles.TryGetValue(cell, out HarvestClass tile))
        {
            tile = new HarvestClass()
            {
                harvester = harvester,
                value = Random.Range(1, 2)
            };
            harvestTiles.Add(cell, tile);
        }

        if (tile.value <= 0.1f)
        {
            value = tile.value;

            harvestTiles.Remove(cell);
            instance.tilemap.SetTile(cell, null);
            return false;
        }

        value = Random.Range(0.075f, 0.1f);
        tile.value -= value;
        return true;
    }

    public static void EndHarvest(UnitHarvester harvester)
    {

    }
}
