using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraShipTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] private float DampTime = 0.2f;
 // [SerializeField] private float ScreenEdgeBuffer = 0.2f;
//   [SerializeField] private float MinSize = 2.1f;
//   [SerializeField] private float MaxSize = 2.92f;
//   [SerializeField] private float ZoomSpeed = 0.4f;
//    private CinemachineImpulseSource impulseSource;
   private Camera cam;
   public  List<Transform> targets; 
   private Vector3 Velocity;
   private Vector3 DesiredPos;
//    private Vector3 origPos;
//    private float ShakeY;

   private const float cameraHeight = 25.61f;

   private const float SpacshipDistance = -5.1f;

   Vector3 camDistance = new Vector3(0, cameraHeight, SpacshipDistance);

    

    // private void setCamerShake(CameraShakeEvent cameraMovements)
    // {
    //     ShakeCamera(cameraMovements.ShakeAmount, cameraMovements.TimeDuration);
    // }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        //impulseSource = GetComponent<CinemachineImpulseSource>();
        
    }

    private void FixedUpdate()
    {

        if (targets.Count == 0) return;

        targets.RemoveAll(t => t == null);

        Move();
        //Zoom();
    }

    // private void ShakeCamera(float ShakeAmount, float timeDuration)
    // {
        

    //    var definition = impulseSource.ImpulseDefinition;

    //     // Note: In some versions, these are just 'SustainTime' and 'DecayTime'
    //     // without the 'm_' prefix if they are accessed via the public property.
    //     definition.TimeEnvelope.SustainTime =  timeDuration * 0.5f;
    //     definition.TimeEnvelope.DecayTime =  timeDuration * 0.5f;

    //     impulseSource.GenerateImpulse(Vector3.up * ShakeAmount);
       
    // }

    private void Move()
    {
        FindAveragePosition();
       // DesiredPos.y += ShakeY;
        Vector3 TargetPosition = DesiredPos + camDistance;
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, DampTime);
    }

    private void FindAveragePosition()
    {
        if (targets == null)
        {
            Debug.Log("could not find targerts");
            return;
        }

        Vector3 averagePos = new Vector3();
        int noTargets = 0;

        for (int i = 0; i < targets.Count; i++)
        {
             if (targets[i] == null || !targets[i].gameObject.activeSelf)
                continue;

                averagePos += targets[i].position;
                noTargets++;

        }

        if (noTargets > 0)
        {
            averagePos /= noTargets;
        }
    

        DesiredPos = averagePos;
    }

}
