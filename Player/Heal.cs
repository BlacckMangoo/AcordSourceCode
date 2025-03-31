using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("player") && collision.gameObject.GetComponent<PlayerHealth>().currentHearts < 7)
        {
            
                collision.gameObject.GetComponent<PlayerHealth>().HealFully();
            collision.gameObject.GetComponent<PlayerHealth>().currentHearts = 7; 



        }

        Destroy(gameObject,0.1f);
    }

    
}