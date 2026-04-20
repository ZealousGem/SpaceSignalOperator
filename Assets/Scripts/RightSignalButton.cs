using UnityEngine;

public class RightSignalButton : BaseSignalButton
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnMouseDown()
    {
       base.OnMouseDown();
       EventBus.Act(new setInput(SignalDirections.Right));
    }
}
