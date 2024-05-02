using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float steadyForce = 2f;
    public float airForce = 3f;
    public float sideForce = 8f;
    public float jumpForce = 0.22f;
    public float sideJumpForce = 3f;
    public float sideJumpHorizForce = 0.002f;
    public float gravityScale = 1.5f;
    public bool inRArch, inLArch = false;

    private Rigidbody rb;
    private float horizontalInput, verticalInput;
    private bool onFloor, onRamp = false;
    private int maxHorizSpeed = 20;
    private bool jumping, negativeJump, sideJumping = false;
    private float jumpTime = 0;
    private float buttonTime = 0.15f;
    private bool onSideR, onSideL = false;
    private int sideBound = 18;
    private float puncherForce = 35;
    private float courseDecline = 12;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Inputs
        DetectMoveCharacter();
    }

    void FixedUpdate()
    {
        // Physics movements
        HandleMoveCharacter(horizontalInput, verticalInput);
        rb.AddForce(Physics.gravity);
        //Debug.Log(rb.velocity.z);
    }

    void DetectMoveCharacter()
    {
        // Detect horizontal key presses
        DetectLRInput();

        // Detect jump key presses
        DetectJump();

        // Detect slow down key presses
        DetectFBInput();
    }

    void HandleMoveCharacter(float horizontalInput, float verticalInput)
    {
        // Left and right movement
        HandleLRMovement(horizontalInput);

        // Jump handling
        HandleJumping();

        // Slow down
        SlowdownHandle(verticalInput);
    }

    void DetectLRInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void HandleLRMovement(float horizontalInput)
    {
        // In the air
        if (!onFloor && !onRamp && !onSideL && !onSideR)
        {
            steadyForce = airForce;
        }


        // On floor
        if (!onSideR && !onSideL)
        {
            BasicLR(horizontalInput);
        }

        // When on sides
        if (onSideR && horizontalInput < 0)
        {
            rb.AddForce(new Vector3(horizontalInput * Mathf.Abs(sideForce / transform.position.x), horizontalInput, 0) * steadyForce);
        }
        else if (onSideR && horizontalInput > 0)
        {
            BasicLR(horizontalInput);
        }
        if (onSideL && horizontalInput > 0)
        {
            rb.AddForce(new Vector3(horizontalInput * Mathf.Abs(sideForce / transform.position.x), -horizontalInput, 0) * steadyForce);
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

    void DetectJump()
    {
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
    }

    void HandleJumping()
    {
        // Begin jump
        if (jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumpTime += Time.deltaTime;
        }
        if (sideJumping)
        {
            if (Mathf.Abs(transform.position.x) > sideBound)
            {
                if ((rb.velocity.x < 0 && onSideR) || (rb.velocity.x > 0 && onSideL))
                {
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                } else {
                    rb.AddForce(new Vector3(0, Mathf.Abs(sideJumpForce / transform.position.x), 0), ForceMode.Impulse);
                }
            }
            else
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            rb.AddForce(new Vector3(-transform.position.x * sideJumpHorizForce, 0, 0), ForceMode.Impulse);
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

    void DetectFBInput()
    {
        verticalInput = Input.GetAxis("Vertical");
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
        steadyForce = 35;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) onFloor = false;
        if (collision.gameObject.CompareTag("SideR")) onSideR = false;
        if (collision.gameObject.CompareTag("SideL")) onSideL = false;
        if (collision.gameObject.CompareTag("Ramp")) onRamp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ArchR"))
        {
            inRArch = true;
        }
        if (other.gameObject.CompareTag("ArchL"))
        {
            inLArch = true;
        }

        if (other.gameObject.CompareTag("ArchREnd"))
        {
            inRArch = false;
        }
        if (other.gameObject.CompareTag("ArchLEnd"))
        {
            inLArch = false;
        }

        // Puncher objects
        if (other.gameObject.CompareTag("Puncher"))
        {
            rb.AddForce(new Vector3(0, Mathf.Cos((courseDecline * Mathf.PI)/180), Mathf.Sin((courseDecline * Mathf.PI)/180)) * puncherForce, ForceMode.Impulse);
            negativeJump = true;
        }
    }
}
