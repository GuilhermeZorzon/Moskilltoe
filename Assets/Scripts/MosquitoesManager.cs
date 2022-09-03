using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MosquitoesManager : MonoBehaviour
{
    public static MosquitoesManager instance;
    private Rigidbody2D _rb;
    private bool isDestoyed = false;
    private bool isFlying = false;

    void Awake()
	{
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
	}

    private async void GameManagerOnGameStateChanged(GameState state)
    {
        if(GameManager.instance.gameState == GameState.MosquitoesTurn)
        {
            var tasks = new List<Task>();
            foreach (Mosquitoe mosquitoe in MosquitoeSpawner.instance.spawnedMosquitoes)
            {
                tasks.Add(mosquitoe.FlyMosquitoes());
            }
            await Task.WhenAll(tasks);
            if(!CollisionManager.instance.isGameOver)
            {
                Debug.Log("Passing turn to spawner");
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
            if(mosquitoe.assignedText.Contains(letter) && !mosquitoe.isDestoyed)
            {
                if(mosquitoe.currentAssignedText.Count == 1)
                {
                    this.isDestoyed = true;
                    DestroyMosquitoe(mosquitoe);
                    MosquitoeSpawner.instance.mosquitoesToRemove.Add(mosquitoe);
                }
                mosquitoe.RemoveLetter(letter);
            }
        }
    }

    void DestroyMosquitoe(Mosquitoe mosquitoe) 
    {
        Debug.Log("destoyed this one: " + mosquitoe.id + " with letter " + mosquitoe.assignedText);
        MosquitoeSpawner.instance.DecreaseMosquitoeCounter();
        Destroy(mosquitoe.gameObject);
    }
}
