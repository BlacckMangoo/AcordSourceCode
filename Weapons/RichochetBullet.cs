using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichochetBullet : Bullet
{
    private Vector2 lastVelocity;
    [SerializeField] float slowFactor;
    public override void Start()
    {
        base.Start();
        MoveForward();
    }
    private void FixedUpdate()
    {
        
        lastVelocity = rb.velocity;
    }
    public override void Update()
    {
       
        base.Update();
        
      
    }

    private void MoveForward()
    {
        rb.velocity = transform.right * bulletSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 direction = Vector2.Reflect(lastVelocity, normal);
            rb.velocity = direction.normalized * bulletSpeed * slowFactor;

            // Slightly move the bullet away from the collision point
            transform.position += (Vector3)(normal * 0.01f);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the bullet
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            
            Destroy(this.gameObject, 0.05f);
        }
        
           


    }
}
