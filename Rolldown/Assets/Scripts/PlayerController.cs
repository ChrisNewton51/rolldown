using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;
    public float speed = 10f;
    private bool onFloor = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Capture input from A and D keys
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }

    void FixedUpdate()
    {
        // Move the character using rigidbody.AddForce()
        if (onFloor) MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 direction)
    {
        // Apply force to the rigidbody
        rb.AddForce(direction * speed);

        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForce(-Vector3.right * 5, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForce(Vector3.right * 5, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = false;
    }
}
