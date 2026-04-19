using UnityEngine;

public class Comet : MovingAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        
        if(other.GetComponent<StaticAsteroid>())
        {
            StaticAsteroid Asteroid = other.GetComponent<StaticAsteroid>();
            StartCoroutine(Explosion(Asteroid.duration));
        }
       

    }
}
