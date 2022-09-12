using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class MosquitoeSpawner : MonoBehaviour
{
	public static MosquitoeSpawner instance;
	public List<ScriptedMosquitoe> spawnableMosquitoes = new List<ScriptedMosquitoe>();
	[SerializeField] Mosquitoe mosquitoePrefab;
	private int mosquitoeCount = 0;
	private bool isSpawning = false;

	public List<Mosquitoe> spawnedMosquitoes = new List<Mosquitoe>();
	public List<Mosquitoe> mosquitoesToRemove = new List<Mosquitoe>();
	public List<float> possibleYPositions = new List<float>() {-0.85f, 1.35f, 3.65f};
	public List<float> occupiedYPositions = new List<float>();
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
			this.occupiedYPositions = new List<float>();
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
		ScriptedMosquitoe scriptedMosquitoe = spawnableMosquitoes[whichToSpawn];
		var positionToSpawn = FindPositionToSpawn();
		Mosquitoe mosquitoe = Instantiate(mosquitoePrefab, positionToSpawn, Quaternion.identity);
		mosquitoe._scriptedMosquitoe = scriptedMosquitoe;
		mosquitoe.transform.SetParent(gameObject.transform, false);
		
		spawnedMosquitoes.Add(mosquitoe);
		IncreaseMosquitoeCount();
	}

	public Vector3 FindPositionToSpawn()
	{
		float yPosition = possibleYPositions[Random.Range(0, 3)];
		while (occupiedYPositions.Contains(yPosition))
		{
			yPosition = possibleYPositions[Random.Range(0, 3)];
		}
		occupiedYPositions.Add(yPosition);
		return new Vector3(-7.5f, yPosition, 0);
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
