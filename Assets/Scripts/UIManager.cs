using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver
{
    
    public UIObersver subject;
    public TMP_Text ShipHealth;
    public TMP_Text ShipTemperature;
    public TMP_Text LightYearsScale;
    public Image Arrow;
    public FuelGauge guage;

    public Image TempImage;

    public Image StaticScreen;
    
    private readonly float maxAlhpa = 7f / 255f;

    private void OnEnable()
    {
        subject.AddObersver(this);
    }

    private void OnDisable()
    {
        subject.RemoveObserver(this);
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

    private void ActivateStaticScreen(bool state) => StaticScreen.gameObject.SetActive(state);

    private void DisplayPlanetDirection(float pos, Vector3 dir)
    {
        int displayLightYears = (int)pos;
        LightYearsScale.text = displayLightYears.ToString() +" light years";

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

    // Update is called once per frame

}
