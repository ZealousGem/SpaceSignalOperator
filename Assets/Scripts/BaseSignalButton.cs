using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSignalButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected virtual void OnMouseDown()
   {
      Debug.Log("Clicked");

   }


}
