using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip shootSfx;
    [SerializeField] float fireRate;
    [SerializeField] Vector2 shootHorRecoil;
    [SerializeField] Vector2 shootUpRecoil;

    public bool canshoot;
    Quaternion bulletRotation;
    [SerializeField] AudioClip gunEquipSfx;
    [SerializeField] ParticleSystem gunfx;
    [SerializeField] ParticleSystem gunfxup;
    float time;
    [SerializeField] Transform bulletInstantiatePos;
    [SerializeField] Transform bulletInstantiatePosUp;
    PlayerMovement playerMovement;
    [SerializeField] Animator anim;
    PlayerAnimator playerAnimator;
    float xinput;

    WeaponManager weaponManager;

    // Variables for smoothing
    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private float rotationSpeed = 10f;
    private Transform currentBulletPos;
    private Transform targetBulletPos;
    private Vector2 currentRecoil;
    private Vector2 targetRecoil;
    private ParticleSystem currentGunFx;
    private ParticleSystem targetGunFx;
    private string currentAnimTrigger = "shootHor";

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>(); // Use GetComponent instead of FindAnyObjectByType
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<PlayerAnimator>();
        }

        weaponManager = FindObjectOfType<WeaponManager>();

        // Initialize smooth variables
        currentBulletPos = bulletInstantiatePos;
        targetBulletPos = bulletInstantiatePos;
        currentRecoil = shootHorRecoil;
        targetRecoil = shootHorRecoil;
        currentGunFx = gunfx;
        targetGunFx = gunfx;
    }

    private void Update()
    {
        xinput = Input.GetAxis("Horizontal");
        bulletRotation = transform.rotation;
        time += Time.deltaTime;

        bool canPerformAction = !playerMovement.isRolling &&
                               !playerAnimator.isWallTouching &&
                               !playerMovement.IsWallJumping &&
                               !playerMovement.IsDashing;

        // Smooth transitions between shooting states
        SmoothTransitions();

        if (Input.GetMouseButton(0) && canPerformAction)
        {
            // Animator layer weights (using 0-1 range)
            anim.SetLayerWeight(1, 1);
            anim.SetLayerWeight(0, 0);

            if (time > weaponManager.bulletPrefabs[weaponManager.currentBulletType].GetComponent<Bullet>().fireRate)
            {
                bool upPressed = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);

                if (upPressed)
                {
                    // Upward shooting (even while running)
                    targetAngle = 90f;
                    targetBulletPos = bulletInstantiatePosUp;
                    targetRecoil = shootUpRecoil;
                    targetGunFx = gunfxup;
                    currentAnimTrigger = "shootUp";
                }
                else
                {
                    // Horizontal shooting
                    targetAngle = 0f;
                    targetBulletPos = bulletInstantiatePos;
                    targetRecoil = shootHorRecoil;
                    targetGunFx = gunfx;
                    currentAnimTrigger = "shootHor";
                }

                ShootWithCurrentSettings();
                time = 0;
            }
        }
        else
        {
            // Only change layer weights when needed
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            canshoot = !canshoot;
            SoundManager.instance.PlaySoundFX(0.5f, gunEquipSfx, transform);
        }
    }

    private void SmoothTransitions()
    {
        // Smoothly lerp between current and target angle
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);

        // Smoothly lerp between current and target recoil
        currentRecoil = Vector2.Lerp(currentRecoil, targetRecoil, Time.deltaTime * rotationSpeed);

        // No need to lerp between transforms, just use the target
        currentBulletPos = targetBulletPos;
        currentGunFx = targetGunFx;
    }

    private void ShootWithCurrentSettings()
    {
        GameObject bulletPrefab = weaponManager.bulletPrefabs[weaponManager.currentBulletType];
        SoundManager.instance.PlaySoundFX(0.1f, shootSfx, transform);

        // Trigger the appropriate animation
        anim.SetTrigger(currentAnimTrigger);

        // Apply small random variation to angle for more natural shooting
        float randomVariation = Random.Range(-2f, 2f);
        Quaternion finalRotation = bulletRotation * Quaternion.Euler(0, 0, currentAngle + randomVariation);

        // Instantiate bullet
        Instantiate(bulletPrefab, currentBulletPos.position, finalRotation);

        // Apply recoil
        Recoil(currentRecoil);

        // Play particle effect
        if (currentGunFx != null && !currentGunFx.isPlaying)
        {
            currentGunFx.Play();
        }
    }

    private void Recoil(Vector2 force)
    {
        if (transform.rotation.y == 180)
        {
            playerMovement.RB.AddForce(force * xinput, ForceMode2D.Impulse);
        }
        else
        {
            playerMovement.RB.AddForce(force, ForceMode2D.Impulse);
        }
    }
}