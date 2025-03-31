using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform spaawnpos; 

  

    public void Shoot()
    {
        Instantiate(projectile, spaawnpos.position, Quaternion.identity);


    }
}
