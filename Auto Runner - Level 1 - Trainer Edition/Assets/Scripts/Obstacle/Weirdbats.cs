using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weirdbats : MonoBehaviour
{
    public enum WeirdBatStates
    {
        IDLE,
        SPAWNBATNORMAL,
        SPAWNBATSWARM
    }

    public float maxBatCountNormal = 5f;
    public float maxBatCountSwarm = 10f;
    public List<GameObject> activeBats;
    public GameObject batPrefab;
    public Transform batSpawnPoint;

    public WeirdBatStates currentBatState;
    public bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        currentBatState = WeirdBatStates.SPAWNBATNORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentBatState)
        {
            case WeirdBatStates.IDLE:
                break;

            case WeirdBatStates.SPAWNBATNORMAL:
                if (!isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(SpawnBatNormal());
                }

                if (activeBats.Count >= maxBatCountNormal)
                {
                    if (currentBatState != WeirdBatStates.IDLE)
                    {
                        currentBatState = WeirdBatStates.IDLE;
                    }
                }

                break;

            case WeirdBatStates.SPAWNBATSWARM:
                break;
        }
    }

    IEnumerator SpawnBatNormal()
    {
        for (int i = 0; i < maxBatCountNormal; i++)
        {
            GameObject spawnedBat = Instantiate(batPrefab, batSpawnPoint.transform.position, Quaternion.identity);
            spawnedBat.transform.parent = this.transform;
            activeBats.Add(spawnedBat);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
