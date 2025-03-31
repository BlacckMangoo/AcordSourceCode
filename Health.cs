using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int startHealth;
    [SerializeField] GameObject destroyFx;
    [SerializeField] AudioClip destroySFx;
    [SerializeField] GameObject moneyDrop; 
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;
    [SerializeField] float health;
    [SerializeField] int maxmoneeyToDrop; 
    

    void Start()
    {
        health = startHealth;
       
    }


    void Update()
    {
        if(health <= 0 )
        {
            OnDeath?.Invoke();
            Instantiate(destroyFx, this.transform.position,Quaternion.identity);
            if(moneyDrop != null)
            {
              GameObject money =  Instantiate(moneyDrop, this.transform.position, Quaternion.identity);
              MoneyDrop moneydrop = money.GetComponent<MoneyDrop>();
                moneydrop.noOfMoneyObjects = Random.Range(5, maxmoneeyToDrop);
                moneydrop.offset = Random.Range(1, 2); 
                

            }
            SoundManager.instance.PlaySoundFX(0.1f, destroySFx, transform);
            Destroy(this.gameObject);
           
        }

       
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("bullet"))
        {
            float damageBullet;
            damageBullet = collision.gameObject.GetComponent<Bullet>().bulletDamage;

            health -= damageBullet;
        }
    }








}
