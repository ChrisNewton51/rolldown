using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float steadyForce = 2f;

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private bool onFloor = false;
    private int appliedForce = 2;
    private bool movingRight = false;
    private int maxHorizSpeed = 15;
    private bool jumping, negativeJump, sideJumping = false;
    private float jumpTime = 0;
    private float buttonTime = 0.2f;
    private float jumpForce = 5;
    private float gravityScale = 1.1f;
    private bool onSideR, onSideL = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Capture input from movement keys
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Debug.Log(negativeJump);

        MoveCharacter(horizontalInput, verticalInput);
    }

    void MoveCharacter(float horizInput, float verticalInput)
    {
        // Movement while touching the floor of the course
        
        if (Mathf.Abs(rb.velocity.x) < maxHorizSpeed) rb.AddForce(new Vector3(horizInput, 0, 0) * steadyForce);

        if (verticalInput < 0 && rb.velocity.z > 0)
        {
            rb.AddForce(new Vector3(0, 0, verticalInput) * steadyForce);
        }

        // Sharper turns while on floors
        if (onFloor)
        {
            if (Input.GetKeyDown(KeyCode.A) && movingRight)
            {
                rb.AddForce(new Vector3(-1, 0, 0) * appliedForce, ForceMode.Impulse);
                movingRight = false;
            }
            if (Input.GetKeyDown(KeyCode.D) && !movingRight)
            {
                rb.AddForce(new Vector3(1, 0, 0) * appliedForce, ForceMode.Impulse);
                movingRight = true;
            }
        }
        
        // On Sides
        if (onSideR && horizInput < 0)
        {
            rb.AddForce(new Vector3(0, horizInput, horizInput * 0.1f) * steadyForce);
            if (Input.GetKeyDown(KeyCode.A) && movingRight)
            {
                rb.AddForce(new Vector3(0, -1, -0.5f) * appliedForce, ForceMode.Impulse);
                movingRight = false;
            }

        }
        if (onSideL && horizInput > 0)
        {
            rb.AddForce(new Vector3(0, -horizInput, -horizInput * 0.1f) * steadyForce);
            if (Input.GetKeyDown(KeyCode.D) && !movingRight)
            {
                rb.AddForce(new Vector3(0, -1, -0.5f) * appliedForce, ForceMode.Impulse);
                movingRight = true;
            }
        }

        // Jump handling
        if (onFloor && Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            negativeJump = true;
            jumpTime = 0;
        }

        if ((onSideL || onSideR) && Input.GetKeyDown(KeyCode.Space))
        {
            sideJumping = true;
            negativeJump = true;
            jumpTime = 0;
        }
        if (jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpTime += Time.deltaTime;
        }
        if (sideJumping)
        {
            if (Mathf.Abs(transform.position.x) > 18)
            {
                rb.AddForce(new Vector3(0, 3 * Mathf.Abs(1 / transform.position.x), 0), ForceMode.Impulse);
            } else
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
            rb.AddForce(new Vector3(-transform.position.x * 0.002f, 0, 0), ForceMode.Impulse);
            jumpTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
        {
            jumping = false;
            sideJumping = false;
        }
        if (!onFloor && negativeJump) { 
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
            negativeJump = false;
        }
        if (collision.gameObject.CompareTag("SideR"))
        {
            onSideR = true;
            negativeJump = false;
        }
        if (collision.gameObject.CompareTag("SideL"))
        {
            onSideL = true;
            negativeJump = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = false;
        if (collision.gameObject.CompareTag("SideR")) onSideR = false;
        if (collision.gameObject.CompareTag("SideL")) onSideL = false;
    }
}
