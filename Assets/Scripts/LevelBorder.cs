using UnityEngine;

public enum Border{ innerBorder, outerBorder}

public class LevelBorder : MonoBehaviour
{
    public Border selectedBorder;

    bool PassedBorder = false;

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" || PassedBorder) return;

        switch (selectedBorder)
        {
            case Border.innerBorder: EventBus.Act(new WarningTextEvent(UITextInfo.MapWarning)); break;
            case Border.outerBorder: EventBus.Act(new DamageShip(Damagedby.FlewAway, 100f)); PassedBorder = true;break; 
        }
    }
}
