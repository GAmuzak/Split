using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private LeanTweenType easeCurve;
    [SerializeField] private GameObject clone;
    [SerializeField] private Transform cloneContainer;
    [SerializeField] private GameObject preview;
    [SerializeField] private Transform previewCloneContainer;

    private List<Vector3> validDirections=new();

    private void OnEnable()
    {
        InputHandler.MovementDirection += Movement;
        InputHandler.CloneCreator += CreateClone;
        InputHandler.ClonePreview += PreviewValidMoves;
    }

    private void OnDisable()
    {
        InputHandler.MovementDirection -= Movement;
        InputHandler.CloneCreator -= CreateClone;
        InputHandler.ClonePreview -= PreviewValidMoves;
    }

    private void Movement(Vector3 moveDirn)
    {
        if (!validDirections.Contains(moveDirn)) return;
            LeanTween.move(gameObject, transform.position + 2*moveDirn, time).setEase(easeCurve);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            validDirections=other.gameObject.GetComponent<Tile>().GetValidDirections();
        }
    }

    private void PreviewValidMoves()
    {
        foreach (Vector3 validDirection in validDirections)
        {
            if (!validDirections.Contains(-1 * validDirection)) continue;
            GameObject previewClone=Instantiate(preview, transform.position, Quaternion.identity,previewCloneContainer);
            LeanTween.move(previewClone, transform.position + 2 * validDirection, time).setEase(easeCurve);
        }
    }

    private void CreateClone(Vector3 movementDirn)
    {
        foreach (Transform child in previewCloneContainer)
        {
            Destroy(child.gameObject);
        }
        if (!validDirections.Contains(-1 * movementDirn) || !validDirections.Contains(movementDirn)) return;

        GameObject newClone=Instantiate(clone, transform.position, Quaternion.identity, cloneContainer);
        Movement(movementDirn);
        newClone.GetComponent<CloneController>().Spawn(transform.position, -movementDirn);
    }
}
