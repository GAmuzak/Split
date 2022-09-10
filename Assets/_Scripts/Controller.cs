using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected float animationTime;
    [SerializeField] protected LeanTweenType easeCurve;
    protected static bool canMove = true;
        
    protected List<Vector3> validDirections=new();

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

    void Update()
    {
        
    }

    protected virtual void Movement(Vector3 moveDirn)
    {
        if (!canMove) {return;}
        canMove = false;
        if (moveDirn.x > 0 && moveDirn.z > 0) moveDirn = new Vector3(moveDirn.x, 0, 0);
        if (validDirections.Contains(moveDirn));
            LeanTween.move(gameObject, transform.position + 2*moveDirn, animationTime).setEase(easeCurve);
        StartCoroutine(moveCooldown());
    }

    private void OnNewTileEntered()
    {
        Ray belowObject = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(belowObject, out RaycastHit raycastHit, Mathf.Infinity))
        {
            Transform tileUnderObject = raycastHit.transform;
            Debug.Log(tileUnderObject.gameObject.name);
            if (tileUnderObject.CompareTag("Tile"))
            {
                
                validDirections = tileUnderObject.GetComponent<Tile>().GetValidDirections();
            }
        }
    }
    
    protected IEnumerator moveCooldown()
    {
        Debug.Log("Started Coroutine");
        yield return new WaitForSeconds(animationTime);
        OnNewTileEntered();
        canMove = true;
        Debug.Log("Stopped Coroutine");
        yield return null;
    }
    
}
