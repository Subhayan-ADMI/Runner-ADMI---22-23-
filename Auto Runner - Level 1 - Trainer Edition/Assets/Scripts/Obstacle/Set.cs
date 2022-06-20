using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MonoBehaviour
{
    public int setID;

    public HandManager hm;
    public List<GameObject> handChildren;
    public bool setDone = false;
    public bool isDoing = false;

    // Start is called before the first frame update
    void Start()
    {
        CreepyHandObstacle[] hands = gameObject.GetComponentsInChildren<CreepyHandObstacle>();
        foreach (CreepyHandObstacle hand in hands)
        {
            handChildren.Add(hand.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setDone && isDoing)
        {
            isDoing = false;
            hm.counter += 1;
            Deactivate();
        }

        if ((setID == hm.counter) && !isDoing)
        {
            isDoing = true;
            setDone = false;

            Activate();
        }
    }

    public void Activate()
    {
        foreach (GameObject hand in handChildren)
        {
            hand.GetComponent<CreepyHandObstacle>().chCurrentState = CreepyHandObstacle.CreepyHandStates.RISEUP;
        }
    }

    void Deactivate()
    {
        foreach (GameObject hand in handChildren)
        {
            hand.GetComponent<CreepyHandObstacle>().chCurrentState = CreepyHandObstacle.CreepyHandStates.IDLE;
        }
    }
}
