using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public int index;
    
    [HideInInspector] public DoorController doorController;

    private readonly float _originalScale = 150f;
    private readonly float _pressedScale = 50f;
    private bool _buttonActivated;
    private GameObject _button;

    private void Start()
    {
        _button = transform.GetChild(1).gameObject;
    }

    private void Awake()
    {
        InputHandler.MovementDirection += Move;
    }

    private void OnDisable()
    {
        InputHandler.MovementDirection -= Move;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || doorController.opened) return;
        LeanTween.scaleZ(_button, _pressedScale, .1f);
        
        doorController.OpenDoor();
        
        _buttonActivated = true;
        doorController.AssignIndicesToButtons();
        doorController.doorCounterVisualizer.SetVisualizer(index);
    }

    private void Move(Vector3 direction)
    {
        if (!_buttonActivated) return;
        if (index > 1)
        {
            index--;
            doorController.doorCounterVisualizer.CountDown();
        }
        else
        {
            if (doorController.opened) return;
            LeanTween.scaleZ(_button, _originalScale, .1f);
            _buttonActivated = false;
            doorController.CloseDoor();
        }
    }
}
