using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{

    [SerializeField] Slider healthBar;
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 10000;
    [SerializeField] AudioClip[] bossbullethitsfx;
    [SerializeField] AudioClip bossdeathfx;
    [SerializeField] GameObject pillars;
    [SerializeField] GameObject healthbar;
    [SerializeField] GameObject deathExplodable;
    [SerializeField] GameObject LoadStartSceneOnDeath; 
    public bool issleeping = true; 

    [SerializeField] GameObject[] spawnhalfhealth;


    public CinemachineImpulseSource CinemachineImpulseSource;




    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        currentHealth = maxHealth;
        LoadStartSceneOnDeath.SetActive(false);
    }

    private void Update()
    
    {

        if( currentHealth <= maxHealth /2)
        {
            foreach (GameObject s in spawnhalfhealth)
            {
                s.SetActive(true);
            }
        }

        if (currentHealth <= 0)
        {
            Destroy(pillars);
            Destroy(healthbar);
            deathExplodable.transform.position = this.transform.position;
           //  SoundManager.instance.PlaySoundFX(0.35f, bossdeathfx, this.transform);

            CinemachineImpulseSource.GenerateImpulse();

            deathExplodable.SetActive(true);

            foreach (GameObject s in spawnhalfhealth)
            {
                s.SetActive(false);
            }
            LoadStartSceneOnDeath.SetActive(true);
            Destroy(this.gameObject,0.1f);
          

        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet" && issleeping == false)
        {

            currentHealth -= collision.gameObject.GetComponent<Bullet>().bulletDamage;
            healthBar.value = currentHealth;
            SoundManager.instance.PlaySoundFX(0.35f, bossbullethitsfx[UnityEngine.Random.Range(0,bossbullethitsfx.Length)],collision.transform);

        }
    }
}
