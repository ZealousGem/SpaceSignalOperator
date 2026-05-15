using UnityEngine;

public class BasePlanet : BaseObstacle
{
   public float rotationSpeed = 50f;
   private const float lockY = 0.016704f;
   private Vector3 PlanetDir;
   protected enum PlanetDirection  {x, y ,z};
   private Vector3 MovingRotation;

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

    private void Update()
    {
       transform.position = new Vector3(transform.position.x, lockY, transform.position.z); 
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
