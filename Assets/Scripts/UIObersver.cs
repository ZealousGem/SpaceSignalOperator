using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IObserver
{
    void onNotify<T>(T notificationData);
};

public struct ShipHealth{ public int amount;}

public struct ShipFuel{public float amount;}

public struct ShipTemp{public float amount;}

public struct PlanetPosDirection{public float amount; public Vector3 Direction;}

public class UIObersver : MonoBehaviour
{
     List<IObserver> _observers = new List<IObserver>();


    public void AddObersver(IObserver observer)=> _observers?.Add(observer);
    

    public void RemoveObserver(IObserver observer)=> _observers?.Remove(observer);
    

     public void TellObervers<T>(T notificationData)
    {
        var observersSnapshot = new List<IObserver>(_observers);
        foreach (IObserver observer in observersSnapshot)
        {
            observer.onNotify(notificationData);
        }
    }
}
