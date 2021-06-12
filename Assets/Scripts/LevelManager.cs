using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // handle game states here
    public enum GameState
    {
        PLAY,
        DEATH,
        LOADING,
        PAUSE
    }

    private GameState gameState;

    public int currentLevel;
    public int startingLevel;   // this is for testing; final product should always start player on 

    // Start is called before the first frame update
    void Start()
    {
        // spawn first level
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnNextLevel()
    {
        gameState = GameState.LOADING;
        // fade out? or maybe a swipe to next level
        // create next level, destroy last level
        // if load is finished:
        gameState = GameState.PLAY;
        //set game state to play
        // 
    }

    public void PauseGame()
    {
        gameState = GameState.PAUSE;
        // bring up pause menu with exit or resume options
    }

    // handling game states
    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

}
