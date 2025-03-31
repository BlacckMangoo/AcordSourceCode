using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddForce : MonoBehaviour
{
 
    [SerializeField] private int force;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 randDir;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        randDir = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
        rb.velocity = randDir * force;
       
    }

    private void Update()
    {
        
    }

}
