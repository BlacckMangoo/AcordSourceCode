using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public GameObject itemPrefab; 

    private void Start()
    {
        // Find the Health script on the same GameObject
        Health healthScript = GetComponent<Health>();
        if (healthScript != null)
        {
            // Subscribe to the OnDeath event
            healthScript.OnDeath += SpawnItem;
        }
    }

    private void SpawnItem()
    {
       
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

}
