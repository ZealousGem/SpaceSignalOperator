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

None


}

public class BaseSignalButton : MonoBehaviour
{ 
    public ButtonAnimations buttonType; 

    private  const float OriginalPos = 0.05106735f;

    private const float LittleDownOrignalPos = 0.02f;

    private const float DownPos = -0.14f;

    private const float LittleUpDownPos = -0.07f;

    private GameState gameState = GameState.Start;
    
    void OnEnable()
    {
        EventBus.Subscribe<EndGameEvent>(SetGameState);
        EventBus.Subscribe<ButtonEvent>(RetrieveData); 
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<ButtonEvent>(RetrieveData);
        EventBus.Unsubscribe<EndGameEvent>(SetGameState);
    }

    private void SetGameState(EndGameEvent gameState) => this.gameState = gameState.GameEvent;

    private void RetrieveData(ButtonEvent data)
   {
      if(Time.timeScale == 0f || gameState != GameState.Ongoing) return;

      if (data.action ==buttonType)
      {
         SoundPlayer.PlaySound("SignalButtonIn");
         StartCoroutine(ButtonUpSequence());
      }
      //Debug.Log(gameObject.name + "pressed");
   }

   
   protected void OnMouseDown()
   {
      if(Time.timeScale == 0f || gameState != GameState.Ongoing) return;

      SoundPlayer.PlaySound("SignalButtonOut");
      StartCoroutine(ButtonDownSequence());
     // Debug.Log(gameObject.name + "pressed");
      
      ActivateButton();

   }

   protected virtual void ActivateButton(){}

   private IEnumerator ButtonUpSequence()
   {
      yield return StartCoroutine(MoveButton(0.3f, OriginalPos));
      yield return StartCoroutine(MoveButton(0.01f, LittleDownOrignalPos));
      yield return StartCoroutine(MoveButton(0.07f, OriginalPos));
   }

   private IEnumerator ButtonDownSequence()
   {
    yield return StartCoroutine(MoveButton(0.2f, DownPos));
    yield return StartCoroutine(MoveButton(0.02f, LittleUpDownPos));
    yield return StartCoroutine(MoveButton(0.01f, DownPos));
   
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
