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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void OnMouseDown()
   {
      Debug.Log("Clicked");

   }


}
