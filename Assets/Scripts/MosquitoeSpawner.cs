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
	int mosquitoeCount = 0;
	int spawnedMosquitoesInWave = 0;
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
			this.waveCount++;
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

		this.spawnedMosquitoesInWave = 0;
		int numberOfMosquitoesInWave = GetNumberOfMosquitoesInWave();
		while (spawnedMosquitoesInWave < numberOfMosquitoesInWave)
		{
			SpawnMosquitoe();
			await Task.Delay(600);
		}

		this.isSpawning = false;

		await Task.Yield();
	}

	void SpawnMosquitoe()
	{
		ScriptedMosquitoe scriptedMosquitoe = GetMosquitoeToSpawn();
		var positionToSpawn = FindPositionToSpawn();
		Mosquitoe mosquitoe = Instantiate(mosquitoePrefab, positionToSpawn, Quaternion.identity);
		mosquitoe._scriptedMosquitoe = scriptedMosquitoe;
		mosquitoe.transform.SetParent(gameObject.transform, false);
		
		spawnedMosquitoes.Add(mosquitoe);
		IncreaseMosquitoeCount();
		this.spawnedMosquitoesInWave++;
	}

	ScriptedMosquitoe GetMosquitoeToSpawn()
	{
		if (this.waveCount == 0)
		{
			return spawnableMosquitoes[0];
		}
		var whichToSpawn = Random.Range(0, spawnableMosquitoes.Count);
		ScriptedMosquitoe scriptedMosquitoe = spawnableMosquitoes[whichToSpawn];
		return scriptedMosquitoe;
	}

	int GetNumberOfMosquitoesInWave()
	{
		int numberOfMosquitoesInWave = 1;

		switch (this.waveCount)
		{
			case 0:
				break;
			case var expression when this.waveCount <= 2:
				numberOfMosquitoesInWave = GetNumberByProbability(new List<int>() {40, 60}, new List<int>() {1, 2});
				break;
			case var expression when this.waveCount <= 5:
				numberOfMosquitoesInWave = GetNumberByProbability(new List<int>() {28, 52, 20}, new List<int>() {1, 2, 3});
				break;
			case var expression when this.waveCount <= 8:
				numberOfMosquitoesInWave = GetNumberByProbability(new List<int>() {20, 55, 25}, new List<int>() {1, 2, 3});
				break;
			case var expression when this.waveCount <= 12:
				numberOfMosquitoesInWave = GetNumberByProbability(new List<int>() {12, 59, 29}, new List<int>() {1, 2, 3});
				break;
			default:
				numberOfMosquitoesInWave = GetNumberByProbability(new List<int>() {5, 61, 34}, new List<int>() {1, 2, 3});
				break;
		}

		return numberOfMosquitoesInWave;
	}

	int GetNumberByProbability(List<int> probabilities, List<int> numbers)
	{
		int probabilityMeasure = Random.Range(0, 100);
		int currentProbabilitySum = 0;
		int currentIndex = 0;

		while (currentIndex < probabilities.Count)
		{
			if (probabilityMeasure <= currentProbabilitySum + probabilities[currentIndex])
			{
				return numbers[currentIndex];
			}
			currentProbabilitySum += probabilities[currentIndex];
			currentIndex++;
		}

		return numbers[0];
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

	public void ResetData()
	{
		mosquitoeCount = 0;
		spawnedMosquitoesInWave = 0;
		spawnedMosquitoes = new List<Mosquitoe>();
		mosquitoesToRemove = new List<Mosquitoe>();
		occupiedYPositions = new List<float>();
		waveCount = 0;
	}
}
