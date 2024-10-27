using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public static bool OwerUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
