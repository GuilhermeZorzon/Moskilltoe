using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class MosquitoeSpawner : MonoBehaviour
{
	public static MosquitoeSpawner instance;
	public List<ScriptedMosquitoe> spawnableMosquitoe = new List<ScriptedMosquitoe>();
	[SerializeField] Mosquitoe mosquitoePrefab;
	private int mosquitoeCount = 0;
	private bool isSpawning = false;

	public List<Mosquitoe> spawnedMosquitoes = new List<Mosquitoe>();
	public List<Mosquitoe> mosquitoesToRemove = new List<Mosquitoe>();

	public int waveCount {get; private set;} = 0; 

	void Awake()
	{
		instance = this;
		GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
	}

	private async void GameManagerOnGameStateChanged(GameState state)
	{
		if(GameManager.instance.gameState == GameState.SpawningMosquitoes)
		{
			await SpawnMosquitoes();
			GameManager.instance.UpdateGameState(GameState.PlayerTurn);
		}
	}

	private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

	async Task SpawnMosquitoes()
	{
		this.isSpawning = true;

		while (mosquitoeCount < 2)
		{
			SpawnMosquitoe();
			await Task.Delay(750);
		}

		this.isSpawning = false;

		await Task.Yield();
	}

	void SpawnMosquitoe()
	{
		var whichToSpawn = Random.Range(0, 2);
		ScriptedMosquitoe scriptedMosquitoe = spawnableMosquitoe[whichToSpawn];
		Mosquitoe mosquitoe = Instantiate(mosquitoePrefab, new Vector3(-7.5f, Random.Range(0, 4), 0), Quaternion.identity);
		mosquitoe._scriptedMosquitoe = scriptedMosquitoe;
		mosquitoe.transform.SetParent(gameObject.transform, false);
		
		spawnedMosquitoes.Add(mosquitoe);
		IncreaseMosquitoeCount();
	}

	public void removeMosquitoes()
	{
			foreach(Mosquitoe mosquitoeToRemove in mosquitoesToRemove)
			{
				spawnedMosquitoes.Remove(mosquitoeToRemove);
			}

			mosquitoesToRemove = new List<Mosquitoe>();
	}

	public void DecreaseMosquitoeCounter()
	{
		mosquitoeCount--;
	}

	public void IncreaseMosquitoeCount()
	{
		mosquitoeCount++;
	}

	
}
