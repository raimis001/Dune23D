using UnityEngine;

public class Building : Proc
{
    [SerializeField]
    BuildKind kind;

    [SerializeField]
    int energy = 0;

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

    protected virtual void OnEnable()
    {
        if (energy > 0)
            Energy.Produce += energy;
        else
            Energy.Using += Mathf.Abs(energy);
    }

    public void SetPlaceType(bool isNormal)
    {
        normal.SetActive(isNormal);
        wrong.SetActive(!isNormal);
    }
}
