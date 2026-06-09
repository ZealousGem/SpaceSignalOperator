using System.Collections;
using System.Collections.Generic;
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

   private const float cameraHeight = 35.53f;

   private const float SpacshipDistance = -5.8f;

   Vector3 camDistance = new Vector3(-29.16f, cameraHeight, SpacshipDistance);

    

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

    private IEnumerator ShakeCamera(float ShakeAmount, float timeDuration)
    {
        Vector3 oriPos = transform.localPosition;

        float timer = 0.0f;

        while (timer < timeDuration)
        {
            float x = Random.Range(-1f, 1f) * ShakeAmount;
            float y = Random.Range(-1f, 1f) * ShakeAmount;

            transform.localPosition = new Vector3(x, y, oriPos.z);

            timer += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = oriPos;
       
    }

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
