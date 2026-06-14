using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum UITextInfo{PlanetText, AsteroidText, solarFlare, CometText}

public class UIManager : MonoBehaviour, IObserver
{
    
    public UIObersver subject;
    public TMP_Text ShipHealth;
    public TMP_Text ShipTemperature;
    public TMP_Text LightYearsScale;
    public TMP_Text WarningText;
    public TMP_Text DeliveredtoPlanetText;
    public Image Arrow;
    public FuelGauge guage;
    public Image TempImage;
    public Image StaticScreen;
    
    private readonly float maxAlhpa = 7f / 255f;

    private void OnEnable()
    {
        subject.AddObersver(this);
        EventBus.Subscribe<WarningTextEvent>(ExtractUItext);
    }

    private void OnDisable()
    {
        subject.RemoveObserver(this);
        EventBus.Unsubscribe<WarningTextEvent>(ExtractUItext);
    }

    public void onNotify<T>(T notificationData)
    {
        switch (notificationData)
        {
            case ShipFuel fuel: guage.StartMovement(fuel.amount); break;
            case ShipHealth health: ShipHealth.text = health.amount.ToString() +"%"; break;
            case ShipTemp temp: setTemperature(temp.amount); break;
            case PlanetPosDirection planet: DisplayPlanetDirection(planet.amount, planet.Direction); break;
            case EvokeSpawnScreen spawnStaticScreen: StartCoroutine(StaticScreenTimer(spawnStaticScreen.timer, spawnStaticScreen.action)); break; 
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
             Debug.Log("transition");    
        }
     
        else
        {
            StaticScreen.gameObject.SetActive(false);
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ActivateStaticScreen(false);

        Color colour = TempImage.color;
        colour.a = 0f;
        TempImage.color = colour;

        ShipHealth.text =100.ToString()+"%";
        ShipTemperature.text =0.ToString() +"c";
    }

    private void ExtractUItext(WarningTextEvent data)
    {
        switch (data.textInfo)
        {
            case UITextInfo.PlanetText: StartCoroutine(EvokeText(DeliveredtoPlanetText, "Package Delivered, Next Planet Coordinates are in", 3f)); break;
            case UITextInfo.AsteroidText: StartCoroutine(EvokeText(WarningText, "Asteroid Incoming", 0.5f)); break;
            case UITextInfo.CometText:StartCoroutine(EvokeText(WarningText, "Comet Incoming", 0.5f)); break;
            case UITextInfo.solarFlare: StartCoroutine(EvokeText(WarningText, "SolarFlare Incoming", 0.5f)); break;
        }
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

    // Update is called once per frame

}
