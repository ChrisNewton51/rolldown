using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovingPlatform : MonoBehaviour
{
    private int RPlatRBound = 10;
    private int RPlatLBound = -10;
    private float moveSpeed = 0.5f;
    private float movement;

    void Start()
    {
        moveSpeed = Random.Range(0.3f, 0.8f);
        movement = Time.deltaTime * moveSpeed;
    }
    // Update is called once per frame
    void Update()
    { 
        transform.Translate(movement, 0, 0);

        if (transform.position.x >= RPlatRBound || transform.position.x <= RPlatLBound)
        {
            movement *= -1;
        } 
    }
}
