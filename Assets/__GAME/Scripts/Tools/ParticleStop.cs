using UnityEngine;

public class ParticleStop : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    private void OnParticleSystemStopped()
    {
        parent.SetActive(false);
    }
}
