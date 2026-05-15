using UnityEngine;

public class DeliveredPlanet : BasePlanet
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") EventBus.Act(new EndGameEvent(GameState.Success, false));
    }
}
