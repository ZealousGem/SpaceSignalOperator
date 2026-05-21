
using UnityEngine;

public class MovingAsteroid : StaticAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Speed = 70f;

    private Vector3 ShipTarget;

    // Update is called once per frame

    protected override void ToggleVisibility(bool state)
    {
       base.ToggleVisibility(state);
       if(gameObject.name == "Comet")Debug.Log(state);
        if (state is true)
        {  
           // if(gameObject.name == "Comet")Debug.Log(state);
            ShipTarget = new Vector3(shipCoordinates.position.x, 0, shipCoordinates.position.z);
            Vector3 direction = (ShipTarget - transform.position).normalized;
            rb.AddForce(direction * Speed, ForceMode.Impulse);
           
        }


    }

    protected override void OnTriggerEnter(Collider other)
    {

        base.OnTriggerEnter(other);
        
        if (other.gameObject.tag == "Player")
        {
            Speed = 0;
            rb.linearVelocity = Vector3.zero;  
        } 

        
    }
    
}
