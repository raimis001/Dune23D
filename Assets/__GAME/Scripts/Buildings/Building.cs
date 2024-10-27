using UnityEngine;

public class Building : Proc
{
    [SerializeField]
    BuildKind kind;

    [SerializeField]
    GameObject normal;
    [SerializeField]
    GameObject wrong;

    public bool placed = false;

    ConstructionData data;

    protected virtual void Start()
    {
        data = Build.construction(kind);
        size = data.size;

        if (placed)
            Build.AddBuilding(transform.position, kind);
    }

    public void SetPlaceType(bool isNormal)
    {
        normal.SetActive(isNormal);
        wrong.SetActive(!isNormal);
    }
}
