using UnityEngine;

public class DeliveryPlanetOrigin : BaseObstacle
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created   
    protected  override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") EventBus.Act(new EndGameEvent(GameState.Fail, true));
    }
}
