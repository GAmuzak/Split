using System;
using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<Directions, GameObject> validTiles = new();
    private Vector3[] checklist;
    private float destroyTime = 0.3f;
    protected readonly Dictionary<Directions, Vector3> directionChecklist = new();

    protected void Awake()
    {
        checklist = new[] { transform.forward, -transform.forward, -transform.right, transform.right };
        directionChecklist.Add(Directions.Up, transform.forward);
        directionChecklist.Add(Directions.Down, -transform.forward);
        directionChecklist.Add(Directions.Right, transform.right);
        directionChecklist.Add(Directions.Left, -transform.right);

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

    public Dictionary<Directions, GameObject> GetValidDirections()
    {
        validTiles.Clear();
        foreach (Directions dir in directionChecklist.Keys)
        {
            Ray ray = new(transform.position, directionChecklist[dir]);
            if (Physics.Raycast(ray, out RaycastHit tile, 2))
            {
                validTiles.Add(dir, tile.transform.gameObject);
            }
            else
            {
                validTiles.Add(dir, null);
            }
        }
        return validTiles;
    }

    public virtual void TileAction(GameObject controllerObject)
    {
        
    }

    public void DestroyNeighbouringTile(float offset)
    {
        LeanTween.scale(gameObject, Vector3.zero, destroyTime+offset/10f).setEase(LeanTweenType.easeInQuad);
    }
}

public static class ConversionMapping
{
    public static readonly Dictionary<Vector3, Directions> inputToDirection = new Dictionary<Vector3, Directions>()
    {
        { Vector3.forward, Directions.Up },
        { Vector3.back, Directions.Down }, 
        { Vector3.right, Directions.Right },
        { Vector3.left, Directions.Left }
    };
    
    public static readonly Dictionary<Directions, Vector3> directionToInput = new Dictionary<Directions, Vector3>()
    {
        { Directions.Up,    Vector3.forward},
        { Directions.Down,  Vector3.back }, 
        { Directions.Right, Vector3.right},
        { Directions.Left,  Vector3.left}
    }; 
}


