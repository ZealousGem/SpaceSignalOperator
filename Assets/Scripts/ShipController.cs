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

    public float Fuel = 100f;

    protected bool isMoving = true;

    private Vector3 Movement = new Vector3(0,0,1); 

    private Transform ShipObject; 

    private const float RotationTime = 1f;

    private Rigidbody rb; 
    
    private bool isRotating = false;

    private bool noMoreFuel = false; 

    protected virtual void OnEnable()
    {
         EventBus.Subscribe<setInput>(retriveInputSingal);
    }

    protected virtual void OnDisable()
    {
        EventBus.Unsubscribe<setInput>(retriveInputSingal);
    }

    private void Awake()
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
             rb.MoveRotation(transform.rotation);
             yield return null;

        }

        rb.MoveRotation(EndRotation);
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
            Fuel -= 0.1f * Time.fixedDeltaTime;
            // Linear interpolation
            ShipSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity.normalized * ShipSpeed, t);
            
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
    Vector3 forward = ShipObject.forward;
       forward.y = 0f;
       forward.Normalize();

       Vector3 targetVelocity = forward * ShipSpeed;

    // Smooth acceleration instead of teleporting
      Vector3 velocityChange = targetVelocity - rb.linearVelocity;

      velocityChange.y = 0f; // 🔒 lock Y

      rb.AddForce(velocityChange, ForceMode.Acceleration);

      if(velocityChange != Vector3.zero) DecreaseShipFuel();
    }

    private void DecreaseShipFuel()
    {
         if(noMoreFuel) return;

        Fuel -= 0.1f * Time.fixedDeltaTime;

        if (Fuel <= 0)
        {
            Fuel = 0;
            isMoving = false;
            noMoreFuel = true;
            
        }
    }
    
}
