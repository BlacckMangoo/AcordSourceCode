using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [SerializeField] private float moveSpeed = 2f;
    private float moveDirection = 1f;

    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float ledgeCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 wallCheckPosition;
    private Vector2 ledgeCheckPosition;

  
    [SerializeField] private Vector2 wallCheckBoxSize = new Vector2(0.2f, 0.2f); // Size of the wall check box
    [SerializeField] private Vector2 ledgeCheckBoxSize = new Vector2(0.2f, 0.2f); // Size of the ledge check box

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        UpdateCheckPositions();
        
        if (IsWallAhead() || IsLedgeAhead())
        {
            TurnAround();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

   

    void UpdateCheckPositions()
    {
        if (boxCollider == null)
        {
            return;
        }

        Bounds bounds = boxCollider.bounds;
        wallCheckPosition = new Vector2(
            moveDirection > 0 ? bounds.max.x + wallCheckDistance : bounds.min.x - wallCheckDistance,
            bounds.center.y
        );
        ledgeCheckPosition = new Vector2(
            moveDirection > 0 ? bounds.max.x : bounds.min.x,
            bounds.min.y - ledgeCheckDistance * 0.5f 
        );
    }

    bool IsWallAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheckPosition, Vector2.right * moveDirection, wallCheckDistance, groundLayer);
        return hit.collider != null;
    }

    bool IsLedgeAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeCheckPosition, Vector2.down, ledgeCheckDistance, groundLayer);
        return hit.collider == null;
    }

    void TurnAround()
    {
        moveDirection *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
     
        UpdateCheckPositions();

  
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPosition, wallCheckBoxSize);

       
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(ledgeCheckPosition, ledgeCheckBoxSize);
    }
}


