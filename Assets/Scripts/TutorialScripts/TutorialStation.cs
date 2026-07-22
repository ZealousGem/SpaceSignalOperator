using UnityEngine;

public class TutorialStation : Station
{
    public TutorialObject tutorialObject;
    private bool HasSeen = false; 
    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if (state && !HasSeen && tutorialObject != null)
        {
            EventBus.Act(new TutorialEvent(tutorialObject));
            HasSeen = true;
        }
    }
}
