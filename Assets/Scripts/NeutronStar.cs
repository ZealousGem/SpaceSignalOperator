using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class NeutronStar : Star
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     
    public float rotationSpeed = 100f;
    public GameObject Pulsar;
    public Collider PulsarCollider;  
    protected enum NeutronStarDirection  {x, y};
    protected NeutronStarDirection currentDir;
    private Vector3 NeutronStarDir; 
    private Vector3 MovingRotation;
    private float FinalTimer = 0; 
    private float currentTimer = 0;
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        RotateNeutronStar();
       
    }

    private void Update()
    {
        ActivatingPulsar();
    }

    protected override void Awake()
    {
            base.Awake();
            currentDir = (NeutronStarDirection)Random.Range(0, 2);
            rotationSpeed = Random.Range(rotationSpeed - 10f, rotationSpeed);
          
            SetNeutronStarRotationDirection(currentDir); 
            Pulsar.SetActive(false);   
        
    } 

    private void SetNeutronStarRotationDirection(NeutronStarDirection dir)
    {
        switch (currentDir)
        {
            case NeutronStarDirection.x: NeutronStarDir = new Vector3(rotationSpeed * Time.fixedDeltaTime, 0 ,0); break;
            case NeutronStarDirection.y: NeutronStarDir = new Vector3(0, rotationSpeed * Time.fixedDeltaTime ,0); break;
            
        }

        MovingRotation = NeutronStarDir;
    }

    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if(state is true)
        {
            NeutronStarDir =  MovingRotation; 
        }

        else
        {
            NeutronStarDir = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
    private void RotateNeutronStar() => rb.MoveRotation(rb.rotation * Quaternion.Euler(NeutronStarDir));

    private void ActivatingPulsar()
    {
        if(NeutronStarDir == Vector3.zero) return;

        currentTimer += Time.deltaTime;

        if(FinalTimer <= 0)FinalTimer = Random.Range(4f, 7f);

        if (FinalTimer <= currentTimer)
        {
            FinalTimer = 0;
            FinalTimer = 0;
            StartCoroutine(TurnOnPulsar(3f));
        }
    }

    private IEnumerator TurnOnPulsar(float duration)
    {
        
        Pulsar.SetActive(true);
        yield return new WaitForSeconds(duration);
        Pulsar.SetActive(false);
    }
    
}
