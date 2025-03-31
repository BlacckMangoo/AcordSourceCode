using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Cinemachine;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Unity.Mathematics;

public class FistSmash : Action
{
    
    public SharedGameObject selectedFist;
    public CinemachineImpulseSource CinemachineImpulseSource;
    public ParticleSystem impactfx;
    public  AudioClip impactsfx;
    public GameObject shockWave;
    public Vector2 offsetShockwave;
 
    public SharedFloat punchDuration = 0.5f;
    public SharedFloat maxPunchDistance = 5f;
   public  Transform fistComeBackPos;

    public LayerMask groundLayer;
    Vector2 hitpoint;

    private float originalYPosition;

    public override void OnStart()
    {
        if (selectedFist.Value != null)
        {
            originalYPosition = selectedFist.Value.transform.position.y;
        }
    }
    public void InstantiateShockwave()
    {
        Object.Instantiate(shockWave, hitpoint + offsetShockwave, Quaternion.identity);
    }
    public override TaskStatus OnUpdate()
    {
        if (selectedFist.Value == null)
        {
            return TaskStatus.Failure;
        }

        RaycastHit2D hit = Physics2D.Raycast(selectedFist.Value.transform.position, Vector2.down, maxPunchDistance.Value, groundLayer);

        if (hit.collider != null)
        {
            float distanceToGround = hit.distance;
            Vector3 punchTarget = new Vector3(selectedFist.Value.transform.position.x, originalYPosition - distanceToGround, selectedFist.Value.transform.position.z);
             hitpoint = hit.point;
         
           
            // Punch towards the ground
            selectedFist.Value.transform.DOMoveY(punchTarget.y, punchDuration.Value).SetEase(Ease.InQuad).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.5f, InstantiateShockwave);
                 Object.Instantiate(impactfx, hitpoint , quaternion.identity);
              
                CinemachineImpulseSource.GenerateImpulse();
                SoundManager.instance.PlaySoundFX(0.7f, impactsfx,hit.transform);
                
                selectedFist.Value.transform.DOMoveY(fistComeBackPos.position.y, punchDuration.Value / 2).SetEase(Ease.OutQuad);
            });

            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("No ground detected");
        }

        return TaskStatus.Failure;
    }
}
