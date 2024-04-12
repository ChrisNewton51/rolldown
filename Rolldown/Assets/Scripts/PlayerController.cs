using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float steadyForce = 2f;
    public float jumpForce = 0.22f;

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private bool onFloor, onRamp = false;
    private int maxHorizSpeed = 20;
    private bool jumping, negativeJump, sideJumping = false;
    private float jumpTime = 0;
    private float buttonTime = 0.15f;
    private float gravityScale = 1.27f;
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
        //Debug.Log(rb.velocity.y);

        MoveCharacter(horizontalInput, verticalInput);
    }

    void MoveCharacter(float horizontalInput, float verticalInput)
    {
        // Left and right movement
        LRMovement(horizontalInput);

        // Jump handling
        HandleJumping();

        // Slow down
        SlowdownHandle(verticalInput);
    }

    void LRMovement(float horizontalInput)
    {
        // In the air
        if (!onFloor && !onRamp && !onSideL && !onSideR)
        {
            steadyForce = 3;
        }


        // On floor
        if (!onSideR && !onSideL)
        {
            BasicLR(horizontalInput);
        }

        // When on sides
        if (onSideR && horizontalInput < 0)
        {
            rb.AddForce(new Vector3(horizontalInput * 8 * Mathf.Abs(1 / transform.position.x), horizontalInput, 0) * steadyForce);
        }
        else if (onSideR && horizontalInput > 0)
        {
            BasicLR(horizontalInput);
        }
        if (onSideL && horizontalInput > 0)
        {
            rb.AddForce(new Vector3(horizontalInput * 8 * Mathf.Abs(1 / transform.position.x), -horizontalInput, 0) * steadyForce);
        }
        else if (onSideL && horizontalInput < 0)
        {
            BasicLR(horizontalInput);
        }
    }

    void BasicLR(float horizontalInput)
    {
        if (Mathf.Abs(rb.velocity.x) < maxHorizSpeed)
        {
            rb.AddForce(new Vector3(horizontalInput, 0, 0) * steadyForce);
        }
        else if (rb.velocity.x >= maxHorizSpeed && horizontalInput < 0)
        {
            rb.AddForce(new Vector3(horizontalInput, 0, 0) * steadyForce);
        }
        else if (-rb.velocity.x >= maxHorizSpeed && horizontalInput > 0)
        {
            rb.AddForce(new Vector3(horizontalInput, 0, 0) * steadyForce);
        }
    }

    void HandleJumping()
    {
        // Detect jump
        if ((onFloor || onRamp) && Input.GetKeyDown(KeyCode.Space))
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

        // Begin jump
        if (jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumpTime += Time.deltaTime;
        }
        if (sideJumping)
        {
            if (Mathf.Abs(transform.position.x) > 18)
            {
                if ((rb.velocity.x < 0 && onSideR) || (rb.velocity.x > 0 && onSideL))
                {
                    Debug.Log("test");
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                } else {
                    rb.AddForce(new Vector3(0, Mathf.Abs(3 / transform.position.x), 0), ForceMode.Impulse);
                }
            }
            else
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            rb.AddForce(new Vector3(-transform.position.x * 0.002f, 0, 0), ForceMode.Impulse);
            jumpTime += Time.deltaTime;
        }

        // Jump is over
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonTime)
        {
            jumping = false;
            sideJumping = false;
        }
        if (!onFloor && negativeJump)
        {
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        }
    }

    void SlowdownHandle(float verticalInput)
    {
        if (verticalInput < 0 && rb.velocity.z > 0)
        {
            rb.AddForce(new Vector3(0, 0, verticalInput) * steadyForce);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
            LandReset();
        }
        if (collision.gameObject.CompareTag("SideR"))
        {
            onSideR = true;
            LandReset();
        }
        if (collision.gameObject.CompareTag("SideL"))
        {
            onSideL = true;
            LandReset();
        }
        if (collision.gameObject.CompareTag("Ramp"))
        {
            onRamp = true;
            LandReset();
        }
    }

    void LandReset()
    {
        negativeJump = false;
        steadyForce = 7;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = false;
        if (collision.gameObject.CompareTag("SideR")) onSideR = false;
        if (collision.gameObject.CompareTag("SideL")) onSideL = false;
        if (collision.gameObject.CompareTag("Ramp")) onRamp = false;
    }
}
