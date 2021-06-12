using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenHandling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "GoodPlayer")
        {
            Debug.Log("Player fell offscreen.");
            // lose screen appears
            FindObjectOfType<LevelManager>().HandlePlayerDeath();
        }

        else if (collision.tag == "BadPlayer")
        {
            Debug.Log("Bad Player offscreen. Deleting.");
            Destroy(collision.gameObject);
        }
    }
}
