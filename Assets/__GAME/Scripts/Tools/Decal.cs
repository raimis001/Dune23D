using UnityEngine;

public class Decal : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(StopDecal), 5);
    }

    void StopDecal()
    {
        gameObject.SetActive(false);
    }
}
