using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static event Action OnInteractPressed;
    public static event Action OnAttack;


    public float moveX {  get; private set; }
    public float moveZ { get; private set; }

    public bool JumpPressed = false;
    public bool CrouchPressed = false;
    public bool InteractPressed = false;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        // Input
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        JumpPressed = Input.GetButtonDown("Jump");
        CrouchPressed = Input.GetButton("Crouch");
        InteractPressed = Input.GetButton("Interact");

        if (Input.GetMouseButtonDown(0))
        {
            //Handle useer input 
            OnAttack?.Invoke();
        }

    }
}
