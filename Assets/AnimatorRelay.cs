using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRelay : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement controller;
    void Start()
    {
        //controller = GetComponentInParent<PlayerMovement>();
    }
    public void OnLadderExitComplete()
    {
        controller.canWalk = true;

    }
}
