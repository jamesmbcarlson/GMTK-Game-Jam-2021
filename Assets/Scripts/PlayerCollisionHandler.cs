using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    int trapsLayer;
    int enemyLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
        enemyLayer = LayerMask.NameToLayer("Enemy");

    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer)
        {
            Debug.Log("You just ran into Traps!");
            FindObjectOfType<LevelManager>().HandlePlayerDeath();
        }
        if(collision.gameObject.layer == enemyLayer)
        {
            Debug.Log("You just ran into Enemy!");
            FindObjectOfType<LevelManager>().HandlePlayerDeath();
        }
    }
}
