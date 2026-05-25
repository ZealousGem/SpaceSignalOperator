using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public enum GameState { Start, Ongoing, Fail, Success, Pause};

public class GameManager : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float TimerToReachPlanet = 0f; 

    private GameState currentGameState = GameState.Start;

    private Damagedby damagedTo; 

    public List<DeliveredPlanet> Planets;

    private Queue<DeliveredPlanet> PlanetObjectives = new Queue<DeliveredPlanet>();

    private void Awake()
    {
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

    //  private void Update()
    //  {

    //   if (currentGameState == GameState.Ongoing)TimerToReachPlanet += Time.deltaTime;   
       
    //  }

    private void setPlanetCoordinate()
    {
        if(PlanetObjectives.Count ==0)
        {
           currentGameState = GameState.Success;
           DetermineGame(currentGameState);
           return;     
        }

        GameObject Planet = PlanetObjectives.Dequeue().gameObject;
        EventBus.Act(new GetTransformOfObject(Planet.transform));      
       
    }

    private void Start()
    {
        setPlanetCoordinate();
        //StartCoroutine(StartDelivery());
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
            }
            
            yield return null;
        }
        
        
       currentGameState = GameState.Ongoing; 
       
    }

    private void DetermineGame(GameState state)
    {
        switch (state)
        {
            case GameState.Success: break;
            case GameState.Fail: Fail(damagedTo); break;
        }

    }

    private void Fail(Damagedby damagedby)
    {
        switch (damagedby)
        {
            case Damagedby.Blackhole:  break;
            case Damagedby.NeutronStar:  break;
            case Damagedby.BurnUp:  break;
            case Damagedby.Timer: break;
            case Damagedby.Default:  break;
        }
    }
}
