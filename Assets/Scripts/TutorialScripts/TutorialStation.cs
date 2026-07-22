using UnityEngine;

public class TutorialStation : Station
{
    public TutorialObject tutorialObject;
    private bool HasSeen = false; 
    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if (state && !HasSeen)
        {
            HasSeen = true;
        }
    }
}
