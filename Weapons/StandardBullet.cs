using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : Bullet 
{
    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();
        MoveForward();
    }
    private void MoveForward()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ParticleSystem fx = Instantiate(hit_fx, transform.position, Quaternion.identity);
        fx.Play();
        Destroy(fx.gameObject, 0.4f);
        Destroy(this.gameObject, 0.05f);

    }
}
