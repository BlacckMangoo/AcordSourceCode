using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAction : Action 
{
    protected Rigidbody2D rb; 
    protected Animator animator;
    protected PlayerMovement player;
    protected PlayerAttack playerAttack;
    protected Vector2 position;
    void Start()
    {
         position = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
            player = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();

    }
}
