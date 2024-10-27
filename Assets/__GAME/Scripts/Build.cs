using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.LightTransport;
using UnityEngine.Tilemaps;

public enum BuildKind
{
    None, Slab, Slab1, Windtrap, Refinery, Turret, TurretRocket, Construction, Radar, Wor
}

public enum BuildMode
{
    None, Place
}

[System.Serializable]
public class ConstructionData
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    public BuildKind kind;
    public GameObject prefab;
    public Vector2Int size;
    public float buildTime;
}

public class Build : MonoBehaviour
{
    static Build instance;
    public static Action OnBuild;

    static public BuildMode mode = BuildMode.None;
    static BuildKind kind = BuildKind.None;

    static Dictionary<Vector3Int, BuildKind> buildingCells = new Dictionary<Vector3Int, BuildKind>();

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Tilemap buildGrid;
    [SerializeField]
    List<ConstructionData> constructionsList;

    public static ConstructionData construction(BuildKind kind) => instance.constructionsList.Find((c) => c.kind == kind);

    static Transform placeConstruction;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (mode != BuildMode.Place)
            return;
        if (UI.OwerUI())
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (placeConstruction)
                Destroy(placeConstruction.gameObject);

            mode = BuildMode.None;
            kind = BuildKind.None;

            return;
        }


        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);

        placeConstruction.position = CellPosition(hit.point);

        bool tileIsBussy = BussyTile(hit.point);
        placeConstruction.GetComponent<Building>().SetPlaceType(tileIsBussy);

        if (!Mouse.current.leftButton.wasPressedThisFrame || !tileIsBussy)
            return;

        if (placeConstruction)
            Destroy(placeConstruction.gameObject);

        ConstructionData data = construction(kind);
        GameObject o = Instantiate(data.prefab, placeConstruction.position, Quaternion.identity);
        AddBuilding(placeConstruction.position, kind);

        mode = BuildMode.None;
        kind = BuildKind.None;

        OnBuild?.Invoke();
    }

    public static void StartPlace(BuildKind kind)
    {
        Build.kind = kind;
        Build.mode = BuildMode.Place;


        if (placeConstruction != null)
            Destroy(placeConstruction.gameObject);

        ConstructionData data = construction(kind);

        placeConstruction = Instantiate(data.prefab).transform;

    }
    public static void AddBuilding(Vector3 world, BuildKind kind)
    {
        Vector3Int cell = instance.buildGrid.WorldToCell(world); 
        AddBuilding(cell, kind);
    }

    public static void AddBuilding(Vector3Int cell,  BuildKind kind)
    {
        ConstructionData data = construction(kind);
        for (int x = 0; x < data.size.x; x++)
            for (int y = 0; y < data.size.y; y++)
            {
                Vector3Int p = new Vector3Int(cell.x + x, cell.y + y);
                buildingCells.Add(p,kind);
            }
        Debug.Log("Add building " + kind);
    }

    public static Vector3Int GetCell(Vector3 world)
    {
        return instance.buildGrid.WorldToCell(world);
    }

    public static Vector3 CellPosition(Vector3 world)
    {
        Vector3Int cell = instance.buildGrid.WorldToCell(world);
        return instance.buildGrid.CellToWorld(cell);
    }

    public static Vector3 CellPosition(Vector3Int cell)
    {
        return instance.buildGrid.CellToWorld(cell);
    }

    public static bool BussyTile(Vector3Int cell)
    {
        if (!instance.buildGrid.HasTile(cell))
            return false;

        return !buildingCells.ContainsKey(cell);
    }
    public static bool BussyTile(Vector3 world)
    {
        Vector3Int cell = instance.buildGrid.WorldToCell(world);
        return BussyTile(cell);
    }
}
