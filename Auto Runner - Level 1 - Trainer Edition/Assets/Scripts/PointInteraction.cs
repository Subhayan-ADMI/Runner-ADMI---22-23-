using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds references to points in the world(CheckPoints in this case, but can be
/// reworked to check for pick ups or walls)
/// In this implementation the ball passes checkpoints and prints out to the console the name
/// of the check point passed as well as setting it inactive.
/// </summary>
public class PointInteraction : MonoBehaviour
{
    [Header("Holds references to CheckPoints in the world")]
    public Transform checkpoint1;
    public Transform checkpoint2;
    public Transform checkpoint3;

   

    private void Update()
    {
      //Checks each frame if ball has overlapped one of the checkpoints referenced.
        RunCheckPointOverlap();
    }

    //Turns checkpoints inactive when they overlapped by the Ball(PC Controller)
    //This implementation of OverlapPoint does not use LayerMask (given to students as assignment)
    private void RunCheckPointOverlap()
    {
        if (Physics2D.OverlapPoint(checkpoint1.position))
        {
            Debug.Log("Well Done,You have reached Checkpoint 1!");
            //can run other code here....
            checkpoint1.gameObject.SetActive(false);
        }
        if (Physics2D.OverlapPoint(checkpoint2.position))
        {
            Debug.Log("Well Done,You have reached Checkpoint 2!");
            //can run other code here....
            checkpoint2.gameObject.SetActive(false);
        }
        if (Physics2D.OverlapPoint(checkpoint3.position))
        {
            Debug.Log("Well Done,You have reached Checkpoint 3!");
            //can run other code here....
            checkpoint3.gameObject.SetActive(false);
        }
    }
}
