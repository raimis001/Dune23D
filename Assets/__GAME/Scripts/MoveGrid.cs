using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class MoveGrid : MonoBehaviour
{
    static MoveGrid instance;

    [SerializeField]
    Tilemap grid;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3Int GetCell(Vector3 world)
    {
        return instance.grid.WorldToCell(world); 
    }

    public static Vector3 CellPosition(Vector3 world)
    {
        Vector3Int cell = instance.grid.WorldToCell(world);
        return instance.grid.CellToWorld(cell) + new Vector3(0.5f,0, 0.5f);
    }

    public static Vector3 CellPosition(Vector3Int cell)
    {
        return instance.grid.CellToWorld(cell) + new Vector3(0.5f, 0, 0.5f);
    }
}
