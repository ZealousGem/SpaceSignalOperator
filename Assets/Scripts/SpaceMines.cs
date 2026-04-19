using UnityEngine;

public class SpaceMines : StaticAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        ParentDirection = false;
        base.Awake();
        currentDir = AsteroidDirection.y;
        SetAsteroidDirection(currentDir);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.tag == "Missile") StartCoroutine(Explosion(duration)); // will add points or something idk 
        if(other.GetComponent<StaticAsteroid>()) StartCoroutine(Explosion(duration));
       
    }
}
