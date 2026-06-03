using System;
using UnityEngine;

public class BasePlanet : BaseObstacle
{
   public float rotationSpeed = 50f;
   [HideInInspector] public enum PlanetDirection  {x, y ,z};
   public PlanetDirection setDir;
   private const float lockY = 0.016704f;
   private Vector3 PlanetDir;
   private Vector3 MovingRotation;

    protected override void Awake()
    {
        base.Awake();
        SetDirection(setDir);
    }

    protected void SetDirection(PlanetDirection currentDir)
    {
        switch (currentDir)
        {
            case PlanetDirection.x: PlanetDir = new Vector3(rotationSpeed * Time.fixedDeltaTime, 0 ,0); break;
            case PlanetDirection.y: PlanetDir = new Vector3(0, rotationSpeed * Time.fixedDeltaTime ,0); break;
            case PlanetDirection.z: PlanetDir = new Vector3(0, 0 , rotationSpeed * Time.fixedDeltaTime); break;
        }

        MovingRotation = PlanetDir;
    }

    protected void Update() => transform.position = new Vector3(transform.position.x, lockY, transform.position.z);

    protected void FixedUpdate() =>  rb.MoveRotation(rb.rotation * Quaternion.Euler(PlanetDir));

    public override void InitialCheck()
    {
        base.InitialCheck();

        if (Object.activeSelf is false)
        {
             PlanetDir= Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
   
    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);

        if(state is true)
        {
            PlanetDir =  MovingRotation; 
        }

        else
        {
            PlanetDir= Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
