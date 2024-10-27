using UnityEngine;

public enum RacesKind
{
    Atreides, Harkonen, Ordos, Imperial, Freemen
}

public class Proc : MonoBehaviour
{
    public RacesKind race;
    [Space]

    protected Vector2 size = Vector2.one;

    public Vector3 center => transform.position + new Vector3(0.5f * size.x, 0, 0.5f * size.y);

    public void Damage(float damage)
    {

    }

    //Kind - 0 any, 1 enemy, 2 friends
    protected bool FindClosestObject<T>(float searchRadius, out T closestObject, int kind = 0) where T : Proc
    {
        closestObject = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        if (colliders.Length == 0)
            return false;

        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            T find = collider.GetComponentInParent<T>();
            if (!find)
                continue;

            if (kind == 2 && find.race != race)
                continue;
            if (kind == 1 && find.race == race)
                continue;

            float d = Vector3.Distance(center, find.center);
            if (d < closestDistance)
            {
                closestDistance = d;
                closestObject = find;
            }

        }
        return closestObject != null;
    }


}
