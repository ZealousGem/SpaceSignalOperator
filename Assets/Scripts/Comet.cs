using UnityEngine;

public class Comet : MovingAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out StaticAsteroid Asteroid)) Asteroid.KillAsteroid();
        base.OnTriggerEnter(other);

    }

  
}
