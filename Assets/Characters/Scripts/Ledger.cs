using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledger : Interactable
{
    public Transform[] desiredLocations;
    int indexLocations = 0;
    private void Start()
    {
        
    }
    protected override void Interacted()
    {
        if(indexLocations >= desiredLocations.Length) indexLocations = 0;
        //; Move ledger from left to right center mass at bottom. animation can help

        StartCoroutine(PlatformMoveRoutine(indexLocations));
        indexLocations++;

    }

    //Desired case (move obj or give access can happen here.
    private IEnumerator PlatformMoveRoutine(int Index)
    {
        float duration = 3.0f;
        float elapse = 0f;
        Vector3 start = desiredLocations[Index].position;
        Vector3 end = desiredLocations[Index+1].position;

        //Traverse the platform to the desired location
        while(elapse < duration)
        {
            float t = elapse / duration;
            transform.position = Vector3.Lerp(start, end, t);
            elapse += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

}
