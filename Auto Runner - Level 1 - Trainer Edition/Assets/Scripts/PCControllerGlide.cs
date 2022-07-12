using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class PCControllerGlide : MonoBehaviour
{
    public enum PlayerStates //We made our own data type which can ONLY accept the values written within the curly brackets
    {
        IDLE, //Value to represent when PC is in IDLE State
        RUN, //Value to represent when PC is in RUN State
        INAIR, //Value to represent when PC is in INAIR State
        ABILITY, //Value to represent when PC is in ABILITY State
        DEAD //Value to represent when PC is in DEAD State
    }

    [Header("Player Attributes variables")] //Seperate speed variables in inspector - to make the project user friendly, not needed
    public float speed = 5f; //stores initial speed value of pc gameobject
    float ballVelocity; //Store X axis velocity of the ball at any given time
    public float minVelocity = 10f; //Store minimum velocity pc gameobject needs to have to move forward
    public float maxVelocity = 30f; //Store maximum velocity pc gameobject can attain
    public float acceleration = 2f; //Store the amount of acceleration of pc gameobject
    public float jumpForce = 100f; //Store the amount of force by which player should jump up
    public float playerGravity = 0.05f;

    [Header("Player Components")] //Seperate components in inspector - to make the project user friendly, not needed
    Rigidbody2D playerRB; //store Rigidbody2D Component of pc gameobject
    Animator playerAnim;

    [Header("State Machine Variables")] //Seperate components in inspector - to make the project user friendly, not needed
    public PlayerStates currentState; //make a variable of data type PlayerState
    

    [Header("Player Collision Variables")] //Seperate components in inspector - to make the project user friendly, not needed
    public LayerMask whatIsGround; //make the public variable to assign which unity layer is to be considered as ground
    public bool isGrounded; //boolean to store if pc is colliding with anything on Ground Layer
    public Transform groundCheckFrontTransform; //Stores the GroundCheckFront Transform in inspector
    public Transform groundCheckBackTransform; //Stores the GroundCheckBack Transform in inspector

    [Header("Game State")] //Seperate components in inspector - to make the project user friendly, not needed
    public bool isPaused = false;
    public bool touchOnUI = false;

    [Header("Animation variables")]
    public bool isRunning = false;
    public bool isJumping = false;

    [Header("VFX variables")]
    public GameObject DustVFX;
    public GameObject DeathVFX;
    public GameObject PickupVFX;
    VisualEffect DustVFXGraph;
    VisualEffect DeathVFXGraph;
    VisualEffect PickupVFXGraph;


    #region Unity Call backs

    void Awake() //Called before Start Function - use this for initializing component variables
    {
        playerRB = GetComponent<Rigidbody2D>(); //Access the Rigidbody2D component and store all properties in playerRB when game starts
        playerAnim = GetComponent<Animator>();

        DustVFXGraph = DustVFX.GetComponent<VisualEffect>();
        DeathVFXGraph = DeathVFX.GetComponent<VisualEffect>();
        PickupVFXGraph = PickupVFX.GetComponent<VisualEffect>();

        currentState = PlayerStates.IDLE; //Set currentstate to Run State at start of the game


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ballVelocity = playerRB.velocity.x; //Update ballVelocity value with pc gameobject's physics X axis velocity every frame

        CheckGroundCollision(); //Call the function which checks ground every frame

        playerAnim.SetBool("isRunning", isRunning);
        playerAnim.SetBool("isJumping", isJumping);

        if (GameManager.instance.currentGameState == GameManager.GameStates.PAUSE)
        {
            isPaused = true;
        }
        else if (GameManager.instance.currentGameState != GameManager.GameStates.PAUSE)
        {
            isPaused = false;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            touchOnUI = true;
        }
        else
        {
            touchOnUI = false;
        }

        switch (currentState) //check the value of currentState every frame
        {
            case PlayerStates.IDLE: //If value is PlayerState.Idle, execute following block of code till break

                DustVFX.SetActive(false);
                SetVFXValue(DustVFXGraph, 0f);
                
                AudioManager.instance.StopSound("Footsteps");


                if (GameManager.instance.currentGameState == GameManager.GameStates.GAMEPLAY)
                {
                    InitialPush();
                    if (currentState != PlayerStates.RUN)
                    {
                        currentState = PlayerStates.RUN;
                    }
                }

                break;

            case PlayerStates.RUN: //If value is PlayerState.Run, execute following block of code till break
                PCMovement(); //Call the PC Movement function so pc gameobject can move

                isRunning = true;
                isJumping = false;

                DustVFX.SetActive(true);
                SetVFXValue(DustVFXGraph, 5f);

                AudioManager.instance.PlaySound("Footsteps");

                if (Input.GetMouseButtonDown(0) && isGrounded && !isPaused && !touchOnUI) //Check If left mouse button is pressed or one finger touched screen and isGrounded is true
                {
                    PCJump(); //if both statements are true, Call the function to make the PC Jump
                }

                if (!isGrounded) //Check if the value of isGrounded is false
                {
                    if (currentState != PlayerStates.INAIR) //Check if current Player State is NOT In Air State
                    {
                        currentState = PlayerStates.INAIR; //if both conditions are true, change the current State to In Air State
                    }
                }

                break;

            case PlayerStates.INAIR: //If value is PlayerState.InAir, execute following block of code till break

                isRunning = false;

                DustVFX.SetActive(false);
                SetVFXValue(DustVFXGraph, 0f);

                AudioManager.instance.StopSound("Footsteps");

                if (isGrounded) //Check if player is grounded
                {
                    if (currentState != PlayerStates.RUN) //Check if current Player State is NOT In Run State
                    {
                        currentState = PlayerStates.RUN; //if both conditions are true, change the current State to Run State
                    }
                }

                if (Input.GetMouseButtonDown(0) && !isPaused && !touchOnUI) //Check If left mouse button is pressed or one finger touched screen
                {
                    if (currentState != PlayerStates.ABILITY) //Check if current Player State is NOT In Ability State
                    {
                        currentState = PlayerStates.ABILITY; //if both conditions are true, change the current State to Ability State
                    }
                }

                break;

            case PlayerStates.ABILITY: //If value is PlayerState.Ability, execute following block of code till break

                playerRB.gravityScale = playerGravity;

                AudioManager.instance.StopSound("Footsteps");

                if (isGrounded) //Check if player is grounded
                {
                    playerRB.gravityScale = 1f;

                    if (currentState != PlayerStates.RUN) //Check if current Player State is NOT In Run State
                    {
                        currentState = PlayerStates.RUN; //if both conditions are true, change the current State to Run State
                    }
                }

                break;

            case PlayerStates.DEAD: //If value is PlayerState.Dead, execute following block of code till break

                AudioManager.instance.StopSound("Footsteps");

                DustVFX.SetActive(false); // Deactivate the DustVFX
                DeathVFX.SetActive(true); // Activate de Death VFX
                SetVFXValue(DustVFXGraph, 0f);
                SetVFXValue(DeathVFXGraph, 5f);

                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Gets called whenever the PC collides with a Trigger Collider 
    {
        if (collision.gameObject.CompareTag("Pickup")) //Check if the gameobject we collided with, has the tag Pickup
        {
            Destroy(collision.gameObject); //If true, destroy that gameobject with tag pickup


            // show Pickup VFX
            StartCoroutine(ShowPickupVFX(collision.transform.position));
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {


        if (col.collider.CompareTag("Obstacle"))
        {
            currentState = PlayerStates.DEAD; // When the player hits an obstacle, change state to DEAD
            Debug.Log("Collided with an obstacle!!! GAME OVER");
            //UIManager.Instance.GameOver();
        }
        else if (col.collider.CompareTag("Bat"))
        {
            currentState = PlayerStates.DEAD; // When the player hits an obstacle, change state to DEAD
            Debug.Log("Collided with an obstacle!!! GAME OVER");
        }
    }
    void CheckGroundCollision()//This Function will detect if the pc is touching a ground collider
    {
        //Convert Vector3 groundCheckFrontTransform.position to a Vector2 variable, groundCheckFront
        Vector2 groundCheckFront = new Vector2(groundCheckFrontTransform.position.x, groundCheckFrontTransform.position.y);

        //Convert Vector3 groundCheckBackTransform.position to a Vector2 variable, groundCheckBack
        Vector2 groundCheckBack = new Vector2(groundCheckBackTransform.position.x, groundCheckBackTransform.position.y);

        //Check if either the front point OR the back point is touching any collider on ground layer
        if (Physics2D.OverlapPoint(groundCheckFront, whatIsGround) || Physics2D.OverlapPoint(groundCheckBack, whatIsGround))
        {
            isGrounded = true; //If true, set isGrounded to true;
        }
        else
        {
            isGrounded = false; //If false, set isGrounded to false
        }

        playerAnim.SetBool("isGrounded", isGrounded);
    } 
    #endregion

    #region Set up Code
    void InitialPush() //The function which gives the pc gameobject an initial velocity
    {
        playerRB.AddForce(new Vector2(speed, 0f), ForceMode2D.Impulse); //Add a force on the horizontal X axis ONLY, of value speed to PC rigidbody once
    } 
    #endregion

    #region Movement Code
    void PCMovement() //The function which makes the player accelerate and move ahead
    {
        ballVelocity += acceleration * Time.deltaTime; // plug in acceleration formula to get new final velocity of the ball after acceleration


        if (ballVelocity >= maxVelocity) //Checking whether pc has reached maximum velocity after acceleration
        {
            ballVelocity = maxVelocity; //If so, ball velocity sets itself to maximum velocity
        }
        else if (ballVelocity <= minVelocity) //Checking whether pc has reached velocities lower or equal to minimum velocity
        {
            ballVelocity = minVelocity; //If so, ball velocity sets itself to minimum velocity
        }

        playerRB.velocity = new Vector2(ballVelocity, playerRB.velocity.y); //Set player's velocity to ball velocity which has acceleration
    }

    void PCJump()
    {
        isJumping = true;
        playerRB.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); //Add a force on the vertical Y axis ONLY, of value jumpForce to PC rigidbody once
    }

    #endregion

    #region VFX related Code
    IEnumerator ShowPickupVFX(Vector3 contactPoint)
    {
        PickupVFX.SetActive(true);
        PickupVFX.transform.position = contactPoint; // the visual effect will be where the contact happened

        float pickupLifetime = PickupVFXGraph.GetFloat("Lifetime"); // We get the lenght of the visual effect ( its lifetime)
        PickupVFXGraph.Play();
        yield return new WaitForSeconds(pickupLifetime); // We wait until the VFX is finished playing
        PickupVFX.SetActive(false); // Then we deactivate the VFX 
    }

    public void SetVFXValue(VisualEffect vfx, float spawnValue)
    {
        vfx.SetFloat("_spawnRate", spawnValue);
    } 
    #endregion

}
