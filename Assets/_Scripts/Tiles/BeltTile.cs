using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltTile : Tile
{
    [SerializeField] private Directions beltDirection;


    private void Update()
    {
        AnimateTile();
    }

    protected override void AnimateTile()
    {
        
    }

    public override void TileAction(GameObject controllerObject)
    {
        Controller controller = controllerObject.GetComponent<Controller>();
        Vector3 movementDirn = ConversionMapping.directionToInput[beltDirection];
        controller.Movement(movementDirn);
    }
}
