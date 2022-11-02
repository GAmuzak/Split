using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool opened;
    
    [HideInInspector] public DoorCounterVisualizer doorCounterVisualizer;

    [SerializeField] private ButtonIntDictionary buttonDictionary;
    [SerializeField] private Tile tile;
    
    private List<ButtonBehaviour> _buttons;
    private List<int> _values;
    private Animator _doorAnimator;
    
    private void Start()
    {
        AssignIndicesToButtons();
        doorCounterVisualizer = GetComponent<DoorCounterVisualizer>();
        _doorAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        opened = true;
        doorCounterVisualizer.ResetTiles();
    }

    public void OpenDoor()
    {
        _doorAnimator.Play("Door Opening", -1, 0f);
        tile.tileType = TileType.Belt;
    }
    
    public void CloseDoor()
    {
        _doorAnimator.Play("Door Closing", -1, 0f);
        tile.tileType = TileType.Wall;
        doorCounterVisualizer.ResetTiles();
    }

    public void AssignIndicesToButtons()
    {
        _buttons = new List<ButtonBehaviour>(buttonDictionary.Keys);
        _values = new List<int>(buttonDictionary.Values);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].index = _values[i];
            _buttons[i].doorController = this;
        }
    }
}
