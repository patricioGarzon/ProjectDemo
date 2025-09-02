using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : Interactable
{
    bool InUse = false;

    public Transform HandleLocation;
    protected override void Interacted()
    {
        //check if player is the one who interacted. 
        if (curPlayer != null) {
            //assign temporary object to use
            curPlayer.OnPickupWeapon(this);
            //Set InUse
            InUse = true;
        }
    }

}
