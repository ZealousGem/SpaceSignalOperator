using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public enum GameState { Start, Ongoing, Fail, Success, Pause, Delivered};

public class GameManager : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float TimerToReachPlanet = 0f; 

    private GameState currentGameState = GameState.Start;

    private Damagedby damagedTo;

    public string Song; 

    public static string SongName;

    public List<DeliveredPlanet> Planets;

    private Queue<DeliveredPlanet> PlanetObjectives = new Queue<DeliveredPlanet>();

    protected virtual void Awake()
    {
        SongName = Song;
        AddPlanetsToQueue();
    }

    private void AddPlanetsToQueue()
    { 

        if(Planets == null || PlanetObjectives == null) return;  

        for (int i = Planets.Count - 1; i >= 0; i--)
        {
            PlanetObjectives.Enqueue(Planets[i]);
        }
    }

    private void OnEnable()=> EventBus.Subscribe<EndGameEvent>(ReceiveGameEvent);
    

    private void OnDisable()=> EventBus.Unsubscribe<EndGameEvent>(ReceiveGameEvent);

    private void ReceiveGameEvent(EndGameEvent data)
    {
        if(data.GameEvent == GameState.Success) return;
        if (data.GameEvent == GameState.Fail) damagedTo = data.action;
        
        DetermineGame(data.GameEvent);
    }
    private void Update()
    {
      if (currentGameState == GameState.Ongoing)TimerToReachPlanet += Time.deltaTime;   
    }

    private void setPlanetCoordinate()
    {
        if(PlanetObjectives.Count == 0)
        {
           currentGameState = GameState.Success;
           UnityEngine.Debug.Log(PlanetObjectives.Count);

           DetermineGame(currentGameState);
           EventBus.Act(new WarningTextEvent(UITextInfo.PlanetLeftText, 0));

           return;     
        }

        if(currentGameState == GameState.Start) EventBus.Act(new WarningTextEvent(UITextInfo.PlanetLeftText, PlanetObjectives.Count));

        if(currentGameState == GameState.Ongoing) EventBus.Act(new WarningTextEvent(UITextInfo.PlanetText, PlanetObjectives.Count));
    
        DeliveredPlanet Planet = PlanetObjectives.Dequeue();
        //UnityEngine.Debug.Log(Planet.gameObject.name);
        Planet.setTargetPlanet();

        EventBus.Act(new GetTransformOfObject(Planet.gameObject.transform));               
    }

    private void Start()
    {
        setPlanetCoordinate();
        UnityEngine.Debug.Log(currentGameState);
       // SoundPlayer.PlaySound("AmbientSounds");
        SoundPlayer.PlaySound(SongName);
        //StartCoroutine(StartDelivery());
    }

    private void DetermineGame(GameState state)
    {
        switch (state)
        {
            case GameState.Success: StartCoroutine(setSuccessScreen());  break;

            case GameState.Fail: currentGameState = state; Fail(damagedTo); break;

            case GameState.Delivered: setPlanetCoordinate();break;

            case GameState.Ongoing: currentGameState = state;  EventBus.Act(new endGameUI(GameState.Ongoing)); break;
        }

    }

    private IEnumerator setSuccessScreen()
    {
        EventBus.Act(new EndGameEvent(GameState.Success,StopShip.stop, true));
        yield return new WaitForSeconds(0.3f);
        
        SoundPlayer.StopSound(SongName);
        EventBus.Act(new endGameUI(GameState.Success, "Packages Successfully Delivered", "All Packages have been delivered to the Planets Well Done. ", TimerToReachPlanet));
    }

    private void Fail(Damagedby damagedby)
    {
       // SoundPlayer.StopSound(SongName);

        switch (damagedby)
        {
            case Damagedby.Blackhole: EventBus.Act(new endGameUI(GameState.Fail, "Fired", "Your Delivery has been lost into the depths of a black hole, good luck getting that back."));  break;

            case Damagedby.NeutronStar: EventBus.Act(new endGameUI(GameState.Fail, "Fired", "You sent the ship too close to a neutron star disintigrating it to pieces.")); break;

            case Damagedby.BurnUp: EventBus.Act(new endGameUI(GameState.Fail, "Fired", "You failed to check your ship's temperature leaving it to burn up and losing the goods.")); break;

            case Damagedby.FlewAway: EventBus.Act(new endGameUI(GameState.Fail, "Ship Missing", "where the fuck are you going???")); break;
            
            case Damagedby.Default: EventBus.Act(new endGameUI(GameState.Fail, "Fired", "You forgot that asteroids and debris exists, maybe dodge it next time.")); break;

            case Damagedby.OringalPlanet: EventBus.Act(new endGameUI(GameState.Fail, "Fired", "You lazy mother fucker you still got other planets to head to, no slacking.")); break;
        }
    }
}
