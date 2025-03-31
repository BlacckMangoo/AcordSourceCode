
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class ChooseRandomFist : Action
{
    public SharedGameObject[] fistObject;
    public SharedGameObject selectedFist;
    
    public override void OnStart()
    {
        selectedFist = fistObject[Random.Range(0,fistObject.Length)];
        
    }
    public override TaskStatus OnUpdate()
    {
        if( selectedFist != null)
        {

           return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }


}
