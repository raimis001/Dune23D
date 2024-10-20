using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    static Transform parent;

    static List<GameObject> pool = new();
    static Dictionary<string, GameObject> prefabs = new();

    private void Start()
    {
        parent = transform;

        for (int i = 0; i < transform.childCount; i++) 
        { 
            Transform child = transform.GetChild(i);
            prefabs.Add(child.gameObject.name, child.gameObject);
            pool.Add(child.gameObject);
            child.gameObject.SetActive(false);
            Debug.Log(child.gameObject.name);
        }
    }

    public static GameObject Create(string poolName)
    {
        
        foreach (GameObject obj in pool)
        {
            if (obj.name != poolName)
                continue;
            if (obj.activeInHierarchy)
                continue;

            obj.SetActive(true);
            return obj;
        }

        if (!prefabs.TryGetValue(poolName, out GameObject prefab))
            return null;

        GameObject o = Instantiate(prefab, parent);
        o.name = poolName;
        o.SetActive(true);

        pool.Add(o);

        return o;
    }
    
    public static GameObject Create(string poolName, Vector3 position, Quaternion rotation )
    {
        GameObject obj = Create(poolName);
        if (obj == null)    
            return null;

        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public static GameObject Create(string poolName, Transform point)
    {
        return Create(poolName, point.position, point.rotation);
    }

}
