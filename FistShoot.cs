using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

using DG.Tweening;
using System.Numerics;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using UnityEngine;

public class FistShoot :  Action
{
    [SerializeField] float fireRate = 1;
    [SerializeField] float shootduration = 3;
    float timer; 
    

    [SerializeField] float shootTimer = 0;


    public SharedGameObject fist;




    public override TaskStatus OnUpdate()
    {
        

        // Update timers
        shootTimer += Time.deltaTime;
        timer += Time.deltaTime;
   

        // Shoot if it's time
        if (shootTimer >= fireRate)
        {
            fist.Value.GetComponent<Fist>().Shoot();
            shootTimer = 0;
            Debug.Log("Fist is shooting");
        }


        if( timer >= shootduration)
        {
            return TaskStatus.Success;
        }



        // If we're still executing and nothing has failed, return Running
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        timer  = 0;
        shootTimer = 0;
     
    }
}



