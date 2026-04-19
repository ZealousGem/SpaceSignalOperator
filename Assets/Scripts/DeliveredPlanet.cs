using UnityEngine;

public class DeliveredPlanet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") EventBus.Act(new EndGameEvent(GameState.Success, false));
        
    }
}
