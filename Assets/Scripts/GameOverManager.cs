using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public bool isGameOver {get; private set;} = false;
    [SerializeField] Canvas gameOverCanvas;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(isGameOver && !Mosquitoe.instance.isFlying)
        {
            gameOverCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Mosquitoe") {
            GameManager.instance.UpdateGameState(GameState.GameOver);
            isGameOver = true;
        }
    }

    private void onDestroy()
	{
		RestartGame();
	}

    public void RestartGame()
    {
        MosquitoesManager.instance.DestroyAll();
        MosquitoeSpawner.instance.ResetData();
        this.isGameOver = false;
        gameOverCanvas.GetComponent<Canvas>().enabled = false;
        GameManager.instance.UpdateGameState(GameState.SpawningMosquitoes);
    }
}
