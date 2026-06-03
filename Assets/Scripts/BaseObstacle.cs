using System;
using UnityEngine;

public class BaseObstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float Damage;
   [NonSerialized] public Transform shipCoordinates;  
   [NonSerialized] public new Collider collider;  
   [NonSerialized] public Rigidbody rb;
    public GameObject Object;
    public float renderDistance = 20f;
    public float checkInterval = 0.5f;
    private bool isVisible = false;

    protected virtual void Awake()
    {
        collider = gameObject.GetComponent<Collider>();
        shipCoordinates = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public virtual void InitialCheck()
    {
    float dist = Vector3.Distance(transform.position, shipCoordinates.position);
    
    bool renderObject = dist <= renderDistance;

    isVisible = !renderObject;
    ToggleVisibility(renderObject);
    
   }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // damages space ship 
            EventBus.Act(new DamageShip(Damagedby.Default, Damage));
        }
    }

    public void CheckDistance(float dist)
    {
         float renderDistanceSqr = renderDistance * renderDistance;
        
        if (dist > renderDistanceSqr && isVisible)
        {
            ToggleVisibility(false);
        }
        else if (dist <= renderDistanceSqr && !isVisible)
        {
            ToggleVisibility(true);
        }
    }

    protected virtual void ToggleVisibility(bool state)
    {
        isVisible = state;
        Object.SetActive(state);
        collider.enabled = state;
    }

}
