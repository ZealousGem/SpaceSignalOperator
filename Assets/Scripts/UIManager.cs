using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver
{
    
    public UIObersver subject;
    public TMP_Text ShipHealth;
    
    public TMP_Text ShipTemperature;

    public TMP_Text ShipFuel;

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
            case ShipFuel fuel: ShipFuel.text ="Fuel: "+ fuel.amount.ToString();break;
            case ShipHealth health: ShipHealth.text = health.amount.ToString() +"%"; break;
            case ShipTemp temp: ShipTemperature.text ="Temp: "+ temp.amount.ToString(); break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShipHealth.text =100.ToString()+"%";
        ShipFuel.text ="Fuel: "+ 100.ToString();
        ShipTemperature.text ="Temp: "+ 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
