using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    private bool isVisible = true;

    protected virtual void Awake()
    {
        collider = gameObject.GetComponent<Collider>();
        shipCoordinates = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        InvokeRepeating(nameof(CheckDistance), 0f, checkInterval);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // damages space ship 
        }
    }

    private void CheckDistance()
    {
        float dist = Vector3.Distance(transform.position,  shipCoordinates.position);

        if (dist > renderDistance && isVisible)
        {
            ToggleVisibility(false);
        }
        else if (dist <= renderDistance && !isVisible)
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
