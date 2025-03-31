using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimator : MonoBehaviour
{

    public bool isInvincible;

    private PlayerMovement mov;
    public Animator anim;
    public GameObject spriteRend;
    [SerializeField] public AudioClip[] jumpSFX;

    [SerializeField] public AudioClip dashSfx;

    [Header("Movement Tilt")]
    [SerializeField] private float maxTilt;
    [SerializeField][Range(0, 1)] private float tiltSpeed;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    private ParticleSystem _jumpParticle;
    private ParticleSystem _landParticle;
    [SerializeField] private GameObject playerDeathExplodable;
    public bool startedJumping { private get; set; }
    public bool justLanded { private get; set; }

    public bool isfalling { private get; set; }
    public float currentVelY;
    private float currentVelX;
    public bool isGrounded { private get; set; }
    public bool isWallTouching;
    public bool isDead { private get; set; }
    public bool isRolling { private get; set; }

    private void Start()
    {
        mov = GetComponentInParent<PlayerMovement>();
        _jumpParticle = jumpFX.GetComponent<ParticleSystem>();
        _landParticle = landFX.GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        #region Tilt
        float tiltProgress;

        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? 1 : -1;
        }

        float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
        spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
        #endregion
        currentVelX = mov.RB.velocity.x;
        CheckAnimationState();

        ParticleSystem.MainModule jumpPSettings = _jumpParticle.main;

        ParticleSystem.MainModule landPSettings = _landParticle.main;

    }

    private void CheckAnimationState()
    {
        if (startedJumping)
        {
            SoundManager.instance.PlayRandomSoundFX(0.09f, jumpSFX, transform);
            anim.SetTrigger("Jump");
            anim.ResetTrigger("Land");
            // GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            // Destroy(obj, 1);
            startedJumping = false;
            return;

        }

        if (justLanded)
        {

            anim.SetTrigger("Land");
            anim.ResetTrigger("Jump");
            //  GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            // Destroy(obj, 1);
            justLanded = false;
            return;

        }

        if (isfalling)
        {
            anim.SetBool("falling", true);
        }
        else
        {
            anim.SetBool("falling", false);
        }

        if (Input.GetAxisRaw("Horizontal") != 0 && !mov.IsDashing && !mov.IsJumping && isGrounded && !mov.isLocked)
        {
            anim.SetBool("running", true);

        }
        else
        {
            anim.SetBool("running", false);
        }

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);

        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        if (isWallTouching)
        {
            anim.SetBool("isWallHolding", true);

        }
        else
        {
            anim.SetBool("isWallHolding", false);

        }

        if (Input.GetMouseButtonDown(1)&& mov.IsDashing)
        {
            anim.SetTrigger("dash");
            SoundManager.instance.PlaySoundFX(0.5f, dashSfx, transform);
        }


        if (isDead )
        {
            GameObject playerDeathExplodableObject = Instantiate(playerDeathExplodable, transform.position, transform.rotation);
            Destroy(mov.gameObject);

            DontDestroyOnLoad(playerDeathExplodableObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }



        if (isRolling)
        {
            anim.SetBool("rolling", true);


        }
        else
        {
            anim.SetBool("rolling", false);
        }





    }

    public void SetInvinsible()
    {
        isInvincible = true;
    }

    public void SetVulnerable()
    {
        isInvincible = false;

    }
}
   

