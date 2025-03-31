using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected  PlayerMovement player;
    [SerializeField] protected  float bulletSpeed;
    [SerializeField] public float bulletDamage;
    [SerializeField] protected float bulletLifeTime;
    [SerializeField] public float fireRate;
    [SerializeField] protected ParticleSystem hit_fx;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
       
    }

    // Update is called once per frame
    public virtual void Update()
    {
     
        if( bulletLifeTime > 0)
        {
            bulletLifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
       
    }

    
   
   

   


}
