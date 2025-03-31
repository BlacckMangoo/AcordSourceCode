using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Windows;
public class obstacle : MonoBehaviour
{

    PlayerHealth playerHealth;
   [SerializeField] int playerPushbackStrength = 20 ;
    void Start()
    {

        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.OnHit(playerPushbackStrength);
            Debug.Log(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.OnHit(playerPushbackStrength);
            Debug.Log(collision.gameObject);
        }

    }
}
