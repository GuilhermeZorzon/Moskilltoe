using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState gameState = GameState.PlayerTurn;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.SpawningMosquitoes);
    }
    public void UpdateGameState(GameState newState)
    {
        this.gameState = newState;

        switch (newState) 
        {
            case GameState.PlayerTurn:
                onGameStateChangeToPlayerTurn();
                break;
            case GameState.KillingMosquitoes:
                onGameStateChangeToKillingMosquitoes();
                break;
            case GameState.MosquitoesTurn:
                onGameStateChangeToMosquitoesTurn();
                break;
            case GameState.SpawningMosquitoes:
                onGameStateChangeToSpawningMosquitoes();
                break;
            case GameState.GameOver:
                onGameStateChangeToGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, "A new non subscribed event was triggered");
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void onGameStateChangeToPlayerTurn()
    {
        // Do nothing
    }
    
    private void onGameStateChangeToKillingMosquitoes()
    {
        // KeyboardManager.instance.killMosquitoes();
    }

    private void onGameStateChangeToMosquitoesTurn()
    {
        // if (MosquitoeSpawner.instance.spawnedMosquitoes.Count > 0)
        // {
        //     // If there are alive mosquitoes, make them fly
        //     foreach(Mosquitoe mosquitoe in MosquitoeSpawner.instance.spawnedMosquitoes)
        //     {
        //         StartCoroutine(mosquitoe.FlyMosquitoes());
        //     }
        // }
        // // If there aren't alive mosquitoes, pass turn to spawner
        // UpdateGameState(GameState.SpawningMosquitoes);
    }

    private void onGameStateChangeToSpawningMosquitoes()
    {
        // Do nothing
    }

    private void onGameStateChangeToGameOver()
    {
        Debug.Log("You Lost");
    }
}

public enum GameState {
    SpawningMosquitoes,
    PlayerTurn,
    KillingMosquitoes,
    MosquitoesTurn,
    GameOver,
}
