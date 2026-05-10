using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public enum Damagedby
{


  NeutronStar,
  Blackhole,

  BurnUp,

  Default,

  Timer,

  OringalPlanet


}

public class DeliveryShip : ShipController
{

    public float ShipHealth = 100f; 
    public float ShipTemp = 0f;
    public GameObject Explosion;
    public GameObject Ship;

    protected override void OnEnable()
    {
       base.OnEnable();
       EventBus.Subscribe<DamageShip>(RetrieveData);
       EventBus.Subscribe<StationEvent>(RetrieveData); 
    }

    protected override void OnDisable()
    {
       base.OnDisable();
       EventBus.Unsubscribe<DamageShip>(RetrieveData);
       EventBus.Unsubscribe<StationEvent>(RetrieveData);
    }

    private void RetrieveData(StationEvent data)
    {
        switch (data.action)
        {
            case StationType.Repairs: ShipHealth += data.amount; break;
            case StationType.ReactorCoolDown: ShipTemp -= data.amount; break;
            case StationType.Fuel: Fuel += data.amount; break;
        }
    }

    private void RetrieveData(DamageShip data)
    {
        //Debug.Log("here");
        if (data.action == Damagedby.BurnUp)
        {
            BurnShip(data.Damaged, data.action);
        }

        else
        {
            DamageShip(data.Damaged, data.action);    
        }
        
    }

    private void BurnShip(float Burn, Damagedby damagedby)
    {
        ShipTemp += Burn;
      //  Debug.Log("ShipTemperature: " + ShipTemp +" "+ damagedby);

        if (ShipTemp < 100f) return;

        ShipTemp = 100f;
        isDead = true;
        ManageThrusters(0f);
        ShipSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        SetShipsDeathAnimation(damagedby); 

    }

    private void DamageShip(float Damage, Damagedby damagedby)
    {
        ShipHealth -= Damage;
       // Debug.Log("ShipHealth: " + ShipHealth);

        if (ShipHealth > 0) return;

        ShipHealth = 0; 
        isDead = true; 
        ManageThrusters(0f);
        ShipSpeed = 0;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        SetShipsDeathAnimation(damagedby);  
        
    }

    private void SetShipsDeathAnimation(Damagedby damagedby)
    {
        switch (damagedby)
        {
            case Damagedby.Blackhole: ShrinkShip(); break;
            case Damagedby.NeutronStar: ObliterateShip(); break;
            case Damagedby.BurnUp: BurnShip(); break;
            case Damagedby.Default: ExplodeShip(); break;
            case Damagedby.OringalPlanet: RetrunToOringialPlanet(); break;
        }
    }

    private void BurnShip()
    {
         EventBus.Act(new EndGameEvent(Damagedby.BurnUp, GameState.Fail));
    }

    private void ExplodeShip()
    {
         
         EventBus.Act(new EndGameEvent(Damagedby.Default, GameState.Fail));
    }

    // private IEnumerator Explodsion()
    // {
    //     Ship.SetActive(false);

    // }

    private void ShrinkShip()
    {
        EventBus.Act(new EndGameEvent(Damagedby.Blackhole, GameState.Fail));
    }

    private void RetrunToOringialPlanet()
    {
        EventBus.Act(new EndGameEvent(Damagedby.OringalPlanet, GameState.Fail));
    }

    private void ObliterateShip()
    {
         EventBus.Act(new EndGameEvent(Damagedby.NeutronStar, GameState.Fail));
    }
}
