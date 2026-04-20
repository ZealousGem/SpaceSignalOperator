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

public class EndGameEvent : EventData
{
    public Damagedby action;

    public GameState GameEvent;

    public bool StopMoving;

    public EndGameEvent(Damagedby _action, GameState _Damaged)
    {
        action = _action;
        GameEvent = _Damaged;
    }

    public EndGameEvent(GameState _Damaged, bool _StopMoving)
    {
        GameEvent = _Damaged;
        StopMoving = _StopMoving;
    }
}

public class StationEvent : EventData
{
    public StationType action;

    public float amount;

    public StationEvent(StationType _action, float _amount)
    {
        action = _action;
        amount = _amount;
    }

    
}
