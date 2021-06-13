using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a lot of this script was built using Robbie's PlayerMovement and PlayerInput scripts-- thanks Robbie!

public class PlayerMovement : MonoBehaviour
{
    

    public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

    public bool isEvil;

    [Header("Sprites")]
    public Sprite spriteFacingLeft;
    public Sprite spriteFacingRight;

    // player stats
    private float playerHeight;
    private float coyoteTime;						//Variable to hold coyote duration
    private float jumpTime;							//Variable to hold jump duration

    // movement
    private int direction = 1;						//Direction player is facing
    private int moveSpeed = 5;
    public float coyoteDuration = .05f;		//How long the player can jump after falling
    public float maxFallSpeed = -25f;		//Max speed player can fall

    // movement inputs
    private bool readyToClear;
    private float horizontal;
    private bool jumpPressed;
    private bool jumpHeld;


    // I just nabbed these off of Robbie's script; will need to adjust the values
    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public float jumpHoldForce = 1.9f;      //Incremental force when jump is held
    public float jumpHoldDuration = .1f;	//How long the jump key can be held

    [Header("Environment Check Properties")]
    public float footOffset = .25f;          //X Offset of feet raycast
    public float eyeHeight = 1.5f;          //Height of wall checks
    public float headClearance = .1f;       //Space needed above the player's head
    public float groundDistance = .5f;      //Distance player is considered to be on the ground
    public LayerMask groundLayer;			//Layer of the ground

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isJumping;                  //Is player jumping?
    public bool isHanging;                  //Is player hanging?
    public bool isHeadBlocked;

    // other gameobject components
    private BoxCollider2D bodyCollider;
    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody;

    // external game objects
    private LevelManager levelManager;


    // Start is called before the first frame update
    void Start()
    {
        // get references for gameojbect components
        sprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();

        // record player height from collider
        playerHeight = bodyCollider.size.y;

        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // clear out old values
        ClearInput();

        // TO-DO: return if game is not running

        if (levelManager.GetGameState() == LevelManager.GameState.PLAY)
        {
            // get input from mouse, keyboard, and/or gamepad
            ProcessInput();

            // clamp horizontal input between -1 and 1
            horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        }
    }

    void FixedUpdate()
    {
        // makes sure we're getting the most current inputs
        readyToClear = true;

        // check surroundings to determine status
        PhysicsCheck();

        if (levelManager.GetGameState() == LevelManager.GameState.PLAY)
        {
            // handle movements
            GroundMovement();
            MidAirMovement();
        }
    }

    private void ClearInput()
    {
        // If we're not ready to clear input, exit
        if (!readyToClear)
            return;

        // reset inputs
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;

        readyToClear = false;
    }

    private void ProcessInput()
    {
        horizontal += Input.GetAxis("Horizontal");

        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");
    }

    private void PhysicsCheck()
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        isOnGround = false;
        isHeadBlocked = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
            isOnGround = true;

        //Cast the ray to check above the player's head
        RaycastHit2D headCheck = Raycast(new Vector2(0f, bodyCollider.size.y), Vector2.up, headClearance);

        //If that ray hits, the player's head is blocked
        if (headCheck)
            isHeadBlocked = true;

        // NOTE: I left out Robbie's logic for wall grabs/jumps, but it could be something fun to add later
    }

    private void GroundMovement()
    {
        // NOTE: left out ledge & crouch logic from Robbie's script

        //Calculate the desired velocity based on inputs
        float xVelocity = moveSpeed * horizontal;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //If the player is on the ground, extend the coyote time window
        if (isOnGround)
            coyoteTime = Time.time + coyoteDuration;
    }

    private void MidAirMovement()
    {
        // NOTE: omitted hanging/wall jump logic

        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
        {
            // NOTE: omitted crouch logic

            //...The player is no longer on the groud and is jumping...
            isOnGround = false;
            isJumping = true;
            GetComponent<AudioSource>().Play();

            //...record the time the player will stop being able to boost their jump...
            jumpTime = Time.time + jumpHoldDuration;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            // TO-DO: tell the Audio Manager to play the jump audio
                ////AudioManager.PlayJumpAudio();
        }
        //Otherwise, if currently within the jump time window...
        else if (isJumping)
        {
            //...and the jump button is held, apply an incremental force to the rigidbody...
            if (jumpHeld)
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            //...and if jump time is past, set isJumping to false
            if (jumpTime <= Time.time)
                isJumping = false;
        }

        //If player is falling too fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }

    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        direction *= -1;

        // update sprites
        if (horizontal < 0)
        {
            sprite.sprite = spriteFacingLeft;
        }
        else if (horizontal > 0)
        {
            sprite.sprite = spriteFacingRight;
        }
    }

    // allows other scripts to flip orientation... I need this for one part lol
    public void ReverseCharDirection()
    {
        if(sprite.sprite == spriteFacingLeft)
        {
            sprite.sprite = spriteFacingRight;
        }
        else
        {
            sprite.sprite = spriteFacingLeft;
        }
    }

    public void Freeze()
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }
}
