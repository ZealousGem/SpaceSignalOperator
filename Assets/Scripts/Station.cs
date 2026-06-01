using System;
using UnityEngine;

public enum StationType {Repairs, ReactorCoolDown, Fuel};
public class Station : BasePlanet
{
    public StationType station;
    public float amount;
    public GameObject StationOutLine;
    private bool PickedUp = false; 
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;

        if (!PickedUp)
        {
            EventBus.Act(new StationEvent(station, amount));
            StationOutLine.SetActive(false);
            PickedUp = true;
        }

        else
        {
             EventBus.Act(new DamageShip(Damagedby.OringalPlanet, 100f));
        }

    }
}
