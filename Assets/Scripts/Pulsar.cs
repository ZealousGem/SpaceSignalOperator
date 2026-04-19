using UnityEngine;

public class Pulsar : MonoBehaviour
{ 

    public float Damage = 50f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
             EventBus.Act(new DamageShip(Damagedby.NeutronStar, Damage));
        }
    }
}
