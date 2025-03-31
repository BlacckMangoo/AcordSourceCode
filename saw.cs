using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class saw : MonoBehaviour
{
   
    public float  moveSpeed = 0.3f ;
    
    public Transform startPosMarker;
    public bool movingToEnd = true;
    public Transform endPosMarker;
    private Rigidbody2D rb;
    private Vector3 targetVelocity;

    private void Start()
    {
        transform.position = startPosMarker.position;   
        rb = GetComponent<Rigidbody2D>();
        targetVelocity = (endPosMarker.position - startPosMarker.position).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = movingToEnd ? endPosMarker.position : startPosMarker.position;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Check if reached the target position
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            movingToEnd = !movingToEnd;
    }

}
