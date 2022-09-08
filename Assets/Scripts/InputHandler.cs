using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector3> MovementDirection;
    public static event Action<Vector3> CloneCreator;
    public static event Action ClonePreview;

    private bool canMove=true;
    private bool canClone = true;
    private bool cloningOngoing;
    // private Vector3 movementInputCorrected;


    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        canMove = false;
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        if (movementInput.x > 0 && movementInput.y > 0) movementInput = new Vector2(movementInput.x, 0);
        Vector3 movementInputCasted = new(movementInput.x, 0, movementInput.y);
        MovementDirection?.Invoke(movementInputCasted);
        StartCoroutine(moveCooldown());
        
        if(cloningOngoing) CloneCreator?.Invoke(movementInputCasted);
        cloningOngoing = false;
        StartCoroutine(cloneCooldown());
    }

    public void onCloneInput(InputAction.CallbackContext ctx)
    {
        if (!canClone  || cloningOngoing) return;
        cloningOngoing = true;
        canClone = false;
        ClonePreview?.Invoke();
    }

    private IEnumerator moveCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }

    private IEnumerator cloneCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canClone = true;
    }
}
