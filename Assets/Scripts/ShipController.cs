using System.Collections;
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

    private bool isMoving = true;

    private Vector3 Movement = new Vector3(0,0,1); 

    private Vector3 curMovment = Vector3.zero;

    private Transform ShipObject; 

    private float smoothMovement = 10f;

    private const float RotationTime = 1f;

    private Rigidbody rb; 
    
    private bool isRotating = false;

    protected virtual void OnEnable()
    {
         EventBus.Subscribe<setInput>(retriveInputSingal);
    }

    protected virtual void OnDisable()
    {
        EventBus.Unsubscribe<setInput>(retriveInputSingal);
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ShipObject = gameObject.GetComponent<Transform>();
    }

    private void retriveInputSingal(setInput data)=> RecieveSingals(data.action);

    protected virtual void RecieveSingals(SignalDirections dir)
    {
        switch (dir)
        {
            case SignalDirections.Left: if(isRotating) return; StartCoroutine(RotateShip(-45f, RotationTime)); break;
            case SignalDirections.Right:if(isRotating) return; StartCoroutine(RotateShip(45f, RotationTime)); break; 
            case SignalDirections.Stop: StartCoroutine(ManageShipSpeed(0.6f, 0f)); isMoving = false; break;
            case SignalDirections.Move: StartCoroutine(ManageShipSpeed(0.3f, 3f)); isMoving = true; break;
            default: break;
        }
    }

    private IEnumerator RotateShip(float amount, float duration)
    {
          //Debug.Log("rotating");
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

    private IEnumerator ManageShipSpeed(float duration, float targetSpeed)
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
        if(!isMoving) return;
        MoveShip();
    }

    private void MoveShip()
    {
        Vector3 newDir = (ShipObject.forward * Movement.z).normalized;
        curMovment = Vector3.Lerp(curMovment, newDir, smoothMovement);
        rb.MovePosition(rb.position + curMovment * ShipSpeed * Time.fixedDeltaTime);
    }
    
}
