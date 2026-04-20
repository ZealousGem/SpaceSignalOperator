using UnityEngine;

public class Pulsar : MonoBehaviour
{ 

    public float Damage = 50f;
    public Damagedby action;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
             EventBus.Act(new DamageShip(action, Damage));
        }
    }
}
