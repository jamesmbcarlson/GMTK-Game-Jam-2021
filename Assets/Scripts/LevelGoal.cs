using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GoodPlayer" && levelManager.GetGameState() == LevelManager.GameState.PLAY)
        {
            //Debug.Log("Player Win condition triggered. CONGRATS!!");
            collision.GetComponent<PlayerMovement>().Freeze();

            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("BadPlayer");
            foreach (GameObject obj in playerObjects)
            {
                obj.GetComponent<PlayerMovement>().Freeze();
            }

            FindObjectOfType<LevelManager>().PlayerBeatLevel();
            GetComponent<AudioSource>().Play();
            
        }
    }
}
