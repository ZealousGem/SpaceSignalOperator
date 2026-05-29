using UnityEngine;

public class DeliveryPlanetOrigin : BasePlanet
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created   
    protected  override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
             EventBus.Act(new DamageShip(Damagedby.OringalPlanet, 100f));
        }
    }
}
