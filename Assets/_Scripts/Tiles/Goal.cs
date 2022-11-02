using UnityEngine;
using System;
using System.Collections;


public class Goal : MonoBehaviour
{
    public static event Action EndLevel;
    
    [SerializeField] private Transform MapContainer;
    
    private float destroyTime = 0.3f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        BeginDestructionOfMap();
        StartCoroutine(WaitTillLevelDestroyed());
        EndLevel?.Invoke();
    }

    private IEnumerator WaitTillLevelDestroyed()
    {
        yield return new WaitForSeconds(0.6f);
        EndLevel?.Invoke();
    }

    private void BeginDestructionOfMap()
    {
        LeanTween.scale(gameObject, Vector3.zero, destroyTime).setEase(LeanTweenType.easeInQuad);
        foreach (Transform tile in MapContainer)
        {
            GameObject currentTile = tile.gameObject;
            currentTile.GetComponent<Tile>().DestroyNeighbouringTile(Vector3.Distance(transform.position,currentTile.transform.position));
        }
    }
}
