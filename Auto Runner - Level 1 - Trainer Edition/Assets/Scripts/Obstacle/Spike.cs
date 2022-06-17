using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public enum SpikeState
    {
        IDLE,
        UP,
        DOWN
    }

    public float upSpeed = 20f;
    public float downSpeed = 2f;

    public SpikeState currentState;

    public float waitTime = 2f;

    public Transform endPoint;
    public Transform startPoint;
    public bool isWaiting = false;
    public bool isWaitingAtTop = false;
    public float counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case SpikeState.IDLE:

                if (!isWaiting)
                {
                    isWaiting = true;
                    StartCoroutine(WaitToStart());
                }
                break;

            case SpikeState.UP:

                if (transform.position.y < endPoint.position.y)
                {
                    MoveUp();
                }
                else
                {
                    if (!isWaitingAtTop)
                    {
                        isWaitingAtTop = true;
                        StartCoroutine(WaitToDown());
                    }
                    
                }

                break;

            case SpikeState.DOWN:

                if (transform.position.y > startPoint.position.y)
                {
                    MoveDown();
                }
                else
                {
                    if (currentState != SpikeState.IDLE)
                    {
                        currentState = SpikeState.IDLE;
                    }
                }

                break;
        }
        
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(waitTime);

        if (currentState != SpikeState.UP)
        {
            currentState = SpikeState.UP;
            isWaiting = false;
        }
       
    }

    IEnumerator WaitToDown()
    {
        yield return new WaitForSeconds(1f);

        if (currentState != SpikeState.DOWN)
        {
            
            currentState = SpikeState.DOWN;
            isWaitingAtTop = false;
        }

    }

    void MoveUp()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPoint.position, upSpeed * Time.deltaTime);
    }

    void MoveDown()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint.position, downSpeed * Time.deltaTime);
    }
}
