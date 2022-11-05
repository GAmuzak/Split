using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector3> MovementDirection;
    public static event Action<Vector3> CloneCreator;
    public static event Action ClonePreview;
    
    public void OnMovementInput(InputAction.CallbackContext input)
    {
        Debug.Log("OnMovemementInput");
        Vector2 movementInput = input.ReadValue<Vector2>();
        Vector3 movementInputCasted = new Vector3(movementInput.x, 0, movementInput.y);
        MovementDirection?.Invoke(movementInputCasted);
    }

    public void onCloneInput(InputAction.CallbackContext ctx)
    {
        ClonePreview?.Invoke();
    }
}
    
    
