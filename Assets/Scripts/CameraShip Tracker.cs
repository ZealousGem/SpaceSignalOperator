using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShipTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private float DampTime = 0.2f;
   public  List<Transform> targets; 
   public Camera cam;
   private Vector3 Velocity;
   private Vector3 DesiredPos;
   private const float cameraHeight = 35.53f;

   private const float SpacshipDistance = -5.8f;

   Vector3 camDistance = new Vector3(-29.16f, cameraHeight, SpacshipDistance);

    private void OnEnable()
    {
        EventBus.Subscribe<DamageShip>(RetrieveData);
    }

    private void OnDisable()
    {
         EventBus.Unsubscribe<DamageShip>(RetrieveData);
    }

    private void RetrieveData(DamageShip damageShip)
    {
        if (damageShip.Damaged > 0 && 
            damageShip.action != Damagedby.Blackhole && 
            damageShip.action != Damagedby.FlewAway && 
            damageShip.action != Damagedby.OringalPlanet && 
            damageShip.action != Damagedby.BurnUp)
        {
           StartCoroutine(ShakeCamera(0.3f, 0.3f));    
        }
       
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
        if (cam == null) yield break;

        Vector3 oriPos = cam.gameObject.transform.localPosition;

        float timer = 0.0f;

        while (timer < timeDuration)
        {
           // float x = Random.Range(-1f, 1f) * ShakeAmount;
            float y = Random.Range(-1f, 1f) * ShakeAmount;

            cam.gameObject.transform.localPosition = new Vector3(oriPos.x, y, oriPos.z);

            timer += Time.deltaTime;

            yield return null;
        }

        cam.gameObject.transform.localPosition = oriPos;
       
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
