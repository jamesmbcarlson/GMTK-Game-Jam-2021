using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Trigger Detected");

        if(collision.tag == "Offscreen")
        {
            Debug.Log("Player fell offscreen.");
            Debug.Log("Player Lose condition triggered.");
            // lose screen appears with option to retry
        }

        if(collision.tag == "Goal")
        {
            Debug.Log("Player Win condition triggered. CONGRATS!!");
            // win screen or just cut to next level
        }
    }
}
