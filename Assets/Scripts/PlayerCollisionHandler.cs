using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    int trapsLayer;
    int enemyLayer;

    private bool level2EventTriggered = false;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelManager.GetGameState() == LevelManager.GameState.PLAY)
        {
            if (collision.gameObject.layer == trapsLayer)
            {
                Debug.Log("You just ran into Traps!");
                FindObjectOfType<LevelManager>().HandlePlayerDeath();
            }
            if (collision.gameObject.layer == enemyLayer)
            {
                Debug.Log("You just ran into Enemy!");
                FindObjectOfType<LevelManager>().HandlePlayerDeath();
            }
        }

        if(collision.tag == "WeirdThing")
        {
            Debug.Log("Weird Thing collision detected");
            if(!level2EventTriggered)
            {
                FindObjectOfType<LevelManager>().TriggerEvent(0);
                level2EventTriggered = true;

            }
        }

    }
}
