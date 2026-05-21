using System.Collections;
using UnityEngine;

public class FuelGauge : MonoBehaviour
{

    public float minArrowAngle;
    public float maxArrowAngle;
    public RectTransform arrow;
    public float duration = 0.6f;  

    public void StartMovement(float amount) => StartCoroutine(transformArrow(amount));
   
    private IEnumerator transformArrow(float amount)
    {
       
       float fuelPercentage = Mathf.InverseLerp(0, 200, amount);
       float targetZRotation = Mathf.Lerp(minArrowAngle, maxArrowAngle, fuelPercentage);
       float currentEuler = arrow.localEulerAngles.z;
       
       float startTime = 0f;

        while (startTime < duration)
        {
            startTime += Time.deltaTime;
            float T = startTime/duration;

            T = Mathf.SmoothStep(0f, 1f, T);
  
            float newZ = Mathf.LerpAngle(currentEuler, targetZRotation, T);
            arrow.localEulerAngles = new Vector3(0, 0, newZ);
            
            yield return null;
        }
         
       arrow.localEulerAngles = new Vector3(0, 0, targetZRotation);
           
        
    }
}
