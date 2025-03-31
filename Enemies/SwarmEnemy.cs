using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float shootingRate; // shoots every shooting rate seconds
    [SerializeField] GameObject bullet;

  
    private Vector3 startpos;
    float radiusToCover = 30;
    float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Store the starting position as a Vector3
        startpos = transform.position;
        timer = 0;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && ! (Vector2.Distance(startpos, transform.position) > radiusToCover))
        {
            Shoot();
            timer = shootingRate;
        }
    }

    private void Update()
    {
        if( Vector2.Distance(startpos, transform.position) > radiusToCover)
        {
            transform.position = Vector2.MoveTowards(transform.position, startpos, speed * Time.deltaTime);
        }
      

        if (player != null && !(Vector2.Distance(startpos, transform.position) > radiusToCover))
        {
            Vector2 playerpos = player.transform.position;
            Vector2 pos = transform.position;
            rb.velocity = new Vector2(playerpos.x - pos.x - Mathf.Sin(Time.time) * 3, playerpos.y - pos.y + Mathf.Sin(Time.time) * Mathf.Sin(Time.time) * 11);
        }
    }

    private void Shoot()
    {
    
        Debug.Log("Shoot");
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusToCover);
        Gizmos.color = Color.red;
    }
}