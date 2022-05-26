using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    [Header("Player Speed variables")] //Seperate speed variables in inspector - to make the project user friendly, not needed
    public float speed = 5f; //stores initial speed value of pc gameobject
    float ballVelocity; //Store X axis velocity of the ball at any given time
    public float minVelocity = 10f; //Store minimum velocity pc gameobject needs to have to move forward
    public float maxVelocity = 30f; //Store maximum velocity pc gameobject can attain
    public float acceleration = 2f; //Store the amount of acceleration of pc gameobject

    [Header("Player Components")] //Seperate components in inspector - to make the project user friendly, not needed
    Rigidbody2D playerRB; //store Rigidbody2D Component of pc gameobject

    void Awake() //Called before Start Function - use this for initializing component variables
    {
        playerRB = GetComponent<Rigidbody2D>(); //Access the Rigidbody2D component and store all properties in playerRB when game starts
    }

    // Start is called before the first frame update
    void Start()
    {
        InitialPush(); //Call the initial push function here
    }

    // Update is called once per frame
    void Update()
    {
        ballVelocity = playerRB.velocity.x; //Update ballVelocity value with pc gameobject's physics X axis velocity every frame

        PCMovement(); //Call the PC Movement function so pc gameobject can move
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
}
