using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    Rigidbody2D rb;
    public float cannonForce;
    public Vector2 shootDir;
    public GameObject fx;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(shootDir * cannonForce, ForceMode2D.Impulse);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

       fx = Instantiate(fx, transform.position, Quaternion.identity);
        fx.GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject, 0.05f);
    }
}
