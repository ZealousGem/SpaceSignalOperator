using System.Collections;
using Unity.Collections;
using UnityEngine;


public class StaticAsteroid : BaseObstacle
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 50f; 

    public float duration = 0f;

    public GameObject ExplosionEffect;

    protected enum AsteroidDirection  {x, y ,z};
 
    protected AsteroidDirection currentDir; 

    private Vector3 AsteroidDir; 

    private Vector3 MovingRotation;

    protected bool ParentDirection = true; 

    protected override void Awake()
    {
        base.Awake();
       
        if(ParentDirection)
        {
            rotationSpeed = Random.Range(rotationSpeed - 10f, rotationSpeed);
            currentDir = (AsteroidDirection)Random.Range(0, (int)AsteroidDirection.z);
            SetAsteroidDirection(currentDir);
        }
        
    }

    protected void SetAsteroidDirection(AsteroidDirection currentDir)
    {
        switch (currentDir)
        {
            case AsteroidDirection.x: AsteroidDir = new Vector3(rotationSpeed * Time.fixedDeltaTime, 0 ,0); break;
            case AsteroidDirection.y: AsteroidDir = new Vector3(0, rotationSpeed * Time.fixedDeltaTime ,0); break;
            case AsteroidDirection.z: AsteroidDir = new Vector3(0, 0 , rotationSpeed * Time.fixedDeltaTime); break;
        }

        MovingRotation = AsteroidDir;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate() => rb.MoveRotation(rb.rotation * Quaternion.Euler(AsteroidDir));

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.tag == "Player")
        {
            rotationSpeed = 0; 
            StartCoroutine(Explosion(duration));
        }
     
    }

    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if(state is true)
        {
            AsteroidDir =  MovingRotation; 
        }

        else
        {
            AsteroidDir = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
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
