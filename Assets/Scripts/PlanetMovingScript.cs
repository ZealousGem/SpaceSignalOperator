using UnityEngine;

public class PlanetMovingScript : MonoBehaviour
{
   public float rotationSpeed = 50f;
   private Rigidbody rb;
   [HideInInspector] public enum PlanetDirection  {x, y ,z};
   public PlanetDirection setDir;
  
   private Vector3 PlanetDir;
    protected  void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
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

    }

    protected void Update() => transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    protected void FixedUpdate() =>  rb.MoveRotation(rb.rotation * Quaternion.Euler(PlanetDir));
}
