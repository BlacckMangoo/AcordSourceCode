using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [SerializeField] private float shockwaveSpeed = 5f;
    [SerializeField] private float lifetime = 1f;
   

    private Rigidbody2D rb;
    private bool playerIsRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Transform player = FindObjectOfType<PlayerMovement>()?.transform;

        if (player != null)
        {
            playerIsRight = player.position.x > transform.position.x;
        }

        Destroy(gameObject, lifetime);
       
    }

    private void Update()
    {
        if (rb != null)
        {
            Vector2 direction = playerIsRight ? Vector2.right : Vector2.left;
            rb.velocity = direction * shockwaveSpeed;
        }
    }
}
