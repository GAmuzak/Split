using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [SerializeField] private GameObject clone;
    [SerializeField] private Transform cloneContainer;
    [SerializeField] private GameObject preview;
    [SerializeField] private Transform previewCloneContainer;
    [SerializeField] private float cloneCoolDown = 0.2f;
    private bool canClone = true;
    private bool cloningOnGoing = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputHandler.CloneCreator += CreateClone;
        InputHandler.ClonePreview += PreviewValidMoves;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputHandler.CloneCreator -= CreateClone;
        InputHandler.ClonePreview -= PreviewValidMoves;
    }

    protected override void Movement(Vector3 moveDirn)
    {
        if (cloningOnGoing)
        {
            CreateClone(moveDirn);
            cloningOnGoing = false;
            StartCoroutine(cloneCooldown());
        }
        else
        {
            base.Movement(moveDirn);
        }

    }

    
    private void PreviewValidMoves()
    {
        if (!canClone  || cloningOnGoing) return;
        cloningOnGoing = true;
        canClone = false;
        foreach (Vector3 validDirection in validDirections)
        {
            if (!validDirections.Contains(-validDirection)) continue;
            GameObject previewClone = Instantiate(preview, transform.position, Quaternion.identity,previewCloneContainer);
            LeanTween.move(previewClone, transform.position + 2 * validDirection, animationTime).setEase(easeCurve);
        }
    }

    private void CreateClone(Vector3 movementDirn)
    {
        foreach (Transform child in previewCloneContainer)
        {
            Destroy(child.gameObject);
        }
        if (!validDirections.Contains(-1 * movementDirn) || !validDirections.Contains(movementDirn)) return;

        GameObject newClone = Instantiate(clone, transform.position, Quaternion.identity, cloneContainer);
        newClone.GetComponent<CloneController>().Spawn(transform.position, -movementDirn);
    }
    
    private IEnumerator cloneCooldown()
    {
        yield return new WaitForSeconds(cloneCoolDown);
        canClone = true;
    }
}
