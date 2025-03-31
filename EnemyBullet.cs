using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    Vector2 target;
    [SerializeField] float bulletSpeed;
    Rigidbody2D rb;
    Vector2 startpos;
    [SerializeField] ParticleSystem hitfx;
    PlayerHealth player; 
    // Start is called before the first frame update
    void Start()
    {

        target = FindObjectOfType<PlayerMovement>().transform.position;
        rb = GetComponent<Rigidbody2D>();
       startpos = transform.position;
        player = FindObjectOfType<PlayerHealth>();
 
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        rb.velocity = target - startpos;
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hitfx.Play();
            Destroy(this.gameObject, 0.2f);


        }

        if( collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            player.TakeDamage();
            player.OnHit(3);
            Debug.Log("player Hit");
            Destroy(this.gameObject);
        }
    }
}
