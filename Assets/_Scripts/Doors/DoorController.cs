using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool opened;
    
    [HideInInspector] public bool tempOpen;

    [SerializeField] private ButtonIntDictionary buttonDictionary;
    [SerializeField] private LayerMask player;
    [SerializeField] private float raycastDistance;
    [SerializeField] private int index;
    
    private DoorCounterVisualizer _doorCounterVisualizer;
    private List<ButtonBehaviour> _buttons;
    private List<int> _values;
    private Animator _doorAnimator;
    private bool _buttonActivated;
    private int _activatedButton;
    private Ray _ray;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _ray = new Ray(_transform.position + Vector3.up, _transform.right);
        ConvertDictionaryToList();
        _doorCounterVisualizer = GetComponent<DoorCounterVisualizer>();
        _doorAnimator = transform.GetChild(0).GetComponent<Animator>();
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
        // if (!other.CompareTag("Player") || opened) return;
        // opened = true;
        // OpenDoor();
        // _doorCounterVisualizer.ResetTiles();
        // transform.GetComponent<Collider>().enabled = false;
    }

    private void Move(Vector3 obj)
    {
        if (!_buttonActivated) return;
        if (opened) return;
        if (index > 1)
        {
            index--;
            _doorCounterVisualizer.CountDown();

            // StartCoroutine(CheckForPlayerInTile());
        }
        else
            Deactivate();
    }

    private IEnumerator CheckForPlayerInTile()
    {
        yield return new WaitForSeconds(.3f);
        if (Physics.Raycast(_ray, raycastDistance, player))
        {
            if (index > 2)
            {
                Deactivate();
                index = 0;
            }
        }
    }

    private void Deactivate()
    {
        CloseDoor();
        _buttons[_activatedButton].CheckAllClosed();
        _buttonActivated = false;
    }

    private void OpenDoor()
    {
        if (tempOpen) return;
        _doorAnimator.Play("Door Open Static", -1, 0f);
    }

    public void OpenDoor(ButtonBehaviour button)
    {
        if (opened) return;
        if (tempOpen) return;
        _buttonActivated = true;
        transform.GetComponent<Collider>().enabled = false;
        for (int i = 0; i < _buttons.Count; i++)
            if (_buttons[i] == button)
            {
                _activatedButton = i;
                index = _values[i];
            }
        _doorCounterVisualizer.SetVisualizer(index);
        
        _doorAnimator.Play("Door Opening", -1, 0f);
        tempOpen = true;
    }
    
    private void CloseDoor()
    {
        _doorAnimator.Play("Door Closing", -1, 0f);
        _doorCounterVisualizer.ResetTiles();
        transform.GetComponent<Collider>().enabled = true;
        tempOpen = false;
    }

    private void ConvertDictionaryToList()
    {
        _buttons = new List<ButtonBehaviour>(buttonDictionary.Keys);
        _values = new List<int>(buttonDictionary.Values);

        foreach (var button in _buttons)
            button.doorControllers.Add(this);
    }
}
