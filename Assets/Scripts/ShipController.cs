using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SignalDirections
{
    Right, 
    Left,
    Stop,
    Boost,

    Fire, 

    Move,   
}

public class ShipController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public float ShipSpeed = 3f;

    public float ShipRotationSpeed = 1f; 

    private bool isMoving = false;

    private Vector3 Movement = new Vector3(0,0,1); 

    private Transform ShipObject; 

    private float smoothMovement = 10f;

    private const float RotationTime = 1f;

    private Rigidbody rb; 
    
    private bool isRotating = false; 



    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ShipObject = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame

    protected virtual void RecieveSingals(SignalDirections dir)
    {
        switch (dir)
        {
            case SignalDirections.Left: if(isRotating) return; StartCoroutine(RotateShip(-0.4f, RotationTime)); break;
            case SignalDirections.Right:if(isRotating) return; StartCoroutine(RotateShip(0.4f, RotationTime)); break; 
            case SignalDirections.Stop: break;
            case SignalDirections.Move: break;
            default: break;
        }
    }

    private IEnumerator RotateShip(float amount, float duration)
    {
          isRotating = true;
          Movement.y = amount; 
          Quaternion startRotation = transform.rotation;
          Quaternion EndRotation = startRotation * Quaternion.Euler(0, Movement.y, 0);
          float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
             timeElapsed += Time.deltaTime;
             float t = timeElapsed / duration;
             transform.rotation = Quaternion.Slerp(startRotation, EndRotation, t);
             yield return null;

        }

        transform.rotation = EndRotation;
        isRotating = false; 
    }

    private IEnumerator ShipManageShipSpeed(float duration, float targetSpeed)
    {
        if(targetSpeed == ShipSpeed) yield break;

        float startSpeed = ShipSpeed;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // Normalize time (0 to 1)
            float t = timeElapsed / duration; 
            
            // Linear interpolation
            ShipSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            
            yield return null; // Wait for next frame 
        } 

        ShipSpeed = targetSpeed;

    }

    void FixedUpdate()
    {
       // if(!isMoving) return;
        MoveShip();
    }

    private void MoveShip()
    {
        Vector3 newDir = (ShipObject.forward * Movement.z).normalized;
        Vector3 curMovment = Vector3.zero;
        curMovment = Vector3.Lerp(curMovment, newDir, smoothMovement);
        rb.MovePosition(rb.position + curMovment * ShipSpeed * Time.fixedDeltaTime);
    }
    
}
