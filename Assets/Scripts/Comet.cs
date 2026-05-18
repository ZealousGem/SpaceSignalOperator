using UnityEngine;

public class Comet : MovingAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<StaticAsteroid>())
        {
            Debug.Log("Asteroid Hit");
            StaticAsteroid Asteroid = other.GetComponent<StaticAsteroid>();
            StartCoroutine(Asteroid.Explosion(Asteroid.duration));
        }
       
        base.OnTriggerEnter(other);

    }
}
