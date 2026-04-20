using System;
using UnityEngine;

public enum StationType {Repairs, ReactorCoolDown, Fuel};
public class Station : BaseObstacle
{
    public StationType station;
    public float amount;
    private bool PickedUp = false; 
    protected override void OnTriggerEnter(Collider other)
    {
        if(PickedUp) return;

        if (other.tag == "Player")
        {
            EventBus.Act(new StationEvent(station, amount));
            PickedUp = true;
        }

    }
}
