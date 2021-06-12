using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int startingLevel;   // this is for testing; final product should always start player on level 1

    public int playerLives;

    [Header("UI Handling")]
    public Canvas loseScreen;
    public Canvas gameOverScreen;
    public Text playerLivesText;

    private bool waiting = false;
    private bool waitingForDecrement = false;
    private bool waitingToReturnToPlay = false;
    private bool waitingForGameOverScreen = false;
    private bool waitingForPlayerInput = false;
    private bool waitingForLevelToLoad = false;

    private CameraFollow cameraFollower;

    // Start is called before the first frame update
    void Start()
    {
        playerLivesText.text = playerLives.ToString();
        loseScreen.enabled = false;
        gameOverScreen.enabled = false;

        cameraFollower = FindObjectOfType<CameraFollow>();

        SetGameState(GameState.LOADING);
    }

    // Update is called once per frame
    void Update()
    {
        // game state dependent loops
        HandlingDeath();
        Loading();
    }

    private void SpawnNextLevel()
    {
        SetGameState(GameState.LOADING);
        // fade out? or maybe a swipe to next level
        // create next level, destroy last level

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

    //other objects can call this method to register a player death
    public void HandlePlayerDeath()
    {
        SetGameState(GameState.DEATH);

        loseScreen.enabled = true;

        //wait .5 second
        StartCoroutine(Wait(0.5f));
        waitingForDecrement = true;

        // the rest should be handled in HandlingDeath() method called in update
    }

    // this is a loop that allows for some "waiting" -- so players can see life decrease, etc
    private void HandlingDeath()
    {
        if (GetGameState() == GameState.DEATH)
        {
            if(!waiting)
            {
                // player life count "ticks" down
                if(waitingForDecrement)
                {
                    playerLives -= 1;
                    playerLivesText.text = playerLives.ToString();

                    //wait 1 second
                    waitingForDecrement = false;
                    waitingToReturnToPlay = true;
                    StartCoroutine(Wait(1f));
                }

                // show new player life count before next screen
                else if(waitingToReturnToPlay)
                {
                    if (playerLives <= 0)
                    {
                        CallGameOver();
                    }
                    else
                    {
                        // return to gameplay on same stage
                        ResetLevel();
                    }

                    waitingToReturnToPlay = false;

                }

                // game over screen will display
                else if(waitingForGameOverScreen)
                {
                    gameOverScreen.enabled = true;
                    loseScreen.enabled = false;

                    waitingForPlayerInput = true;
                    waitingForGameOverScreen = false;
                }

                // player input required to exit game over screen
                else if(waitingForPlayerInput)
                {
                    // player can press any button to return to start
                    if (Input.anyKeyDown) //temp
                    {
                        waitingForPlayerInput = false;
                        gameOverScreen.enabled = false;
                        currentLevel = startingLevel;
                        playerLives = 3;
                        playerLivesText.text = playerLives.ToString();
                        ResetLevel(); //temp, should return to start menu
                    }
                }
            }
        }
    }

    // this loop makes sure necessary references are set before letting playing back into play
    private void Loading()
    {
        if(GetGameState() == GameState.LOADING)
        {
            if (!waiting && waitingForLevelToLoad)
            {
                if (!cameraFollower.ReferencesAreSet())
                {
                    cameraFollower.SetCameraObjects();
                }
                else
                {
                    loseScreen.enabled = false;
                    SetGameState(GameState.PLAY);
                    waitingForLevelToLoad = false;
                }
            }
        }
    }


    private void ResetLevel()
    {
        SetGameState(GameState.LOADING);

        // hard reset -- destroys old assets and reinstantiates them
        Destroy(GameObject.Find("Level " + currentLevel));
        Destroy(GameObject.Find("Level " + currentLevel + "(Clone)"));
        Instantiate(Resources.Load("Prefabs/Levels/Level " + currentLevel), Vector3.zero, Quaternion.identity);

        waitingForLevelToLoad = true;
        StartCoroutine(Wait(1f));
    }

    private void CallGameOver()
    {
        // wait two seconds
        waitingForGameOverScreen = true;
        StartCoroutine(Wait(1f));
    }

    // waits in realtime seconds
    IEnumerator Wait(float seconds)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(seconds);
        waiting = false;
    }
}
