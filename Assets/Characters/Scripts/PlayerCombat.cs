using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    bool canAttack = true;

    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable() => PlayerInput.OnAttack += Attack;

    private void OnDisable() => PlayerInput.OnAttack -= Attack;

    public void Attack()
    {
        UnityEngine.Debug.Log("Delegate arrived Combat Script");
        // check if user has collectable and can perform action
        animator.SetTrigger("");
    }
}
