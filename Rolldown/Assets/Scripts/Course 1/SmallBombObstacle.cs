using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBombObstacle : NetworkBehaviour
{
    private float speed;

    private int rightBound = 39;
    private int leftBound = -39;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 0)
        {
            speed = 8;
        } else
        {
            speed = -8;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure only the server runs this logic
        if (!IsServerInitialized) return;

        transform.Translate(Vector3.right * Time.deltaTime * speed);
        if (transform.position.x > rightBound || transform.position.x < leftBound)
        {
            Despawn();
        } 
    }
}
