using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] public float maxHealth = 10f;

    public float currentHealth;

    public int knockOutCount;

    public int lives = 3;

    public bool hasTakenDamage { get; set; }

    public float knockbackForce = 1000f; 

    public string whichRespawn;

    private int originalLayer;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip respawnSound; 
    public AudioClip spawnOrbSound;

    private Animator animator;
    public GameObject orbPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        originalLayer = gameObject.layer;
    }

    public void Damage(float damageAmount, int playerNumber)
    {
        animator.SetTrigger("Hurt"); //trigger animation
        AudioManager.instance.PlaySoundEffect(hurtSound); //play sound effect
        hasTakenDamage = true;
        currentHealth -= damageAmount; // reduce health
        if (currentHealth <= 0)
        {
            gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions"); //allow recovery without taking damage
            animator.SetTrigger("Death");
            Invoke("Die", 1f); // Call Die() function after 1 seconds
        }
    }


    public void KnockOut(Vector2 sourcePosition)
    {
        // Trigger hurt animation
        animator.SetTrigger("Hurt");

        // Play hurt sound effect
        AudioManager.instance.PlaySoundEffect(hurtSound);

        // Mark that the player has taken damage
        hasTakenDamage = true;

        // Increment knockOutCount
        knockOutCount += 1;

        // Calculate knockback direction from source position to player's position
        Vector2 knockbackDirection = (new Vector2(transform.position.x, transform.position.y) - sourcePosition).normalized;

        // Apply knockback force to the player's Rigidbody2D component
        
        Rigidbody2D playerRigidbody2D = GetComponent<Rigidbody2D>();

        if (playerRigidbody2D != null)
        {
            playerRigidbody2D.AddForce(knockbackDirection * knockbackForce);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the player GameObject.");
        }

        // Check if knockOutCount has reached 3 and spawn orb
        if (knockOutCount >= 3)
        {
            // Spawn orb
            SpawnOrb();

            // Play spawn orb sound effect
            AudioManager.instance.PlaySoundEffect(spawnOrbSound);
        }
    }

    private void Die()
    {
        lives -= 1;
        if (lives <= 0)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                // Determine which player number this script belongs to then
                // assigns opposite players playerNumber to display in the win text
                int playerNumber = gameObject.CompareTag("Player1") ? 2 : 1;
                gameManager.PlayerWins(playerNumber);
            }
            else
            {
                Debug.LogWarning("GameManager not found in the scene.");
            }
        }
        else
        {
            currentHealth = maxHealth;
            // Respawn the player after a delay
            RespawnAfterDelay();
            AudioManager.instance.PlaySoundEffect(respawnSound);
        }
    }


    private void SpawnOrb()
    {
        if (orbPrefab != null)
        {
            // Define offset to raise the orb above the player's position so it doesnt get stuck
            float yOffset = 1.0f; // Adjust this value based on how high you want the orb to spawn

            // Calculate the spawn position for the orb (slightly above the player's position)
            Vector3 spawnPosition = transform.position + Vector3.up * yOffset;

            // Instantiate the orb prefab at the calculated position
            GameObject orb = Instantiate(orbPrefab, spawnPosition, Quaternion.identity);

            // Load player methods
            GameObject player1Object = GameObject.FindGameObjectWithTag("Player1");
            PlayerAttack player1Attack = player1Object.GetComponent<PlayerAttack>();

            GameObject player2Object = GameObject.FindGameObjectWithTag("Player2");
            PlayerAttack player2Attack = player2Object.GetComponent<PlayerAttack>();

            // Set attacking and defending flags to false
            player1Attack.attackingPlayer = false;
            player1Attack.defendingPlayer = false;

            player2Attack.attackingPlayer = false;
            player2Attack.defendingPlayer = false;

            knockOutCount = 0;
        }
        else
        {
            Debug.LogError("Orb Prefab not assigned in the PlayerHealth script.");
        }
    }
    private void RespawnAfterDelay()
    {
        // Checks which player this is so it can put them in the right respawn point
        int playerNumber = gameObject.CompareTag("Player1") ? 1 : 2;
        if(playerNumber == 1)
        {
            whichRespawn = "P1Respawn";
        }
        else
        {
            whichRespawn = "P2Respawn";
        }

        GameObject respawnPoint = GameObject.Find(whichRespawn);

        if (respawnPoint != null)
        {
            // Delay before respawning 
            float respawnDelay = 1.0f; 

            StartCoroutine(RespawnCoroutine(respawnPoint.transform.position, respawnDelay));
        }
        else
        {
            Debug.LogWarning("Respawn point not found in the scene.");
        }
    }

    private IEnumerator RespawnCoroutine(Vector3 respawnPosition, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Reactivate the player GameObject at the respawn position
        transform.position = respawnPosition; // Move player to the respawn point
        animator.SetTrigger("Recover");
        animator.SetInteger("AnimState", 0);
        gameObject.layer = originalLayer;
    }


}

