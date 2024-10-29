using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Device : MonoBehaviour
{
    public static bool MouseLeftDown => Mouse.current.leftButton.wasPressedThisFrame;
    public static bool MouseLeftUp => Mouse.current.leftButton.wasReleasedThisFrame;

    public static bool MouseRightDown => Mouse.current.rightButton.wasPressedThisFrame;
    public static bool MouseRightUp => Mouse.current.rightButton.wasReleasedThisFrame;

    
    public static Vector2 MousePosition => Mouse.current.position.ReadValue();

    public static bool MouseWorld(out Vector3 position)
    {
        position = Vector3.zero;
        if (OwerUI)
            return false;

        Ray ray = Camera.main.ScreenPointToRay(MousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, instance.groundLayer);
        position = hit.point;

        return true;
    }

    public static bool OwerUI => EventSystem.current.IsPointerOverGameObject();

    static Device instance;

    [SerializeField]
    LayerMask groundLayer;

    private void Awake()
    {
        instance = this;
    }

}
