using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private LeanTweenType easeCurve;
    
    private bool canDestroy;
    private int mirrorLine;
    private List<Vector3> validDirections=new();

    
    private void Start()
    {
        StartCoroutine(WaitBeforeMerge());
    }

    private void OnEnable()
    {
        InputHandler.MovementDirection += Movement;
    }

    private void OnDisable()
    {
        InputHandler.MovementDirection -= Movement;
    }

    private void Movement(Vector3 dirn)
    {
        
        if(mirrorLine==0)
        {
            Vector3 targetMove = new(-1 * dirn.x,0, dirn.z);
            if (!validDirections.Contains(targetMove)) return;
            LeanTween.move(gameObject, transform.position + 2*targetMove, time).setEase(easeCurve);
        }
        else
        {
            Vector3 targetMove = new(dirn.x,0, -1*dirn.z);
            if (!validDirections.Contains(targetMove)) return;
            LeanTween.move(gameObject, transform.position + 2*targetMove, time).setEase(easeCurve);
        }
    }

    public void Spawn(Vector3 parentPosition, Vector3 moveDirn)
    {
        LeanTween.move(gameObject, parentPosition + 2*moveDirn, time).setEase(easeCurve);
        mirrorLine = moveDirn.x != 0 ? 0 : 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            validDirections=other.gameObject.GetComponent<Tile>().GetValidDirections();
            foreach (Vector3 validDirection in validDirections)
            {
                Debug.Log(validDirection);
            }
            
        }
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
