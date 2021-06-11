using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int moveSpeed = 5;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        // update position
        var deltaX = transform.position.x + Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(deltaX, 0f);

        // update sprites
        if(Input.GetAxis("Horizontal") < 0)
        {
            sprite.flipX = true;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            sprite.flipX = false;
        }
    }
}
