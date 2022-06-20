using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public int counter = 1;

    public List<GameObject> sets;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sets.Count; i++)
        {
            Set s = sets[i].GetComponent<Set>();
            s.setID = i + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > sets.Count)
        {
            counter = 1;
        }
    }
}
