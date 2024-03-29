using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput,verticalInput;
    public float turnSpeed = 10000;
    private Rigidbody rb;
    private float currentX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(-turnSpeed);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(turnSpeed);
        }

    }

    private void Move(float velocity)
    {
        currentX = transform.position.x + 1;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentX, transform.position.y, transform.position.z), velocity * Time.deltaTime);
    }
}
