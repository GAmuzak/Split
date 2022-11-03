using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool opened;
    
    [HideInInspector] public bool tempOpen;
    [HideInInspector] public DoorCounterVisualizer doorCounterVisualizer;

    [SerializeField] private ButtonIntDictionary buttonDictionary;
    
    private List<ButtonBehaviour> _buttons;
    private List<int> _values;
    private Animator _doorAnimator;
    private int _index;
    private bool _buttonActivated;
    private int _activatedButton;
    
    private void Start()
    {
        ConvertDictionaryToList();
        doorCounterVisualizer = GetComponent<DoorCounterVisualizer>();
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
        if (!other.CompareTag("Player") || opened) return;
        opened = true;
        OpenDoor();
        doorCounterVisualizer.ResetTiles();
    }

    private void Move(Vector3 obj)
    {
        if (!_buttonActivated) return;
        if (opened) return;
        if (_index > 1)
        {
            _index--;
        }
        else
        {
            CloseDoor();
            _buttons[_activatedButton].CheckAllClosed();
            _buttonActivated = false;
        }
    }

    private void OpenDoor()
    {
        if (tempOpen) return;
        _doorAnimator.Play("Door Open Static", -1, 0f);
    }

    public void OpenDoor(ButtonBehaviour button)
    {
        if (tempOpen) return;
        _buttonActivated = true;
        for (int i = 0; i < _buttons.Count; i++)
            if (_buttons[i] == button)
            {
                _activatedButton = i;
                _index = _values[i];
            }
        
        _doorAnimator.Play("Door Opening", -1, 0f);
        tempOpen = true;
    }
    
    private void CloseDoor()
    {
        _doorAnimator.Play("Door Closing", -1, 0f);
        doorCounterVisualizer.ResetTiles();
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
