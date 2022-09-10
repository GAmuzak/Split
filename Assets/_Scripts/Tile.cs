using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Vector3> validDirections = new List<Vector3>();
    private Vector3[] checklist;
    private float destroyTime = 0.3f;

    private void Start()
    {
        checklist = new[] { transform.forward, -transform.forward, -transform.right, transform.right };
        Debug.Log(checklist);
    }

    public List<Vector3> GetValidDirections()
    {
        Debug.Log(validDirections);
        validDirections.Clear();
        foreach (Vector3 direction in checklist)
        {
            Ray ray = new(transform.position,direction);
            if (Physics.Raycast(ray, out RaycastHit _, 2))
            {
                validDirections.Add(direction);
            }
        }
        return validDirections;
    }

    public void DestroyNeighbouringTile(float offset)
    {
        LeanTween.scale(gameObject, Vector3.zero, destroyTime+offset/10f).setEase(LeanTweenType.easeInQuad);
    }

    // private IEnumerator WaitForSquish()
    // {
    //     yield return new WaitForSeconds(destroyTime);
    //     Destroy(gameObject);
    // }
}
