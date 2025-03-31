using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using Cinemachine;



using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    [SerializeField] float rotation_interpolation;
    [SerializeField] float downwardRayDistance;
    [SerializeField] ParticleSystem shootfx;
    public GameObject cannonBallPrefab;
    public Transform firePoint;
    public LayerMask collisionLayer;
    [SerializeField] Transform tipofGun;
    [SerializeField] Transform centreOfgun;
    PlayerMovement player;
    [SerializeField] GameObject gunBone;
    Rigidbody2D rb;
   [SerializeField] float firerate = 0.3f; 
   float  nextFireTime = 0f;
    public float rayDistance; 
    public float cannonSpeed;
    bool hasCollided = false;
    public float  shootpause ; 
    float shootPauseTimer = 0.3f ;
    public float lerpSpeed = 0.5f;
    public Vector2 forceRange; 
    private Vector2 moveDirection = Vector2.right; 
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * cannonSpeed;
    }

    private void FixedUpdate()
    {
        CalculateAngleToThrowAt();
        // Check if it's time to fire again
        if (Time.time >= nextFireTime)
        {
            Shoot(); // Call the Shoot method

            nextFireTime = Time.time + firerate; // Set the time for the next shot
        }
        shootPauseTimer -= Time.deltaTime;
    }
    void Update()
    {
        MoveCannon();

    }

    void MoveCannon()
    {
        if ((shootPauseTimer < 0 ))
        {
            rb.velocity = moveDirection * cannonSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
     
        Vector3 startPoint = transform.position;

        // Set the direction to be horizontal (right)
        Vector3 direction = rb.velocity;
        RaycastHit2D hitInfo = Physics2D.Raycast(startPoint, direction,   rayDistance, collisionLayer);

        if (hitInfo.collider != null && hasCollided == false)
        {

            moveDirection = moveDirection * -1;
           
            
            Debug.Log("Hit");
            hasCollided = true;
            Invoke("ResetCollsiom", 0.3f);
            
        }
      


    }

    private void ResetCollsiom()
    {
        hasCollided = !hasCollided;
    }

    private   void   CalculateAngleToThrowAt()
    {
      

        float angle; 

        
        if(transform.position.x < player.transform.position.x)
        {
           
                angle = Mathf.Lerp(30, 90, Mathf.PingPong(Time.time * lerpSpeed, rotation_interpolation));
            
           
        }
        else
        {
           
            angle = Mathf.Lerp(130, 90, Mathf.PingPong(Time.time * lerpSpeed, rotation_interpolation));
        }
        


        gunBone.transform.rotation = Quaternion.Euler(0, 0, angle);

    }


    public void Shoot()
    {
        
        shootfx.Play();
        shootPauseTimer = shootpause; 
        
      // CinemachineImpulseSource shake = FindAnyObjectByType<CinemachineImpulseSource>();
       // shake.GenerateImpulse();

        Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        CannonBall cannonball = cannonBallPrefab.GetComponent<CannonBall>();
        cannonball.shootDir = tipofGun.position - centreOfgun.position ;
        cannonball.cannonForce =  Random.Range(forceRange.x , forceRange.y);
      
    }

   

    private void OnDrawGizmos()
    {
     
        Gizmos.color = Color.red;

        Vector3 startPoint = transform.position;
        Vector3 direction = Vector3.right * rayDistance;

       
        Gizmos.DrawLine(startPoint, startPoint + direction);
        Gizmos.DrawLine(startPoint, startPoint + Vector3.down * downwardRayDistance);
    }
}
