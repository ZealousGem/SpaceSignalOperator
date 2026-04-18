using UnityEngine;

public class ManageSpeedButton : BaseSignalButton
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isMoving = true;
    protected override void OnMouseDown()
    {

        if (isMoving)
        {
             EventBus.Act(new setInput(SignalDirections.Stop));
             isMoving = false;
        }

        else
        {
             EventBus.Act(new setInput(SignalDirections.Move));
             isMoving = true;
        }
        
    }
}
