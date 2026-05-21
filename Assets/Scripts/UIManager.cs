using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver
{
    
    public UIObersver subject;
    public TMP_Text ShipHealth;
    public TMP_Text ShipTemperature;
    public FuelGauge guage;

    void OnEnable()
    {
        subject.AddObersver(this);
    }

    void OnDisable()
    {
        subject.RemoveObserver(this);
    }

    public void onNotify<T>(T notificationData)
    {
        switch (notificationData)
        {
            case ShipFuel fuel: guage.StartMovement(fuel.amount); break;
            case ShipHealth health: ShipHealth.text = health.amount.ToString() +"%"; break;
            case ShipTemp temp: ShipTemperature.text = temp.amount.ToString() +"c"; break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShipHealth.text =100.ToString()+"%";
        ShipTemperature.text =0.ToString() +"c";
    }

    // Update is called once per frame

}
