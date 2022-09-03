using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;
    int chosenLettersCount = 0;
    List<string> chosenLetters = new List<string>();

    [SerializeField] Text playersTurnText;
    [SerializeField] Canvas keyboardCanvas;

    void Awake()
    {
        instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        playersTurnText.gameObject.SetActive(state == GameState.PlayerTurn);
        keyboardCanvas.gameObject.SetActive(state == GameState.PlayerTurn);
        if(GameManager.instance.gameState == GameState.KillingMosquitoes)
        {
            killMosquitoes();
        }
    }

    private void onDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

    public void chooseLetter(string letter)
    {
        if(GameManager.instance.gameState == GameState.PlayerTurn)
        {
            increaseChosenLettersCount();
            chosenLetters.Add(letter);
            if(this.chosenLettersCount == 3)
            {
                GameManager.instance.UpdateGameState(GameState.KillingMosquitoes);
                resetChosenLettersCount();
            }
        }
    }

    public void killMosquitoes()
    {
        // Kill mosquitoes and pass turn to them
        foreach(string letter in chosenLetters)
        {
            MosquitoesManager.instance.checkChosenLetter(letter);
        }

        MosquitoeSpawner.instance.removeMosquitoes();
        chosenLetters = new List<string>();
        GameManager.instance.UpdateGameState(GameState.MosquitoesTurn);
    }

    void increaseChosenLettersCount()
    {
        this.chosenLettersCount++;
    }
    
    void decreaseChosenLettersCount()
    {
        this.chosenLettersCount--;
    }

    void resetChosenLettersCount()
    {
        this.chosenLettersCount = 0;
    } 
}
