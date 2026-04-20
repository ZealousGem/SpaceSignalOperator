using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonAnimations
{
    
LeftButton,

RightButton,

StopButton, 

BoostButton, 

MissileButton, 

ShieldButton, 


}

public class BaseSignalButton : MonoBehaviour
{ 
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    private  const float OriginalPos = 0.05106735f;

    private const float LittleDownOrignalPos = 0.02f;

    private const float DownPos = -0.14f;

    private const float LittleUpDownPos = -0.07f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected virtual void OnMouseDown()
   {
      StartCoroutine(ButtonSequence());

   }

   private IEnumerator ButtonSequence()
   {
    // 'yield return' tells the code to wait until the MoveButton Coroutine finishes
    yield return StartCoroutine(MoveButton(0.2f, DownPos));
    yield return StartCoroutine(MoveButton(0.02f, LittleUpDownPos));
    yield return StartCoroutine(MoveButton(0.01f, DownPos));
    yield return StartCoroutine(MoveButton(0.3f, OriginalPos));
    yield return StartCoroutine(MoveButton(0.01f, LittleDownOrignalPos));
    yield return StartCoroutine(MoveButton(0.07f, OriginalPos));
   }

   private IEnumerator MoveButton(float duration, float endCoordinate)
   {
      float timeElapsed = 0f;
      Vector3 StartPos = transform.localPosition;
      Vector3 EndPos = new Vector3(transform.localPosition.x, endCoordinate, transform.localPosition.z);

       while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // Normalize time (0 to 1)
            float t = timeElapsed / duration;
            t = t * t * (3f - 2f * t);
            
            // Move object using Vector3.Lerp
            transform.localPosition = Vector3.Lerp(StartPos, EndPos, t);

            yield return null; // Wait for next frame 
        }

        transform.localPosition = EndPos;
   }

   


}
