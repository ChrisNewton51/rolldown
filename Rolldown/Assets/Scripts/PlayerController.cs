using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float steadyForce = 2f;

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private bool onFloor, onRamp = false;
    private int appliedForce = 2;
    private bool movingRight = false;
    private int maxHorizSpeed = 15;
    private bool jumping, negativeJump, sideJumping = false;
    private float jumpTime = 0;
    private float buttonTime = 0.15f;
    private float jumpForce = 0.22f;
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
        Debug.Log(rb.velocity.x);

        MoveCharacter(horizontalInput, verticalInput);
    }

    void MoveCharacter(float horizInput, float verticalInput)
    {
        // Left and right movement
        if (!onSideR && !onSideL)
        {
            LRMovement(horizInput, verticalInput);
        }
        
        //onSideR = (transform.position.x > 16.5) ? true : false;
        //onSideL = (transform.position.x < -16.5) ? true : false;

        // Always be able to slow down
        if (verticalInput < 0 && rb.velocity.z > 0)
        {
            rb.AddForce(new Vector3(0, 0, verticalInput) * steadyForce);
        }
        
        // On Sides
        if (onSideR && horizInput < 0)
        {
            rb.AddForce(new Vector3(horizInput * 5 * Mathf.Abs(1 / transform.position.x), horizInput, 0) * steadyForce);
        } else if (onSideR && horizInput > 0) 
        {
            LRMovement(horizInput, verticalInput);
        }
        if (onSideL && horizInput > 0)
        {
            rb.AddForce(new Vector3(horizInput * 5 * Mathf.Abs(1 / transform.position.x), -horizInput, 0) * steadyForce);
        } else if (onSideL && horizInput < 0)
        {
            LRMovement(horizInput, verticalInput);
        }

        // Jump handling
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
        if (jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumpTime += Time.deltaTime;
        }
        if (sideJumping)
        {
            if (Mathf.Abs(transform.position.x) > 18)
            {
                rb.AddForce(new Vector3(0, 5 * Mathf.Abs(1 / transform.position.x), 0), ForceMode.Impulse);
            } else
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            rb.AddForce(new Vector3(-transform.position.x * 0.003f, 0, 0), ForceMode.Impulse);
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

    void LRMovement(float horizInput, float verticalInput)
    {
        if (Mathf.Abs(rb.velocity.x) < maxHorizSpeed)
        {
            rb.AddForce(new Vector3(horizInput, 0, 0) * steadyForce);
        }
        else if (rb.velocity.x >= maxHorizSpeed && horizInput < 0)
        {
            rb.AddForce(new Vector3(horizInput, 0, 0) * steadyForce);
        }
        else if (-rb.velocity.x >= maxHorizSpeed && horizInput > 0)
        {
            rb.AddForce(new Vector3(horizInput, 0, 0) * steadyForce);
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
        if (collision.gameObject.CompareTag("Ramp"))
        {
            onRamp = true;
            negativeJump = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = false;
        if (collision.gameObject.CompareTag("SideR")) onSideR = false;
        if (collision.gameObject.CompareTag("SideL")) onSideL = false;
        if (collision.gameObject.CompareTag("Ramp")) onRamp = false;
    }
}
