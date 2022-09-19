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
    protected event Action OnTileEntered;
    [SerializeField] protected List<LayerMask> interactable;

    [SerializeField] private TileType tileType;
    private List<Vector3> validDirections = new List<Vector3>();
    private Vector3[] checklist;
    private float destroyTime = 0.3f;
    [SerializeField] protected static readonly Dictionary<Directions, Vector3> directionChecklist = new Dictionary<Directions, Vector3>();

    protected void Awake()
    {
        checklist = new[] { transform.forward, -transform.forward, -transform.right, transform.right };
        directionChecklist.Add(Directions.Up, transform.forward);
        directionChecklist.Add(Directions.Down, - transform.forward);
        directionChecklist.Add(Directions.Left, -transform.right);
        directionChecklist.Add(Directions.Right, transform.right);

    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
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


