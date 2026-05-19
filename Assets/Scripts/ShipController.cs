using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    public List<ParticleSystem> ShipThrusters; 

    public UIObersver subject;

    private bool isMoving = true;

    protected bool isDead = false;

    private Vector3 Movement = new Vector3(0,0,1); 

    private const float RotationTime = 1.2f;

    protected Rigidbody rb; 

     private const float ThrusterSize = 1.5f;

    private float currentThrust = ThrusterSize;
    
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
    }

    private void retriveInputSingal(setInput data)=> RecieveSingals(data.action);

    protected virtual void RecieveSingals(SignalDirections dir)
    {
        if(isDead) return;
        switch (dir)
        {
            case SignalDirections.Left: if(isRotating) return; StartCoroutine(RotateShip(-45f, RotationTime, ButtonAnimations.LeftButton, 25f)); break;
            case SignalDirections.Right:if(isRotating) return; StartCoroutine(RotateShip(45f, RotationTime, ButtonAnimations.RightButton, -25f));break; 
            case SignalDirections.Stop: StartCoroutine(ManageShipSpeed(0.6f, 0f, 0f)); isMoving = false; break;
            case SignalDirections.Move: StartCoroutine(ManageShipSpeed(0.3f, 3f, ThrusterSize)); isMoving = true; break;
            default: break;
        }
    }

    

    private IEnumerator RotateShip(float amount, float duration, ButtonAnimations button, float ShipRoatationAmount)
    {
          //Debug.Log("rotating");
          isRotating = true;
          Movement.y = amount; 
          Movement.z = ShipRoatationAmount;

           Quaternion startRotation = transform.rotation;
           Quaternion EndRotation = startRotation * Quaternion.Euler(0, amount, 0);
           //Quaternion EndRotation = startRotation * Quaternion.Euler(0, Movement.y, 0) * Quaternion.Euler(0,0, Movement.z);

          float timeElapsed = 0f;
          
          float currentTilt = 0f;

        while (timeElapsed < duration)
        {
             timeElapsed += Time.deltaTime;
             float t = timeElapsed / duration;
             Quaternion rotate = Quaternion.Slerp(startRotation, EndRotation, t); 
             
             if(t <= 0.5f) currentTilt = Mathf.Lerp(0f, ShipRoatationAmount, t*2f);
             else currentTilt = Mathf.Lerp(ShipRoatationAmount, 0f, (t - 0.5f)* 2f);
             
             Quaternion currentBank = Quaternion.Euler(0, 0, currentTilt);
             rb.MoveRotation(rotate * currentBank);
             yield return null;

        }

        rb.MoveRotation(EndRotation);
        isRotating = false; 
        EventBus.Act(new ButtonEvent(button)); 
    }

    protected void ManageThrusters(float amount)
    {
        for (int i = 0; i < ShipThrusters.Count; i++)
    {
        var mainModule = ShipThrusters[i].main;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(amount);
    }
    }

    private IEnumerator ManageShipSpeed(float duration, float targetSpeed, float targetThrust)
    {
        if(targetSpeed == ShipSpeed)yield break;    

        float startSpeed = ShipSpeed;
        float startThrust = currentThrust;
        float timeElapsed = 0f;


        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // Normalize time (0 to 1)
            float t = timeElapsed / duration; 
            // Linear interpolation
            ShipSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            currentThrust = Mathf.Lerp(startThrust, targetThrust, t);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity.normalized * ShipSpeed, t);
            ManageThrusters(currentThrust);
            
            yield return null; // Wait for next frame 
        } 

       
        ShipSpeed = targetSpeed;
        currentThrust = targetThrust;
       // Debug.Log(ShipSpeed);
        EventBus.Act(new ButtonEvent(ButtonAnimations.StopButton));

    }

    void FixedUpdate()
    {
        if(isDead) return;
        if(!isMoving) return;
        MoveShip();
    }

    private void MoveShip()
    {

      Vector3 forward = transform.forward;
      forward.y = 0f;
      forward.Normalize();

   //   Vector3 velocityChange = targetVelocity - rb.linearVelocity;

      rb.linearVelocity = forward * ShipSpeed;;

      if(ShipSpeed > 0.1f) DecreaseShipFuel();
    }

    private void DecreaseShipFuel()
    {
         if(noMoreFuel) return;

        Fuel -= 0.1f * Time.fixedDeltaTime;

        if (Fuel <= 0)
        {
            Fuel = 0;
            ManageThrusters(0f);
            isMoving = false;
            noMoreFuel = true;
            
        }

        subject.TellObervers(new ShipFuel{amount = (int)Fuel});
    }
    
}
