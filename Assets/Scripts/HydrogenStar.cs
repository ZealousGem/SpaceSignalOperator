using UnityEngine;

public class HydrogenStar : Star
{

    public GameObject SolarFlare; 
    public float SolarFlareSpeed = 5f;

    public float SmallestNum = 3f;
    public float LargestNum = 4f;
    private float shootFlareTimer = 0f;
    private float FinalTimer = 0;
    private bool ShootShip = false; 

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ShootSolarFlares();
    }

    private void Update()
    {
        CheckDistance();
        SolarFlareTimer();
    }

     protected void ShootSolarFlares()
    {
 
        if(!ShootShip) return;
        
        GameObject temp = Instantiate(SolarFlare, transform.position, Quaternion.identity);
        Rigidbody r = temp.GetComponent<Rigidbody>();
        if(r == null) return;

        
        Vector3 direction = (shipCoordinates.position - gameObject.transform.position).normalized;
        r.AddForce(direction * SolarFlareSpeed, ForceMode.Impulse); //adds the force to create speed to the projectile once spawned 
        ShootShip = false;


    }

    protected void SolarFlareTimer()
    {

       if (!PlayerInRange) return;
       shootFlareTimer += Time.deltaTime;

       if (FinalTimer <= 0)FinalTimer = Random.Range(SmallestNum, LargestNum);

        if (FinalTimer <= shootFlareTimer)
        {
            ShootShip = true; 
            shootFlareTimer = 0;
            FinalTimer = 0;
        }
    }
}
