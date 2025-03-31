using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
   [SerializeField] PlayerMovement mov;
    [SerializeField] float trampoline_jump_force;
    
    [SerializeField] Animator animator;

    // Start is called before the first frame update
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") )
        {
         
            mov.Jump(trampoline_jump_force);
            animator.SetTrigger("jump");
        }
    }
}
