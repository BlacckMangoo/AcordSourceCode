using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public GameObject itemPrefab; 

    private void Start()
    {
      
        Health healthScript = GetComponent<Health>();
        if (healthScript != null)
        {
            healthScript.OnDeath += SpawnItem;
        }
    }

    private void SpawnItem()
    {
       
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

}
