using BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    
    public int currentHearts;
    PlayerMovement mov;
    Rigidbody2D rb;
    public Animator hit_flash;
    bool isInvincible = false;
    private bool isDead = false;
    [SerializeField] float iFrames = 1.3f;
    [SerializeField] AudioClip HitSfx;
    [SerializeField] GameObject[] heartObjects;
    [SerializeField] CinemachineImpulseSource impulseSource;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHearts = heartObjects.Length;
        mov = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if ((currentHearts < 1) && !isDead)
        {
            isDead = true;
            mov.PlayerDie();
        }
    }
    public void TakeDamage()
    {
        if (isDead) return; // Skip damage if already dead

        TriggerHeartBreakAnimation(currentHearts);
        currentHearts--;
        isInvincible = true;
        DOVirtual.DelayedCall(iFrames, SetInvincibility);
        impulseSource.GenerateImpulse();
    }
    void SetInvincibility()
    {
        isInvincible = false;
    }
    public void OnHit(int pushback)
    {
        SoundManager.instance.PlaySoundFX(0.5f, HitSfx, transform);
        if (isInvincible == false && !isDead)
        {
            TakeDamage();
            // Calculate a more realistic direction vector
            // Using player's facing direction to determine knockback direction
            float directionX = -Mathf.Sign(transform.localScale.x); // Assuming character faces right when scale.x > 0

            // Apply force with slight upward component for more realistic knockback
            Vector2 knockbackDirection = new Vector2(directionX, 0.5f).normalized;
            rb.AddForce(knockbackDirection * pushback, ForceMode2D.Impulse);

            hit_flash.Play(0);
            
        }
    }
    void TriggerHeartBreakAnimation(int heartIndex)
    {
        if (heartIndex > 0)
        {
            {
                Animator heartAnimator = heartObjects[heartIndex - 1].GetComponent<Animator>();
                if (heartAnimator != null)
                {
                    heartAnimator.SetTrigger("heartbreak");
                }
            }
        }
    }

    public void HealFully()
    {
        if(currentHearts == heartObjects.Length)
        {
            return;
        }
        currentHearts = heartObjects.Length;
        foreach (GameObject heart in heartObjects)
        {
            Animator heartAnimator = heart.GetComponent<Animator>();
            if (heartAnimator != null)
            {
                heartAnimator.SetTrigger("beat");
            }
        }

    }
}