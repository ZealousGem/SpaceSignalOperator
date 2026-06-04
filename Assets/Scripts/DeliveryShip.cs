using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public enum Damagedby
{


  NeutronStar,
  Blackhole,

  BurnUp,

  Default,

  Timer,

  OringalPlanet,

  FlewAway


}

public class DeliveryShip : ShipController
{

    public float ShipHealth = 100f; 
    public float ShipTemp = 0f;
    public GameObject Explosion;
    public GameObject Ship;
    public MeshRenderer ShipMaterial; 
    public VisualEffect burningVFX;
    
    private string OrangeHexidicaml = "#BF2C03";

    private string blueHecidicaml = "#18D4EA";

    private float intensity = 1f;

    private float dissolveRate = 0.0125f;

    private float refreshRate = 0.025f;

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
        if(isDead) return;

        ShipTemp += Burn;
        subject.TellObervers(new ShipTemp{amount = (int)ShipTemp});
      //  Debug.Log("ShipTemperature: " + ShipTemp +" "+ damagedby);

        if (ShipTemp < 100f) return;

        ShipTemp = 100f;
        isDead = true;

        ManageThrusters(0f);

        ShipSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        EventBus.Act(new StopObstacles(true));

        SetShipsDeathAnimation(damagedby); 

    }

    private void DamageShip(float Damage, Damagedby damagedby)
    {
        if(isDead) return;

        ShipHealth -= Damage;
        subject.TellObervers(new ShipHealth{amount = (int)Mathf.Max(0, ShipHealth)});
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
        if(damagedby != Damagedby.Blackhole) EventBus.Act(new StopObstacles(true));

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
         StartCoroutine(DissolveShip(dissolveRate, refreshRate, OrangeHexidicaml));
         EventBus.Act(new EndGameEvent(Damagedby.BurnUp, GameState.Fail));
    }

    private IEnumerator DissolveShip(float dissolveRate, float refreshRate, string hexColour)
    {
        if(ShipMaterial == null) yield break;

        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColour, out Color customColor))
        {
             float multiplier = Mathf.Pow(2, intensity);
            Color hdrColor = new Color(
                customColor.r * multiplier,
                customColor.g * multiplier,
                customColor.b * multiplier,
                customColor.a
            );

            for (int i = 0;  i < ShipMaterial.materials.Length; i++)
            {
                 ShipMaterial.materials[i].SetColor("_DissolveColour", hdrColor);
            }

           // Debug.Log("worked");
           
        }

        if (burningVFX != null)
        {
            burningVFX.Play();
        }

        float counter = 0;

        while (ShipMaterial.materials[0].GetFloat("_DissolveAmount") < 1)
        {
           counter+= dissolveRate;

            for (int i = 0;  i < ShipMaterial.materials.Length; i++)
            {
                ShipMaterial.materials[i].SetFloat("_DissolveAmount", counter);
            }
           
           yield return new WaitForSeconds(refreshRate); 
        }
    }

    private void ExplodeShip()
    {
         StartCoroutine(ExplosionEffect(1f));
         EventBus.Act(new EndGameEvent(Damagedby.Default, GameState.Fail));
    }
    
    protected IEnumerator ExplosionEffect(float duration)
    {
        Ship.SetActive(false);
        Explosion.SetActive(true);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // Normalize time (0 to 1)
            float t = timeElapsed / duration;

            yield return null; // Wait for next frame 
        }

        Explosion.SetActive(false);
        //Destroy(gameObject);
    }
    private void ShrinkShip()
    {
        Debug.Log("shrinking");
        StartCoroutine(ShrinkEffect(4f));
        EventBus.Act(new EndGameEvent(Damagedby.Blackhole, GameState.Fail));
    }

    private IEnumerator ShrinkEffect(float duration)
    {
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = new Vector3(0.002219783f, 0.002219783f, 0.005828707f);
        float Timer = 0f;

        while (Timer < duration)
        {
            Timer += Time.fixedDeltaTime;
            float t = Timer / duration;

            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        gameObject.transform.localScale = targetScale;

    }

    private void RetrunToOringialPlanet()
    {
        StartCoroutine(ShrinkEffect(5f));
        EventBus.Act(new EndGameEvent(Damagedby.OringalPlanet, GameState.Fail));
    }

    private void ObliterateShip()
    {
         StartCoroutine(DissolveShip(dissolveRate, refreshRate, blueHecidicaml));
         EventBus.Act(new EndGameEvent(Damagedby.NeutronStar, GameState.Fail));
    }
}
