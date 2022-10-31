using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected float animationTime;
    [SerializeField] protected LeanTweenType easeCurve;
    [SerializeField] protected static Vector3 placementOffset = new Vector3(0, 1, 0);
    protected bool canMove = true;
    protected Dictionary<Directions, GameObject> validTiles = new();


    protected virtual void OnEnable()
    {
        InputHandler.MovementDirection += Movement;
    }
    
    protected virtual void OnDisable()
    {
        InputHandler.MovementDirection -= Movement;
    }

    void Start()
    {
        OnNewTileEntered();
    }
    

    public virtual void Movement(Vector3 moveDirn)
    {
        if (!canMove || moveDirn == Vector3.zero) {return;}
        canMove = false;
        if (moveDirn.x != 0 && moveDirn.z != 0) moveDirn = new Vector3(moveDirn.x, 0, 0);
        if (validTiles[ConversionMapping.inputToDirection[moveDirn]] != null)
        {
            Directions targetDirection = ConversionMapping.inputToDirection[moveDirn];
            Vector3 targetPosition = validTiles[targetDirection].transform.position;
            LeanTween.move(gameObject, targetPosition + placementOffset , animationTime).setEase(easeCurve);
        }

        StartCoroutine(moveCooldown());
    }

    private void OnNewTileEntered()
    {
        Ray belowObject = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(belowObject, out RaycastHit raycastHit, Mathf.Infinity))
        {
            GameObject tileUnderObject = raycastHit.transform.gameObject;
            if (tileUnderObject.TryGetComponent(out Tile tile))
            {
                validTiles = tile.GetValidDirections();
                tile.TileAction(gameObject);
            }
        }
    }
    
    protected IEnumerator moveCooldown()
    {
        yield return new WaitForSeconds(animationTime);
        canMove = true;
        OnNewTileEntered();
    }

    public virtual void DestroyAnimation()
    {
        //fill code
    }
    
    
    
}
