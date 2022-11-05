using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [HideInInspector] public List<DoorController> doorControllers;
    
    private readonly float _pressedScale = 50f;
    private readonly float _originalScale = 150f;
    private GameObject _button;
    private bool _allOpen;
    private int _closedDoors;

    private void Awake()
    {
        doorControllers = new List<DoorController>();
        _button = transform.GetChild(1).gameObject;
    }

    public void CheckAllClosed()
    {
        foreach (var doorController in doorControllers)
        {
            if (!doorController.tempOpen)
                _closedDoors++;
        }
        
        if (_closedDoors >= doorControllers.Count)
            LeanTween.scaleZ(_button, _originalScale, .1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_allOpen) return;
        foreach (var doorController in doorControllers)
        {
            if (doorController.tempOpen) return;
        }

        if (!other.CompareTag("Player") || other.CompareTag("Clone")) return;
        LeanTween.scaleZ(_button, _pressedScale, .1f);

        _closedDoors = 0;

        foreach (var doorController in doorControllers)
            doorController.OpenDoor(this);
    }
}
