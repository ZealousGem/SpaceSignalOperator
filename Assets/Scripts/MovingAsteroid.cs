
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

        if (state is true)
        { 
            ShipTarget = new Vector3(shipCoordinates.position.x, 0, shipCoordinates.position.z);
            Vector3 direction = (ShipTarget - transform.position).normalized;
            rb.AddForce(direction * Speed, ForceMode.Impulse);
        } 

        else
        {
          rb.linearVelocity = Vector3.zero;  
        }


    }
    
}
