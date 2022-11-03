using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForObstacles : MonoBehaviour
{
    [SerializeField] private LayerMask obstacle;
    [SerializeField] private float maxDistance;
    
    private Controller _controller;
    private Transform _transform;
    private List<Ray> _rays;
    
    private void Start()
    {
        _controller = GetComponent<Controller>();
        _transform = transform;
    }

    private void Awake()
    {
        _rays = new List<Ray>();
        InputHandler.MovementDirection += Move;
    }

    private void OnDisable()
    {
        InputHandler.MovementDirection -= Move;
    }

    private void Move(Vector3 direction)
    {
        StartCoroutine(AnimationCooldown());
    }

    private void GenerateRays()
    {
        _rays.Clear();
        
        var position = _transform.position;
        var forward = _transform.forward;
        var rayForward = new Ray(position, forward);
        var rayBack = new Ray(position, -forward);
        
        var right = _transform.right;
        var rayRight = new Ray(position, right);
        var rayLeft = new Ray(position, -right);
        
        _rays.Add(rayForward);
        _rays.Add(rayBack);
        _rays.Add(rayRight);
        _rays.Add(rayLeft);
    }

    private IEnumerator AnimationCooldown()
    {
        yield return new WaitForSeconds(.25f);

        GenerateRays();
        
        foreach (var ray in _rays)
        {
            if (Physics.Raycast(ray, maxDistance, obstacle))
            {
                foreach (var validDirection in _controller.validDirections)
                {
                    if (validDirection == ray.direction)
                    {
                        _controller.validDirections.Remove(validDirection);
                        yield break;
                    }
                }
            }
        }
    }
}
