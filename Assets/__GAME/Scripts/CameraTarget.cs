using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    float panSpeed = 5;

    [SerializeField]
    InputAction moveAction;
    [SerializeField]
    InputAction mouseAction;

    bool isPanning;
    Vector3 lastMousePosition;

    private void OnEnable()
    {
        moveAction.Enable();

        mouseAction.started += OnMousePressed;
        mouseAction.performed += OnMouseHeld;
        mouseAction.canceled += OnMouseReleased;

        mouseAction.Enable();
    }

    private void OnMouseReleased(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse release");
        //isPanning = false;
    }

    private void OnMouseHeld(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse hold");
    }

    private void OnMousePressed(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse press");
        //isPanning = Device.MouseWorld(out lastMousePosition);

    }

    private void Update()
    {
        if (moveAction.IsPressed())
        {
            Vector2 keyboard = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(keyboard.x, 0, keyboard.y) * Time.deltaTime * speed;

            transform.Translate(move);
        }

        if (Device.MouseLeftDown)
        {
            isPanning = Device.MouseWorld(out lastMousePosition);
            return;
        }

        if (Device.MouseLeftUp)
        {
            isPanning = false;
            return;
        }

        if (isPanning)
        {
            if (!Device.MouseWorld(out Vector3 mousePosition))
            {
                isPanning = false;
                return;
            }

            Vector3 delta = (lastMousePosition - mousePosition) ;
            delta.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + delta, Time.deltaTime * panSpeed);

            //lastMousePosition = mousePosition;
        }

    }
}
