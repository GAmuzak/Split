using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum TileType
{
    Belt = 0,
    Replicate = 1,
    Teleport = 2,
    Wall = 3,
    Fire = 4,
    Flood = 5,
    Plain = 6
}

public class Tile : MonoBehaviour
{
    [SerializeField] protected List<LayerMask> interactable;
    [SerializeField] private TileType tileType;
    
    protected event Action OnTileEntered;
    protected readonly Dictionary<Directions, Vector3> directionChecklist = new Dictionary<Directions, Vector3>();
    
    private List<Vector3> validDirections = new List<Vector3>();
    private float destroyTime = 0.3f;

    protected void Awake()
    {
        directionChecklist.Add(Directions.Up, transform.forward);
        directionChecklist.Add(Directions.Down, -transform.forward);
        directionChecklist.Add(Directions.Left, -transform.right);
        directionChecklist.Add(Directions.Right, transform.right);
    }

    protected virtual void AnimateTile()
    {
        
    }

    public List<Vector3> GetValidDirections()
    {
        validDirections.Clear();
        foreach (Vector3 direction in directionChecklist.Values)
        {
            Ray ray = new(transform.position,direction);
            if (Physics.Raycast(ray, out RaycastHit _, 2))
            {
                validDirections.Add(direction);
            }
        }
        return validDirections;
    }

    public virtual void TileAction(GameObject controllerObject)
    {
        
    }

    public void DestroyNeighbouringTile(float offset)
    {
        LeanTween.scale(gameObject, Vector3.zero, destroyTime+offset/10f).setEase(LeanTweenType.easeInQuad);
    }
}


