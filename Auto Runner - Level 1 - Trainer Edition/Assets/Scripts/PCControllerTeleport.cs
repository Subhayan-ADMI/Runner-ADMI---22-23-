using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControllerTeleport : MonoBehaviour
{
    public enum PlayerStates //We made our own data type which can ONLY accept the values written within the curly brackets
    {
        IDLE, //Value to represent when PC is in IDLE State
        RUN, //Value to represent when PC is in RUN State
        INAIR, //Value to represent when PC is in INAIR State
        ABILITY, //Value to represent when PC is in ABILITY State
        DEAD //Value to represent when PC is in DEAD State
    }

    [Header("Player Control variables")] //Seperate speed variables in inspector - to make the project user friendly, not needed
    Vector3 startSwipePosition;

    [Header("Player Attributes variables")] //Seperate speed variables in inspector - to make the project user friendly, not needed
    public float speed = 5f; //stores initial speed value of pc gameobject
    float ballVelocity; //Store X axis velocity of the ball at any given time
    public float minVelocity = 10f; //Store minimum velocity pc gameobject needs to have to move forward
    public float maxVelocity = 30f; //Store maximum velocity pc gameobject can attain
    public float acceleration = 2f; //Store the amount of acceleration of pc gameobject
    public float jumpForce = 100f; //Store the amount of force by which player should jump up
    public bool isTeleporting = false;
    public float teleportDistance = 3f;
    public bool canTeleport = false;


    [Header("Player Components")] //Seperate components in inspector - to make the project user friendly, not needed
    Rigidbody2D playerRB; //store Rigidbody2D Component of pc gameobject
    CircleCollider2D playerCollider;

    [Header("State Machine Variables")] //Seperate components in inspector - to make the project user friendly, not needed
    public PlayerStates currentState; //make a variable of data type PlayerState

    [Header("Player Collision Variables")] //Seperate components in inspector - to make the project user friendly, not needed
    public LayerMask whatIsGround; //make the public variable to assign which unity layer is to be considered as ground
    public bool isGrounded; //boolean to store if pc is colliding with anything on Ground Layer
    public Transform groundCheckFrontTransform; //Stores the GroundCheckFront Transform in inspector
    public Transform groundCheckBackTransform; //Stores the GroundCheckBack Transform in inspector

    [Header("Game State")] //Seperate components in inspector - to make the project user friendly, not needed
    public bool isPaused;

    void Awake() //Called before Start Function - use this for initializing component variables
    {
        playerRB = GetComponent<Rigidbody2D>(); //Access the Rigidbody2D component and store all properties in playerRB when game starts
        playerCollider = GetComponent<CircleCollider2D>();
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
        CheckFront();

        if (GameManager.instance.currentGameState == GameManager.GameStates.PAUSE)
        {
            isPaused = true;
        }
        else if (GameManager.instance.currentGameState != GameManager.GameStates.PAUSE)
        {
            isPaused = false;
        }

        switch (currentState) //check the value of currentState every frame
        {
            case PlayerStates.IDLE: //If value is PlayerState.Idle, execute following block of code till break

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

                if (Input.GetMouseButtonDown(0) && isGrounded && !isPaused)
                {
                    startSwipePosition = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(0) && isGrounded && !isPaused)
                {
                    CalculateSwipe(Input.mousePosition);
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

                if (isGrounded) //Check if player is grounded
                {
                    if (currentState != PlayerStates.RUN) //Check if current Player State is NOT In Run State
                    {
                        currentState = PlayerStates.RUN; //if both conditions are true, change the current State to Run State
                    }
                }

                break;

            case PlayerStates.ABILITY: //If value is PlayerState.Ability, execute following block of code till break

                if (!isTeleporting)
                {
                    isTeleporting = true;
                    Vector2 previousVelocity = playerRB.velocity;

                    playerRB.velocity = Vector2.zero;
                    playerRB.gravityScale = 0f;
                    playerCollider.enabled = false;
                    transform.position = new Vector3(transform.position.x + teleportDistance, transform.position.y, transform.position.z);

                    StartCoroutine(ResetTeleport(previousVelocity));
                }
               
               

                break;

            case PlayerStates.DEAD: //If value is PlayerState.Dead, execute following block of code till break
                break;
            
        }
    }

    void InitialPush() //The function which gives the pc gameobject an initial velocity
    {
        playerRB.AddForce(new Vector2(speed, 0f), ForceMode2D.Impulse); //Add a force on the horizontal X axis ONLY, of value speed to PC rigidbody once
    }

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
        playerRB.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); //Add a force on the vertical Y axis ONLY, of value jumpForce to PC rigidbody once
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
    }

    private void OnTriggerEnter2D(Collider2D collision) //Gets called whenever the PC collides with a Trigger Collider 
    {
        if (collision.gameObject.CompareTag("Pickup")) //Check if the gameobject we collided with, has the tag Pickup
        {
            Destroy(collision.gameObject); //If true, destroy that gameobject with tag pickup
        }
    }

    void CalculateSwipe(Vector3 finalPos)
    {
        float distanceX = Mathf.Abs(startSwipePosition.x - finalPos.x);
        float distanceY = Mathf.Abs(startSwipePosition.y - finalPos.y);

        if (distanceX > 0 || distanceY > 0)
        {
            if (distanceX > distanceY)
            {
                if (startSwipePosition.x < finalPos.x)
                {
                    if (currentState != PlayerStates.ABILITY && canTeleport) //Check if current Player State is NOT In Ability State
                    {
                        currentState = PlayerStates.ABILITY; //if both conditions are true, change the current State to Ability State
                    }
                    
                }
            }
            else
            {
                if (startSwipePosition.y < finalPos.y)
                {
                    PCJump();
                }
            }
        }
    }

    IEnumerator ResetTeleport(Vector2 originalSpeed)
    {
        yield return new WaitForSeconds(0.2f);

        isTeleporting = false;
        playerCollider.enabled = true;
        playerRB.gravityScale = 1f;
        playerRB.velocity = originalSpeed;

        if (currentState != PlayerStates.RUN)
        {
            currentState = PlayerStates.RUN;
        }
    }

    void CheckFront()
    {
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right, teleportDistance + 0.5f, whatIsGround);

        if (hit)
        {
            canTeleport = false;
        }
        else
        {
            canTeleport = true;
        }
    }
}
