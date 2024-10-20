using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Battle : MonoBehaviour
{
    [SerializeField]
    Transform cursor;

    [SerializeField]
    NavMeshAgent currentUnit;



    // Update is called once per frame
    void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;
        Debug.Log("Mouse press: ");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (cursor)
                cursor.position = hit.point;

            if (currentUnit)
            {
                //currentUnit.destination = hit.point;
            }
        }
    }
}
