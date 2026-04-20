using UnityEngine;

public class SolarFlare : Pulsar
{

    private float MaxCounter = 5f;

    private float Counter = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        
        base.OnTriggerEnter(other);

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
