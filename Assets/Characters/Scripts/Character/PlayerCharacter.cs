using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    private PlayerMovement PMovement;
    private PlayerInput pInput;

    private bool weildObj = false;
    
    void Start()
    {
        PMovement = GetComponent<PlayerMovement>();
        pInput = GetComponent<PlayerInput>();

    }

    void Update()
    {
        if (pInput)
        {
            PMovement.HandleMovement(pInput.moveX, pInput.moveZ);
            PMovement.HandleJump(pInput.moveX, pInput.moveZ, pInput.JumpPressed);
            PMovement.HandleCrouch(pInput.CrouchPressed);
        }
 
             
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            //check if ladder
            Ladder curLadder = other.GetComponent<Ladder>();
            if (curLadder)
            {
                curLadder.OnTouchInteract(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Interactable>())
        {
            //check if ladder 
            Ladder curLadder = other.GetComponent<Ladder>();
            if (curLadder)
            {
                //Remove component
                curLadder.OnExitLadder();
            }
        }
    }
    public void OnPickupWeapon(PickUpObject Obj)
    {
        if(Obj.HandleLocation.position != Vector3.zero)
        {
            Obj.transform.SetParent(this.transform);
            weildObj = true;
        }
        //change animations if necesary 
    }
}
