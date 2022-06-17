using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepyHandObstacle : MonoBehaviour
{
    public float handSpeed = 1f;
    public float maxHandHeight = 3f;
    public float minHandHeight = 0f;
    public float handWaitTime = 2f;

    public enum CreepyHandStates
    {
        IDLE,
        RISEUP,
        RISEDOWN
    }

    public CreepyHandStates chCurrentState;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1f, 0f, 1f);
        chCurrentState = CreepyHandStates.RISEUP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (chCurrentState)
        {
            case CreepyHandStates.IDLE:
                break;

            case CreepyHandStates.RISEUP:

                if (transform.localScale.y < maxHandHeight)
                {
                    transform.localScale += transform.up * handSpeed * Time.deltaTime;
                }
                else if ((transform.localScale.y >= maxHandHeight) && chCurrentState != CreepyHandStates.RISEDOWN)
                {
                    StartCoroutine(HandWait(CreepyHandStates.RISEDOWN));
                }

                break;

            case CreepyHandStates.RISEDOWN:

                if (transform.localScale.y > minHandHeight)
                {
                    transform.localScale -= transform.up * handSpeed * Time.deltaTime;
                }
                else if ((transform.localScale.y <= minHandHeight) && chCurrentState != CreepyHandStates.RISEUP)
                {
                    StartCoroutine(HandWait(CreepyHandStates.RISEUP));
                }

                break;
        }
    }

    IEnumerator HandWait(CreepyHandStates ch)
    {
        yield return new WaitForSeconds(handWaitTime);
        ChangeState(ch);
    }

    void ChangeState(CreepyHandStates ch)
    {
        if (chCurrentState != ch)
        {
            chCurrentState = ch;
        }
    }
}
