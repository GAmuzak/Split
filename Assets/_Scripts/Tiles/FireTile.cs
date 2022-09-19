using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTile : Tile
{
    private void Update()
    {
        AnimateTile();
    }

    protected override void AnimateTile()
    {
        
    }
    public override void TileAction(GameObject controllerObject)
    {
        controllerObject.GetComponent<Controller>().DestroyAnimation();
        Destroy(controllerObject);
    }
    
}
