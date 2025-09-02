using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Called when player presses interact button
    public PlayerCharacter curPlayer;
    private void OnCollisionEnter(Collision collision)
    {
        curPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (curPlayer)
        {
            PlayerInput.OnInteractPressed += Interacted;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        curPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (curPlayer)
        {
            PlayerInput.OnInteractPressed -= Interacted;
            curPlayer = null;
        }
    }

    protected abstract void Interacted();
}
