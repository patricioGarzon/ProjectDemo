using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;
    public float followSpeed = 5f;

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // Target position (player position + offset)
        Vector3 targetPosition = player.position + offset;

        // Smoothly move camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

    }
}
