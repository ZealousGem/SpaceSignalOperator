using UnityEngine;

public class BlackHole : Star
{
    public float GravityStrength = 200f;
    public float GrabMaxDistance = 100f;
    public float GrabMinDistance = 0.5f;
    private Rigidbody ShipRb;

    protected override void Awake()
    {
        base.Awake();
        ShipRb = shipCoordinates.gameObject.GetComponent<Rigidbody>();
    }
    protected override void FixedUpdate()
    {
       base.FixedUpdate();
       GrabShip();
      
    }

    private void GrabShip()
    {
        Vector3 dir = transform.position - shipCoordinates.position;
        dir.y = 0f;
        float dist = dir.magnitude;

        if (dist > GrabMaxDistance) return;

     //   float clampedDist = Mathf.Max(dist, GrabMinDistance);
         float clampedDist = Mathf.Max(dist, GrabMinDistance);
        // Inverse square falloff
        float force = GravityStrength / clampedDist;

        Vector3 pull = new Vector3(dir.x , 0 , dir.z).normalized * force;

        ShipRb.AddForce(pull, ForceMode.Acceleration);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventBus.Act(new DamageShip(Damagedby.Blackhole, 100f));
        }
    }
}
