using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState gameState = GameState.GameOver;

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
        if(this.gameState == newState) 
        {
            return;
        }
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
        // Do nothing
    }

    private void onGameStateChangeToMosquitoesTurn()
    {
        // Do nothing
    }

    private void onGameStateChangeToSpawningMosquitoes()
    {
        // Do nothing
    }

    private void onGameStateChangeToGameOver()
    {
        // Do nothing
    }
}

public enum GameState {
    SpawningMosquitoes,
    PlayerTurn,
    KillingMosquitoes,
    MosquitoesTurn,
    GameOver,
}
