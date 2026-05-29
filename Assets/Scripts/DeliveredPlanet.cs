using UnityEngine;

public class DeliveredPlanet : BasePlanet
{

    private bool isDelivered = false; 

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        if (!isDelivered)
        {
           EventBus.Act(new EndGameEvent(GameState.Success, false));
           isDelivered = true;    
        }

        else
        {
             EventBus.Act(new DamageShip(Damagedby.OringalPlanet, 100f));
        }
        

    }
}
