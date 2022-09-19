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
        LeanTween.move(controllerObject, transform.position + 2 * directionChecklist[beltDirection], 0.2f);
    }
}
