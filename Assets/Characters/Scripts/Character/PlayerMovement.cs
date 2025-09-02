using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // GRAVITY 
    public float gravity = -9.81f;

    //PLAYER MOVEMENT
    public float moveSpeed = 3f;  
    public float zClampRange = 0.5f;
    public float rotationSpeed = 10f; // Smooth turning
    private Vector3 velocity;
    public bool canWalk = true;

    //PLAYER JUMPING 
    public float jumpHeight = 1f;
    public bool isJumping = false;
    private Vector3 jumpMoveDirection = Vector3.zero;

    //CROUCHING
    private float standingHeight = 2.0f; 
    private float crouchingHeight = 1.0f;

    //COMPONENTS
    private CharacterController controller;
    private Animator anim;

    /// FOR CLIMBING LADDER 
    public bool setClimbing { get; set; }
    public float ClimbSpeed = 1.2f;
    private bool ExitingLadder = false;
    public Transform modelTransform;
    public Ladder curLader; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        standingHeight = controller.height;
    }

    private void Update()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // Keep model centered on the controller
        modelTransform.localPosition = Vector3.zero;
        modelTransform.localRotation = Quaternion.identity;
    }
    public void HandleMovement(float moveX,float moveZ)
    {
        if (!setClimbing && canWalk)
        {
           // anim.applyRootMotion = true;
            Vector3 pos = transform.position;
            pos.z = Mathf.Clamp(pos.z, -zClampRange, zClampRange);
            transform.position = pos;

            Vector3 horizontalMove = new Vector3(moveX, 0f, moveZ) * moveSpeed;

            if (horizontalMove.sqrMagnitude > 0.001f)
            {
                float targetYRotation = Mathf.Atan2(horizontalMove.x, horizontalMove.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }


            // Gravity
            if (IsGrounded() && velocity.y < 0)
            {
                velocity.y = -2f; // keep grounded
                isJumping = false;
            }
            velocity.y += gravity * Time.deltaTime;
            Vector3 totalMove = Vector3.zero;
            // Combine horizontal + vertical movement     
            totalMove = horizontalMove + new Vector3(0f, velocity.y, 0f);
            controller.Move(totalMove * Time.deltaTime);

            // Animator updates
            float planarSpeed = horizontalMove.magnitude;
            anim.SetFloat("Speed", planarSpeed);
            anim.SetBool("IsGrounded", IsGrounded());

        }
        else if(setClimbing && !ExitingLadder)
        {
            HandleClimbing(moveZ);
            //anim.applyRootMotion = false;
            //check if is grounded and climb speed < 0.1 play reverse entri animation ladder
            if(moveZ < 0.1 && IsGrounded())
            {
                //play exit ladder down. 
                if (curLader)
                {
                    ExitLadder(curLader.exitBottomPoint);
                }              
            }
        }
        anim.SetBool("IsClimbing", setClimbing);
    }

    public void HandleJump(float moveX,float moveZ,bool canJump)
    {

        if(canJump && IsGrounded())
        {
            anim.SetTrigger("JumpTrigger");
            anim.SetBool("HasJumped", true);
            // Store movement at jump start
            jumpMoveDirection = new Vector3(moveX, 0f, moveZ);
            float movementMag = jumpMoveDirection.magnitude;

            float delay = (movementMag > 0.01f) ? 0.1f : 0.3f;

            StartCoroutine(ApplyJumpAfterDelay(delay));
            isJumping = true;
        }
    }

    public bool IsGrounded()
    {
        float rayDistance = 0.2f;
        LayerMask groundLayer = LayerMask.GetMask("Ground");
 
        // Always cast from Player root (where CharacterController is), not the model
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        anim.SetBool("HasJumped", false);
        return controller.isGrounded;
    }

    IEnumerator ApplyJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void HandleCrouch(bool Crouching)
    {
        moveSpeed = (Crouching) ? 1.5f : 3.0f;
        if (Crouching && IsGrounded())
        {            
            if (Crouching)
            {                
                controller.height = 1.0f;
                controller.center = new Vector3(0, crouchingHeight / 2f, 0);
            }
            else
            {                
                controller.height = 2.0f;
                controller.center = new Vector3(0, standingHeight / 2f, 0);
            }            
        }
        anim.SetBool("isCrouching", Crouching);
    }
    public void HandleClimbing(float moveY)
    {
        anim.applyRootMotion = true;
        setClimbing = true;
        Vector3 moveVertical = new Vector3(0f, moveY, 0f) * ClimbSpeed;
        anim.SetFloat("ClimbSpeed", moveY);
        controller.Move(moveVertical * Time.deltaTime);
        canWalk = false;
        OnAnimatorMove();
    }

    public void ExitLadder(Transform targetPos)
    {
        anim.applyRootMotion = false;
        anim.SetBool("ClimbExit",true);
        StartCoroutine(ExitLadderRoutine(targetPos));
    }

    private IEnumerator ExitLadderRoutine(Transform targetPos)
    {
        canWalk = false;
        setClimbing = false;       
        Vector3 start = transform.position;
        // 1. Go up (Y only)
        float upDuration = 2.2f;
        float elapsed = 0f;
        Vector3 upTarget = new Vector3(start.x, targetPos.position.y, start.z);

        while (elapsed < upDuration)
        {
            float t = elapsed / upDuration;
            transform.position = Vector3.Lerp(start, upTarget, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = upTarget;

        // 2. Go forward (Z/X plane)
        float forwardDuration = 0.6f;
        elapsed = 0f;
        Vector3 forwardTarget = targetPos.position;

        while (elapsed < forwardDuration)
        {
            float t = elapsed / forwardDuration;
            transform.position = Vector3.Lerp(upTarget, forwardTarget, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = forwardTarget;        
        anim.SetBool("IsClimbing", false);
        anim.SetBool("ClimbExit", false);
        anim.SetFloat("Speed", 0f);
    }
    void OnAnimatorMove()
    {
        if (anim.hasRootMotion)
        {
            Vector3 delta = anim.deltaPosition;
            Vector3 horizontal = new Vector3(delta.x, 0f, delta.z);
            controller.Move(horizontal);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);

            transform.rotation *= anim.deltaRotation;
        }
    }
}
