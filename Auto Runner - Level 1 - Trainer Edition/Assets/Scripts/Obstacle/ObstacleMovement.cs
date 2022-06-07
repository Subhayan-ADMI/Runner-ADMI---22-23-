using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the obstacle along a path determined by waypoints in the scene
/// </summary>
public class ObstacleMovement : MonoBehaviour
{
    #region Fields

    [Header("Collection of waypoints to represent a path")]
    public Transform[] waypoints;

    [Header("Obstacle Movement Speed")]
    public float speed;

    //counter variable that determines the next waypoint to move to
    int nextWaypoint = 1; 
    #endregion

    #region Monobehaviour Callbacks
    private void Start()
    {
        //sets the obstacle position to the position of the first waypoint
        InitObstacle();
    }


    private void Update()
    {
        //moves obstacle using Vector2.MoveTowards() from one waypoint to the next and loops
        MoveByWaypoints();
    }
    #endregion

    #region Private Methods
    private void InitObstacle()
    {
        transform.position = waypoints[0].transform.position;
    }
    private void MoveByWaypoints()
    {
        //move to next waypoint
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[nextWaypoint].position, speed);

        //when there move to the next
        if (transform.position == waypoints[nextWaypoint].position)

        {
            nextWaypoint++;

            //if at the end of the array, loop over
            if (nextWaypoint >= waypoints.Length)
            {
                nextWaypoint = 0;
            }
        }
    } 

    #endregion
}
