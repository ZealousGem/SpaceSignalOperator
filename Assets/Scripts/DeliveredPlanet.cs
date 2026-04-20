using UnityEngine;

public class DeliveredPlanet : BaseObstacle
{
    protected  override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") EventBus.Act(new EndGameEvent(GameState.Success, false));
    }
}
