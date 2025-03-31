using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Boss1Sleep: Conditional
{
    public SharedGameObject player;
    public float activationDistance = 10f;

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, player.Value.transform.position) <= activationDistance)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
