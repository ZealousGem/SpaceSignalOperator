using JetBrains.Annotations;
using UnityEngine;

public enum Damagedby
{


  NeutronStar,
  Blackhole,

  BurnUp,

  Default,

  Timer


}

public class DeliveryShip : ShipController
{

    public float ShipHealth = 100f; 
    public float ShipTemp = 0f;
    public GameObject Explosion;

    protected override void OnEnable()
    {
       base.OnEnable();
       EventBus.Subscribe<DamageShip>(RetrieveData); 
    }

    protected override void OnDisable()
    {
       base.OnDisable();
       EventBus.Unsubscribe<DamageShip>(RetrieveData);
    }

    private void RetrieveData(DamageShip data)
    {
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

        if (ShipTemp < 100) return;

        ShipTemp = 100;
        SetShipsDeathAnimation(damagedby); 

    }

    private void DamageShip(float Damage, Damagedby damagedby)
    {
        ShipHealth -= Damage;

        if (ShipHealth > 0) return;

        ShipHealth = 0; 
        isMoving = false;
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

    private void ShrinkShip()
    {
        EventBus.Act(new EndGameEvent(Damagedby.Blackhole, GameState.Fail));
    }

    private void ObliterateShip()
    {
         EventBus.Act(new EndGameEvent(Damagedby.NeutronStar, GameState.Fail));
    }
}
