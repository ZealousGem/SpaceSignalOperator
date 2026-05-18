using System.Collections;
using UnityEngine;

public class SolarFlare : Pulsar
{
    
    public GameObject Object;
    public GameObject ExplosionEffect;
    public Rigidbody rb;
    private const float duration = 1f;
    private float MaxCounter = 5f;
    private float Counter = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        
        base.OnTriggerEnter(other);
        if (other.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector3.zero; 
            StartCoroutine(Explosion(duration));
        }

    }

     public IEnumerator Explosion(float duration)
    {
        Object.SetActive(false);
        ExplosionEffect.SetActive(true);
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

     void Update()
    {
        Counter += Time.deltaTime; // once counter has been reached bomb will despawn to save peformance, this is only done if the turret or enemy miises their shot 

        if (Counter >= MaxCounter)
        {
            Destroy(gameObject);    
        }
    }
}
