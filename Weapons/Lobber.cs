using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobber : Bullet
{
    public override void Start()
    {
        base.Start();
        if(player.IsFacingRight)
        {
            rb.AddForce( new Vector2(1,1)* bulletSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce( new Vector2(-1,1)* bulletSpeed, ForceMode2D.Impulse);
        }
       
    }

    public override void Update()
    {
        base.Update();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ParticleSystem fx = Instantiate(hit_fx, transform.position, Quaternion.identity);
        fx.Play();
        Destroy(fx.gameObject, 0.4f);
        Destroy(this.gameObject, 0.05f);

    }


}
