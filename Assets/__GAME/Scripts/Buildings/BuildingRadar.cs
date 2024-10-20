using UnityEngine;

public class BuildingRadar : Building
{
    [SerializeField]
    Transform dish;
    [SerializeField]
    float dishSpeed;


    private void Update()
    {
        dish.Rotate(Vector3.up, Time.deltaTime * dishSpeed);
    }
}
