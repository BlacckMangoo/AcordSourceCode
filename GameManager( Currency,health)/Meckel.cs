using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meckel : MonoBehaviour
{
    [SerializeField] int moneyValue;
    DataManager dataManager;
    Rigidbody2D rb; 
    [SerializeField] float startForce = 2f;
    [SerializeField]  AudioClip[] coinCollect ; 

    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dataManager = FindObjectOfType<DataManager>();

        rb.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(0.5f,1)) * startForce, ForceMode2D.Impulse);
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if( collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            if ( coinCollect != null ) 
            SoundManager.instance.PlaySoundFX(1f, coinCollect[Random.Range(0,coinCollect.Length)], this.transform);
           
            dataManager.AddMoney(moneyValue);
            Destroy(this.gameObject);
        }
    }
}
