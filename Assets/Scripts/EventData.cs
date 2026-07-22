using System;
using System.Collections.Generic;
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

public class ClearObjectFromList : EventData
{
    public BaseObstacle action;

    public ClearObjectFromList(BaseObstacle _action)
    {
        action = _action;
    }
}

public class StopObstacles : EventData
{
    public bool action;

    public StopObstacles(bool _action)
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

    public StopShip stop;

    public EndGameEvent(GameState gamstate, StopShip _stop, bool _StopMoving)
    {
        stop = _stop;
        StopMoving = _StopMoving;
        GameEvent = gamstate;
    }

    public EndGameEvent(Damagedby _action, GameState _Damaged)
    {
        action = _action;
        GameEvent = _Damaged;
    }

    public EndGameEvent(GameState _Damaged)
    {
        GameEvent = _Damaged;
    }
}

public class TutorialEvent : EndGameEvent
{
    public TutorialObject obj; 

    public TutorialEvent(TutorialObject _obj) : base(GameState.Tutorial)
    {
        obj = _obj;
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

public class ButtonEvent : EventData
{
    public ButtonAnimations action;

    public float amount;

    public ButtonEvent(ButtonAnimations _action)
    {
        action = _action;
        
    }

    
}

public class GetTransformOfObject : EventData
{
    public Transform PlanetCoordinates;

    public GetTransformOfObject(Transform Planet)
    {
        PlanetCoordinates = Planet;
    }

}

public class endGameUI: EventData
{
    public GameState gameState;

    public string Title;

    public string Reason;

    public float Amount;

    public int StarRating;

    public List<float> scoreList;

    public endGameUI(GameState _gameState)
    {
        gameState = _gameState;
    }

    public endGameUI(GameState _gameState, string title, string reason)
    {
        gameState = _gameState;
        Title = title;
        Reason = reason;
    }

    public endGameUI(GameState _gameState, string title, string reason, float timer, int amount, List<float> _scoreList)
    {
        gameState = _gameState;
        Title = title;
        Reason = reason;
        Amount = timer;
        StarRating = amount;
        scoreList = _scoreList;
    }
}

public class WarningTextEvent : EventData
{
    public UITextInfo textInfo; 

    public BaseObstacle obstacle;

    public int PlanetsLeft;

    public WarningTextEvent(UITextInfo uITextInfo)
    {
        textInfo = uITextInfo;
    }

    public WarningTextEvent(UITextInfo uITextInfo, int _PlanetLeft)
    {
        textInfo = uITextInfo;
        PlanetsLeft = _PlanetLeft;
    }

    public WarningTextEvent(UITextInfo uITextInfo, BaseObstacle _obstacle)
    {
        textInfo = uITextInfo;
        obstacle = _obstacle;
    }
}
