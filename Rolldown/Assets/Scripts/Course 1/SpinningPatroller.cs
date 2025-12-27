using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPatroller : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Left limit for the obstacle's movement (Local X)")]
    public float leftBound = -10f;
    [Tooltip("Right limit for the obstacle's movement (Local X)")]
    public float rightBound = 10f;
    [Tooltip("Speed at which the object moves side to side")]
    public float moveSpeed = 5f;

    [Header("Rotation Settings")]
    [Tooltip("Speed at which the object rotates (degrees per second)")]
    public float rotationSpeed = 90f;
    [Tooltip("Axis to rotate around")]
    public Vector3 rotationAxis = Vector3.up;

    private int direction = 1;

    void Start()
    {
        // Randomize speed slightly for variety, similar to SideMovingPlatform
        moveSpeed = Random.Range(moveSpeed * 0.8f, moveSpeed * 1.2f);
        
        // Randomize starting direction
        if (Random.value > 0.5f) direction = -1;
    }

    void Update()
    {
        // 1. Handle Side-to-Side Movement
        // Calculate the next position
        float nextX = transform.localPosition.x + (moveSpeed * direction * Time.deltaTime);

        // Check boundaries and reverse direction if needed
        if (nextX >= rightBound)
        {
            direction = -1;
        }
        else if (nextX <= leftBound)
        {
            direction = 1;
        }

        // Apply movement
        transform.Translate(Vector3.right * moveSpeed * direction * Time.deltaTime, Space.World);

        // 2. Handle Rotation
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}