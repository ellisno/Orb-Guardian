using UnityEngine;

public class Orb : MonoBehaviour
{
    public float initialRotationSpeed = 100f; // Initial rotation speed
    public float rotationAcceleration = 10f; // Acceleration of rotation speed
    public float maxRotationSpeed = 500f; // Maximum rotation speed
    public float floatingSpeed = 1f; // Speed at which the ball floats up and down
    public float floatingHeight = 0.5f; // Maximum floating height
    public float floatingOffset = 0.5f; // Initial floating position offset
    public float maxHorizontalSpeed = 2f; // Maximum horizontal speed when moving randomly

    private Vector3 originalPosition;
    private Rigidbody2D rb;
    private float startY;
    private bool movingRight = true;
    private float currentRotationSpeed;

    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        startY = originalPosition.y;
        currentRotationSpeed = initialRotationSpeed;
    }

    void Update()
    {
        // Rotate the ball
        transform.Rotate(Vector3.forward * currentRotationSpeed * Time.deltaTime);

        // Float the ball up and down
        float newY = startY + Mathf.Sin(Time.time * floatingSpeed) * floatingHeight + floatingOffset;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Move the ball randomly horizontally
        float horizontalSpeed = movingRight ? maxHorizontalSpeed : -maxHorizontalSpeed;
        rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction if hitting platform or screen edge
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("ScreenEdge"))
        {
            movingRight = !movingRight;
            // Reset rotation speed
            currentRotationSpeed = initialRotationSpeed;
        }
    }

    void FixedUpdate()
    {
        // Accelerate rotation speed if it's not exceeding the max
        if (currentRotationSpeed < maxRotationSpeed)
        {
            currentRotationSpeed += rotationAcceleration * Time.fixedDeltaTime;
        }
    }
}
