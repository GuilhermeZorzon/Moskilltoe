using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MosquitoesManager : MonoBehaviour
{
    public static MosquitoesManager instance;
    private Rigidbody2D _rb;
    private bool isFlying = false;

    void Awake()
	{
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
	}

    private async void GameManagerOnGameStateChanged(GameState state)
    {
        if(this && this.gameObject && state == GameState.MosquitoesTurn)
        {
            var tasks = new List<Task>();
            foreach (Mosquitoe mosquitoe in MosquitoeSpawner.instance.spawnedMosquitoes)
            {
                tasks.Add(mosquitoe.FlyMosquitoes());
            }
            await Task.WhenAll(tasks);
            if(!GameOverManager.instance.isGameOver)
            {
                GameManager.instance.UpdateGameState(GameState.SpawningMosquitoes);
            }
        }
    }

    private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

    public void checkChosenLetter(string letter)
    {
        foreach (Mosquitoe mosquitoe in MosquitoeSpawner.instance.spawnedMosquitoes)
        {
            mosquitoe.CheckScriptedLetterActions(mosquitoe, letter);
        }
    }

    public void DestroyAll()
    {
        foreach (Mosquitoe mosquitoe in MosquitoeSpawner.instance.spawnedMosquitoes)
        {
            mosquitoe.isDestoyed = true;
            Destroy(mosquitoe.gameObject);
        }
    }
}
