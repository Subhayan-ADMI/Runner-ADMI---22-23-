using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bats : MonoBehaviour
{ 

    public float batSpeed = 2f;
    public float maxXDistance = 1f;
    public float maxYDistance = 3f;
    Vector3 newPosition;
    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        PositionChangeAtStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.localPosition, newPosition) < 1f && !isMoving)
        {
            isMoving = true;
            StartCoroutine(PositionUpdate());
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * batSpeed);
    }

    void PositionChangeAtStart()
    {
        newPosition = new Vector2(Random.Range(-maxXDistance, maxXDistance), Random.Range(0f, maxYDistance));
    }

    IEnumerator PositionUpdate()
    {
        yield return new WaitForSeconds(3f);

        newPosition = new Vector2(Random.Range(-maxXDistance, maxXDistance), Random.Range(0.5f, maxYDistance));
        isMoving = false;
    }
}
