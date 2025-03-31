using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FistBlowFire : Action
{
    public SharedGameObject selectedFist; 
    public override void OnStart()
    {
       
       
    }
    public override TaskStatus OnUpdate()
    {
        if (selectedFist != null)
        {
            return TaskStatus.Failure;
        }
        return base.OnUpdate();
    }

    void FollowPlayer(GameObject target)
    {
        {

        }
    }
}
