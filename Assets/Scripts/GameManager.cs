using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

 public enum GameState { Start, Ongoing, Fail, Success};

public class GameManager : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float TimerToReachPlanet = 100f; 

    private GameState currentGameState = GameState.Start;

    private Damagedby damagedTo; 

    private bool GameIsOver = true;

    private void Start()
    {
        StartCoroutine(StartDelivery());
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
       GameIsOver = false; 
       
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameIsOver) return;

        if (currentGameState == GameState.Ongoing)
        {
            if (0 < TimerToReachPlanet)
            {
               TimerToReachPlanet -= Time.deltaTime;     
            }

            else
            {
                TimerToReachPlanet -= Time.deltaTime; 
                currentGameState = GameState.Fail;
                damagedTo = Damagedby.Timer; 

            }
                 
        }

        else
        {
            DetermineGame(currentGameState);
        }
    }

    private void DetermineGame(GameState state)
    {
        switch (state)
        {
            case GameState.Success: break;
            case GameState.Fail: Fail(damagedTo); break;
        }

        GameIsOver = true;
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
