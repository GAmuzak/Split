using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected float animationTime;
    [SerializeField] protected LeanTweenType easeCurve;
    [SerializeField] protected float stepSize = 2f;

    protected List<Vector3> validDirections=new();
    
    private bool canMove = true;

    private bool movementPerfored;

    private Vector3 _inputBufferVector;

    private void Start()
    {
        stepSize *= transform.localScale.x; //slightly hacky, but unlikely situation in the first place
        OnNewTileEntered();
    }

    protected virtual void OnEnable()
    {
        InputHandler.MovementDirection += Movement;
    }

    protected virtual void OnDisable()
    {
        InputHandler.MovementDirection -= Movement;
    }

    protected virtual void Movement(Vector3 moveDirn)
    {
        if (!canMove) return;
        if (!movementPerfored)
        {
            movementPerfored = true;
            if (moveDirn.x > 0 && moveDirn.z > 0) moveDirn = new Vector3(moveDirn.x, 0, 0);
            if (validDirections.Contains(moveDirn))
            {
                LeanTween.move(gameObject, transform.position + stepSize * moveDirn, animationTime)
                    .setEase(easeCurve);
                StartCoroutine(moveCooldown());
                _inputBufferVector = Vector3.zero;
            }
            else
            {
                movementPerfored = false;
                canMove = true; //so we don't have cooldown for incorrect moves
            }
        }
        else
        {
            _inputBufferVector = moveDirn;
        }
    }
    
    protected void OnNewTileEntered()
    {
        Ray belowObject = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(belowObject, out RaycastHit raycastHit, Mathf.Infinity))
        {
            if (raycastHit.transform.gameObject.TryGetComponent<Tile>(out Tile tileUnderObject))
            {
                validDirections = tileUnderObject.GetValidDirections();
                tileUnderObject.TileAction(gameObject);
            }
        }
    }
    
    protected IEnumerator moveCooldown()
    {
        yield return new WaitForSeconds(animationTime);
        canMove = true;
        movementPerfored = false;
        OnNewTileEntered();

        if (_inputBufferVector != Vector3.zero)
        {
            Movement(_inputBufferVector);
        }
    }

    public virtual void DestroyAnimation()
    {
        //fill code
    }
    
    
    
}
