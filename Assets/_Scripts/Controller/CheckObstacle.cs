using UnityEngine;

public class CheckObstacle : MonoBehaviour
{
    private void Awake()
    {
        InputHandler.MovementDirection += Check;
    }

    private void Check(Vector3 direction)
    {
        
    }
}
