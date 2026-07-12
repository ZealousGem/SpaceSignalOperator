using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum UITextInfo{PlanetText, AsteroidText, Sun, CometText, StationImage, Fuel, Temp, Repairs, PlanetLeftText, MapWarning, Counter}

public class UIManager : MonoBehaviour, IObserver
{
    public UIObersver subject;
    public TMP_Text ShipHealth;
    public TMP_Text ShipTemperature;
    public TMP_Text LightYearsScale;
    public TMP_Text PlanetsLeft;
    public TMP_Text Counter;
    public RectTransform WarningText;
    public RectTransform StationText;
    public TMP_Text DeliveredtoPlanetText;
    public Image Arrow;
    public FuelGauge guage;
    public Image TempImage;
    public Image StaticScreen;
    private readonly float maxAlhpa = 7f / 255f;

    private void OnEnable() => subject.AddObersver(this); 

    private void OnDisable() => subject.RemoveObserver(this);

    public void onNotify<T>(T notificationData)
    {
        switch (notificationData)
        {
            case ShipFuel fuel: guage.StartMovement(fuel.amount); break;
            case ShipHealth health: ShipHealth.text = health.amount.ToString() +"%"; break;
            case ShipTemp temp: setTemperature(temp.amount); break;
            case PlanetPosDirection planet: DisplayPlanetDirection(planet.amount, planet.Direction); break;
            case EvokeSpawnScreen spawnStaticScreen: StartCoroutine(StaticScreenTimer(spawnStaticScreen.timer, spawnStaticScreen.action)); break; 
            case UIinformation information: ExtractUItext(information); break;
        }
    }

    private IEnumerator StaticScreenTimer(float counter, bool state)
    {
        yield return new WaitForSeconds(counter);
        ActivateStaticScreen(state);
    } 

    private void ActivateStaticScreen(bool state)
    {
       
        if (state)
        {
             Vector3 orignalScale = StaticScreen.gameObject.transform.localScale;
             StaticScreen.gameObject.transform.localScale = Vector3.zero;

             StaticScreen.gameObject.SetActive(true);
             StaticScreen.gameObject.transform.DOKill();

             float duration = 0.2f;
             StaticScreen.gameObject.transform.DOScale(orignalScale,duration).From(Vector3.zero).SetEase(Ease.OutBack); 

             SoundPlayer.PlaySound("Static");
            // Debug.Log("transition");    
        }
     
        else
        {
            StaticScreen.gameObject.SetActive(false);
        }

    }

    private IEnumerator StartDelivery()
    {
        yield return new WaitForSeconds(0.5f);
        float timer = 3;
        int lastDisplayedTime = -1;
        

        while (timer > 0)
        {

            timer -= Time.deltaTime;
            int currentTime = Mathf.CeilToInt(timer);

            if (currentTime != lastDisplayedTime)
            {
                lastDisplayedTime = currentTime;
                setCounterAnimation(currentTime);
            }
            
            yield return null;
        }
         
         EventBus.Act(new EndGameEvent(GameState.Ongoing));
       
    }

    private void setCounterAnimation(int currentTime)
    {
        if (currentTime != 0)
        {
             SoundPlayer.PlaySound("Counter");
             StartCoroutine(EvokeText(Counter, currentTime.ToString(), 0.5f));
        }

        else
        {
             SoundPlayer.PlaySound("Counter");
             StartCoroutine(EvokeText(Counter, "go!", 0.5f));
        }
       
    }
    

    private void DisplayPlanetDirection(float pos, Vector3 dir)
    {
        int displayLightYears = (int)pos;
        LightYearsScale.text = displayLightYears.ToString() +" AU to Delivery";

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;



        Arrow.rectTransform.localRotation = Quaternion.Euler(0,0, angle);

    }

    private void setTemperature(float amount)
    {
        int roundedAmount = (int)amount;
        ShipTemperature.text = roundedAmount.ToString() +"c";

        float Percentage = Mathf.InverseLerp(0, 100, amount);
        float targetAmount = Mathf.Lerp(0, maxAlhpa, Percentage);

        Color colour = TempImage.color;
        colour.a = targetAmount;

        TempImage.color = colour;
    }
    private void Start()
    {
        ActivateStaticScreen(false);

        Color colour = TempImage.color;
        colour.a = 0f;
        TempImage.color = colour;

        ShipHealth.text =100.ToString()+"%";
        ShipTemperature.text =0.ToString() +"c";

      //  StartCoroutine(StartDelivery());
    }

    private void ExtractUItext(UIinformation data)
    {
      //  Debug.Log(data.info);
        switch (data.info)
        {
            case UITextInfo.PlanetText: SoundPlayer.PlaySound("PackageDevilvered"); PlanetsLeft.text = "Planets Left " +  data.count.ToString(); StartCoroutine(EvokeText(DeliveredtoPlanetText, "Package Delivered, Next Planet Coordinates are in", 3f)); break;

            case UITextInfo.AsteroidText: SoundPlayer.PlaySound("AsteroidSignal"); EvokeTextSeq(data.info, WarningText, "Asteroid Incoming", 0.5f, data.Direction); break;

            case UITextInfo.CometText: SoundPlayer.PlaySound("AsteroidSignal"); EvokeTextSeq(data.info, WarningText, "Comet Incoming", 0.5f, data.Direction); break;

            case UITextInfo.Sun: EvokeTextSeq(data.info, WarningText, "Star Nearby", 0.5f, data.Direction); break;

            case UITextInfo.StationImage: SoundPlayer.PlaySound("SpaceStationSound"); EvokeTextSeq(data.info, StationText, "Space-Station Nearby", 3f, data.Direction); break;

            case UITextInfo.Fuel: SoundPlayer.PlaySound("StationFix"); StartCoroutine(EvokeText(DeliveredtoPlanetText, "Space-Ship Refueled", 0.5f));break;

            case UITextInfo.Temp: SoundPlayer.PlaySound("StationFix"); StartCoroutine(EvokeText(DeliveredtoPlanetText, "Space-Ship Temperature Cooled", 0.5f));break;

            case UITextInfo.Repairs: SoundPlayer.PlaySound("StationFix"); StartCoroutine(EvokeText(DeliveredtoPlanetText, "Space-Ship Repaired", 0.5f)); break;

            case UITextInfo.PlanetLeftText: PlanetsLeft.text = "Planets Left " +  data.count.ToString(); break;

            case UITextInfo.MapWarning: StartCoroutine(EvokeText(DeliveredtoPlanetText, "You are too far away from our Space HQ Radius, Turn Around!", 3f)); break;

            case UITextInfo.Counter: StartCoroutine(StartDelivery()); break;
        }
    } 

    private void EvokeTextSeq(UITextInfo type,RectTransform element, string text, float duration, Vector3 dir)
    {
        element.DOComplete();
        element.DOKill();

        CanvasGroup group = element.GetComponent<CanvasGroup>(); 
        if(group == null) return;

        TMP_Text font = element.GetComponentInChildren<TMP_Text>();
        if(font == null) return;

        Image arrow = element.GetComponentInChildren<Image>();
        if(arrow == null) return;

        element.DOKill();
        group.DOKill();

        font.text = text;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (type != UITextInfo.StationImage)
        {
            angle = (angle + 180f) % 360f;
        }

        arrow.rectTransform.localRotation = Quaternion.Euler(0,0, angle);      
       
        group.alpha = 1f;
        element.gameObject.SetActive(true);

        Sequence textSequence = DOTween.Sequence();

        textSequence.SetId(element); 

        textSequence.Append(element.transform.DOScale(element.transform.localScale, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack))
                .AppendInterval(duration) // This replaces WaitForSeconds
                .Append(group.DOFade(0f, 0.5f))
                .OnComplete(() => group.gameObject.SetActive(false));

    }

     private IEnumerator EvokeText(TMP_Text font, string text, float duration)
    {
        font.DOKill();
        font.rectTransform.DOKill();
        
        font.alpha = 1f;
        font.text = text;
        font.gameObject.SetActive(true);
        
        RectTransform rectT = font.gameObject.GetComponent<RectTransform>();

        Vector3 orignalScale = rectT.transform.localScale;
        rectT.transform.localScale = Vector3.zero;

        rectT.DOScale(orignalScale, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(duration);

        font.DOFade(0f, 0.5f).OnComplete(() => 
        {
        font.gameObject.SetActive(false);
        });

    }

}
