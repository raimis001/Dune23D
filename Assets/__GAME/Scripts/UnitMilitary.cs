using UnityEngine;

public class UnitMilitary : Unit
{
    [SerializeField]
    Transform targetPoint;


    protected override void Update()
    {
        base.Update();
        if (status == 0)
        {
            MoveToPosition(targetPoint.position);
            status = 1;
        }
    }
}
