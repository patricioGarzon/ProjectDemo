using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;

    void Start() => anim = GetComponent<Animator>();

    public void UpdateAnimator(float speed, bool grounded, bool crouching, bool jumping)
    {
        anim.SetFloat("Speed", speed);
        anim.SetBool("IsGrounded", grounded);
        anim.SetBool("isCrouching", crouching);
        if (jumping) anim.SetTrigger("JumpTrigger");
    }
}
