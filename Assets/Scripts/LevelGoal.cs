using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
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
        if (collision.tag == "GoodPlayer")
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
