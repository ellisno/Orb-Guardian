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


    [Header("Controls")]
    public KeyCode leftButton;
    public KeyCode rightButton;
    public KeyCode JumpButton;


    public AudioClip jumpNoise;
    public AudioClip walkNoise;

    private RaycastHit2D groundHit;

    private Coroutine resetAnimationTrigger;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (rb.velocity.x == 0)
        {
            animator.SetInteger("AnimState", 0);
        }
        Move();
        Jump();
    }

    #region Movement Functions
    private void Move()
    {



        if (Input.GetKey(leftButton))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (Input.GetKey(rightButton))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (rb.velocity.x > 0 || rb.velocity.x < 0)
        {
            TurnCheck();
            if (IsGrounded())
            {
                AudioManager.instance.PlaySoundEffect(walkNoise);
                animator.SetInteger("AnimState", 2);
            }

        }

    }

    private void TurnCheck()
    {


        if (rb.velocity.x > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (rb.velocity.x < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    #endregion


    private void Jump()
    {
        //button was pushed this frame
        if (Input.GetKeyDown(JumpButton) && IsGrounded())
        {
            animator.SetTrigger("Jump");
            AudioManager.instance.PlaySoundEffect(jumpNoise);
            animator.SetBool("Grounded", false);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //button is being held
        if (Input.GetKey(JumpButton))
        {

            if (jumpTimeCounter > 0 && isJumping)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else if (jumpTimeCounter == 0)
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
        if (Input.GetKeyUp(JumpButton))
        {
            isJumping = false;
            isFalling = true;
        }


        if (!isJumping && CheckForLand())
        {

            resetAnimationTrigger = StartCoroutine(Reset());
        }
        DrawGroundcheck();
    }


    #region Ground Check
    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, extraHeight, groundSensor);

        if (groundHit.collider != null)
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
