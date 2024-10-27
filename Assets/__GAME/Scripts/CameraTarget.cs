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
        isPanning = false;
    }

    private void OnMouseHeld(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse hold");
    }

    private void OnMousePressed(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse press");
        isPanning = true;
        lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        if (moveAction.IsPressed())
        {
            Vector2 keyboard = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(keyboard.x, 0, keyboard.y) * Time.deltaTime * speed;

            transform.Translate(move);
        }

        if (isPanning)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 delta = (mousePosition - lastMousePosition) * Time.deltaTime * panSpeed;

            transform.Translate(-delta.x, 0, -delta.y);

            lastMousePosition = mousePosition;
        }

    }
}
