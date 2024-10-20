using System.Collections;
using UnityEngine;

public class BuildingRefinery : Building
{
    public Transform harvesterPoint;


    public void Unload(UnitHarvester harvester)
    {
        StartCoroutine(IUnload(harvester));
    }

    IEnumerator IUnload(UnitHarvester harvester)
    {
        while (harvester.storage > 0)
        {
            float r = Random.Range(2f, 3f); 
            yield return new WaitForSeconds(r);
            harvester.storage -= Random.Range(0.05f, 0.1f);
        }
        harvester.storage = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center, 0.25f);
    }
}
