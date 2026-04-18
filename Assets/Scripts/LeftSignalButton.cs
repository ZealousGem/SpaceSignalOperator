using UnityEngine;
using UnityEngine.InputSystem;

public class LeftSignalButton : BaseSignalButton
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnMouseDown()
    {
         EventBus.Act(new setInput(SignalDirections.Left));
         //Debug.Log("clicked");
    }
}
