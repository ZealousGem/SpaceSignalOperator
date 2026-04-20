using UnityEngine;
using UnityEngine.Rendering;

public class Star : BaseObstacle
{
    public float SolarRayMaxDistance = 100f;
    public float SolarRayMinDistance = 0.5f;
    public float BurningStrenth = 5f;
    protected bool PlayerInRange = false; 

    protected virtual void FixedUpdate()
    {
        BurnShip();
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void BurnShip()
    {
        
        float dist = Vector3.Distance(transform.position, shipCoordinates.position);
        float ratio = dist / SolarRayMaxDistance;
        float damageScale = Mathf.Clamp01(1f - ratio);
        float burnAmount = BurningStrenth * damageScale;
        if(burnAmount > 0) EventBus.Act(new DamageShip(Damagedby.BurnUp, burnAmount));
       // Debug.Log($"Dist: {dist} | Max: {SolarRayMaxDistance} | Scale: {damageScale} | Final Burn: {burnAmount}");

        // burns player 
    }

    protected void CheckDistance()
    {
        Vector3 dir = transform.position - shipCoordinates.position;
        dir.y = 0f;
        float dist = dir.magnitude;

        PlayerInRange = dist <= SolarRayMaxDistance;
       // Debug.Log(PlayerInRange);
    }

   
}
