using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public bool isGameOver {get; private set;} = false;
    GameObject gameOverCanvasGameObject;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameOverCanvasGameObject = GameObject.Find("/GameOverCanvas");
    }

    void Update()
    {
        if(isGameOver)
        {
            gameOverCanvasGameObject.GetComponent<Canvas>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Mosquitoe") {
            GameManager.instance.UpdateGameState(GameState.GameOver);
            isGameOver = true;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Calling restart game");
        MosquitoesManager.instance.DestroyAll();
        MosquitoeSpawner.instance.ResetData();
        this.isGameOver = false;
        gameOverCanvasGameObject.GetComponent<Canvas>().enabled = false;
        GameManager.instance.UpdateGameState(GameState.SpawningMosquitoes);
    }
}
