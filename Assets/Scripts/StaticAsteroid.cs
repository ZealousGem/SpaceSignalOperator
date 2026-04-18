using System.Collections;
using UnityEngine;

public class StaticAsteroid : BaseObstacle
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 50f; 

    public float duration = 0f;

    public GameObject ExplosionEffect;  

    // Update is called once per frame
    protected virtual void FixedUpdate() => rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotationSpeed * Time.fixedDeltaTime, 0));

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.tag == "Player")
        {
            rotationSpeed = 0; 
            StartCoroutine(Explosion(duration));
        }
     
    }

    protected IEnumerator Explosion(float duration)
    {
        Object.SetActive(false);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // Normalize time (0 to 1)
            float t = timeElapsed / duration;

            yield return null; // Wait for next frame 
        }

        ExplosionEffect.SetActive(false);
        Destroy(gameObject);
    }


}
