using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : Controller
{
    [SerializeField] private Disolver disolver;
    
    private bool canDestroy;
    private Vector3 mirrorLine;

    private void Start()
    {
        
        StartCoroutine(WaitBeforeMerge());
    }
    
    protected override void Movement(Vector3 dirn)
    {
        Vector3 targetMove = Vector3.Scale(dirn, mirrorLine);
        base.Movement(targetMove);
    }

    public void Spawn(Vector3 parentPosition, Vector3 moveDirn)
    {
        LeanTween.move(gameObject, parentPosition + stepSize*moveDirn, animationTime).setEase(easeCurve);
        disolver.Dissolve(0, animationTime);
        mirrorLine = moveDirn.x != 0 ? new Vector3(-1,0,1) :  new Vector3(1,0,-1);
        OnNewTileEntered();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!canDestroy) return;
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitBeforeMerge()
    {
        yield return new WaitForSeconds(0.2f);
        canDestroy = true;
    }
    
    
}
