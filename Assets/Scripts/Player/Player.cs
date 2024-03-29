using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpTime = 0.5f;


    [Header("Ground Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask groundSensor;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;
    private float moveInputX;

    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;

    private RaycastHit2D groundHit;

    private Coroutine resetAnimationTrigger;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator.GetComponent<Animator>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (moveInputX == 0 )
        {
            animator.SetInteger("AnimState", 0);
        }
        Move();
        Jump();
    }

    #region Movement Functions
    private void Move()
    {
        moveInputX = UserInput.instance.moveInput.x;
        

        if (moveInputX > 0 || moveInputX < 0)
        {
            TurnCheck();
            if (IsGrounded())
            {
                animator.SetInteger("AnimState", 2);
            }
            
        }

        rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);

    }

    private void TurnCheck()
    {
        

        if (UserInput.instance.moveInput.x > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (UserInput.instance.moveInput.x < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    #endregion


    private void Jump()
    {
        //button was pushed this frame
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() && IsGrounded())
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //button is being held
        if (UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            
            if(jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else if(jumpTimeCounter == 0)
            {
                isFalling = true;
                isJumping = false;
            }
            else
            {
                isJumping = false;
            }
        }

        //button is released
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
            isFalling = true;
        }

      
        if(!isJumping && CheckForLand())
        {
            
            resetAnimationTrigger = StartCoroutine(Reset());
        }
        DrawGroundcheck();
    }


    #region Ground Check
    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, extraHeight, groundSensor);

        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckForLand()
    {
        if (isFalling)
        {
            Debug.Log("Checking for land...");
            if (IsGrounded())
            {
                Debug.Log("Player landed!");
                isFalling = false;
                animator.SetInteger("AnimState", 0); // Set animation state to Idle when grounded
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    private IEnumerator Reset()
    {
        yield return null;
        animator.ResetTrigger("Jump");
        animator.SetBool("Grounded", true); // Reset any other relevant animation states

    }


    #endregion

    #region Gizmos

    private void DrawGroundcheck()
    {
        Color rayColor;

        if (IsGrounded())
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + extraHeight), Vector2.right * (playerCollider.bounds.extents.x * 2), rayColor);
    }
    #endregion
}