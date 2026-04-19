using UnityEngine;

public class EventData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    public EventData()
    {
        
    }


}

public class setInput : EventData
{
    public SignalDirections action;

    public setInput(SignalDirections _action)
    {
        action = _action;
    }
}

public class DamageShip : EventData
{
    public Damagedby action;

    public float Damaged;

    public DamageShip(Damagedby _action, float _Damaged)
    {
        action = _action;
        Damaged = _Damaged;
    }
}
