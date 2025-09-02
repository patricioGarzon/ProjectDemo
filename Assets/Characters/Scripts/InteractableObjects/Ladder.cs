using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public Transform exitTopPoint;
    public Transform exitBottomPoint;

    public void OnTouchInteract(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            curPlayer = player;
            movement.curLader = this;
            //set climbing only upwards and apply animation
            movement.setClimbing = true;            
        }
    }
    public void OnExitLadder()
    {
        if (curPlayer)
        {
            PlayerMovement movement = curPlayer.GetComponent<PlayerMovement>();
            if (movement)
            {
                movement.ExitLadder(exitTopPoint);
                movement.curLader = null;
            }
            curPlayer = null;
        }
    }

    protected override void Interacted()
    {
        UnityEngine.Debug.Log("To be implemented");
    }
}
