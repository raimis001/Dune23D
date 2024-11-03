using UnityEngine;

public class BuildingGenerator : Building
{
    public float currentEnergy => Mathf.Abs(energy) * (1 - currentDamage);
}
