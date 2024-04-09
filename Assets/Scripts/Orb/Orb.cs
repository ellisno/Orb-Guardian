using UnityEngine;

public class Orb : MonoBehaviour
{
    public float moveSpeed = 3f;        // Speed of the orb movement
    public float bounceForce = 5f;      // Force applied when bouncing off platforms or screen edges

    private Vector2 moveDirection;      // Direction of movement 

    void Start()
    {
        // Randomize the initial movement direction
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        MoveOrb();
    }

    void MoveOrb()
    {
        // Calculate the new position based on movement direction and speed
        Vector2 newPosition = (Vector2)transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Move the orb to the new position
        transform.position = newPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if colliding with a platform or screen edge
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("ScreenEdge"))
        {
            // Reflect the movement direction upon collision
            moveDirection = Vector2.Reflect(moveDirection, collision.contacts[0].normal).normalized;

            // Apply a bounce force to simulate rebound
            GetComponent<Rigidbody2D>().AddForce(moveDirection * bounceForce, ForceMode2D.Impulse);
        }
    }
}





