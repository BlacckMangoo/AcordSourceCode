
using UnityEngine;

public class Fatjumper : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform movedirCheckerRight;
    [SerializeField] Transform movedirCheckerLeft;
    [SerializeField] Transform player;
 
    float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float side_force;
    [SerializeField] private float fallGravity_multiplier;
    public GameObject groundChecker;
    public Vector2 groundCheckerSize;
    public LayerMask groundLayer;
    [SerializeField] Vector3 moveDirCheckerSize;
    
    [SerializeField] Transform wallcheckerRight;
    [SerializeField] Transform wallcheckerLeft;
    Vector2 movedir =  Vector2.right;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movedir.x = 1;
        gravity = rb.gravityScale;
    }

    void Update()
    {
       
        animator.SetBool("grounded",isTouchingGround() );


        if(rb.velocity.y <0)
        {
            rb.gravityScale *= fallGravity_multiplier;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        if (TouchingLeftWall() || TouchingRightWall())
        {
            movedir.x *= -1;
        }

        Move();
        

    }
    private bool isTouchingGround()
    {
        return Physics2D.OverlapBox(groundChecker.transform.position, groundCheckerSize, 0, groundLayer);
    }

    private bool LedgeAway()
    {
        return Physics2D.OverlapBox(movedirCheckerRight.transform.position, groundCheckerSize, 0, groundLayer) &&  Physics2D.OverlapBox(movedirCheckerLeft.transform.position, groundCheckerSize, 0, groundLayer); ;
    }
    private bool LeftLedgeAway()
    {
        return Physics2D.OverlapBox(movedirCheckerLeft.transform.position, groundCheckerSize, 0, groundLayer);
    }
    private bool RightLedgeAway()
    {
        return Physics2D.OverlapBox(movedirCheckerRight.transform.position, groundCheckerSize, 0, groundLayer);
    }

    private bool TouchingRightWall()
    {
        return Physics2D.OverlapBox(wallcheckerRight.transform.position, groundCheckerSize, 0, groundLayer);
    }
    
    private bool TouchingLeftWall()
    {
        return Physics2D.OverlapBox(wallcheckerLeft.transform.position, groundCheckerSize, 0, groundLayer);
    }
        

    public void Move()
    {

        if (isTouchingGround())
        {
            rb.velocity = new Vector2(movedir.x*side_force,1) * jumpHeight;
          
        }

           
    }

  

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundChecker.transform.position, groundCheckerSize);
        Gizmos.DrawWireCube(movedirCheckerRight.position, moveDirCheckerSize);
        Gizmos.DrawWireCube(movedirCheckerLeft.position, moveDirCheckerSize);
        Gizmos.DrawWireCube(wallcheckerLeft.transform.position, groundCheckerSize);
        Gizmos.DrawWireCube(wallcheckerRight.transform.position, groundCheckerSize);
    }

  
}
