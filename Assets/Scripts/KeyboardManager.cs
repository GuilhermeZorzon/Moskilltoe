using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;
    int chosenLettersCount = 0;
    List<string> chosenLetters = new List<string>();
    GameObject keyboardCanvasGameObject;
    public ScriptableSound buttonSelectSound;

    void Awake()
    {
        instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Start()
    {
        keyboardCanvasGameObject = GameObject.Find("/KeyboardCanvas");
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (keyboardCanvasGameObject)
        {
            this.keyboardCanvasGameObject.GetComponent<Canvas>().enabled = (state == GameState.PlayerTurn);
        }
        if (state == GameState.KillingMosquitoes)
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
            buttonSelectSound.Play();
            increaseChosenLettersCount();
            this.chosenLetters.Add(letter);

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
        foreach(string letter in this.chosenLetters)
        {
            MosquitoesManager.instance.checkChosenLetter(letter);
        }
        MosquitoeSpawner.instance.removeMosquitoes();
        this.chosenLetters = new List<string>();
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
