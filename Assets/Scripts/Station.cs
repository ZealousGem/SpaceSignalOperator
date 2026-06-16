using System;
using UnityEngine;

public enum StationType {Repairs, ReactorCoolDown, Fuel};
public class Station : BasePlanet
{
    public StationType station;
    public float amount;
    public GameObject StationOutLine;
    private bool PickedUp = false; 

    private const float ScaleSize = 6.92f;
    private SphereCollider StationCollider;
    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if (state && !PickedUp) EventBus.Act(new WarningTextEvent(UITextInfo.StationImage, this));

        if (state && StationCollider == null)StationCollider = gameObject.GetComponent<SphereCollider>();
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;

        if (!PickedUp)
        {
            EventBus.Act(new StationEvent(station, amount));
            StationOutLine.SetActive(false);
            StationCollider.radius = ScaleSize;
            PickedUp = true;
        }

        else
        {
             EventBus.Act(new DamageShip(Damagedby.OringalPlanet, 100f));
        }

    }
}
