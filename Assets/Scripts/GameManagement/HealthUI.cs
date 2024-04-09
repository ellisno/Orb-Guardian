using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject playerObject; // Reference to the player GameObject (Player1 or Player2)
    private PlayerHealth playerHealth; // Reference to the PlayerHealth script of the player

    public Image healthBarImage; // UI Image component to represent the health bar
    public Text livesText; // UI Text element to display lives

    void Start()
    {
        // Find and assign the PlayerHealth script of the associated player
        if (playerObject != null)
        {
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Player object not assigned to PlayerUIManager!");
        }

        // Update UI elements with initial values
        UpdateUI();
    }

    void Update()
    {
        // Update UI continuously (e.g., in case health or lives change during gameplay)
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update the health bar fill amount based on the player's current health percentage
        if (playerHealth != null)
        {
            // Calculate health percentage (value between 0 and 1)
            float healthPercentage = playerHealth.currentHealth / playerHealth.maxHealth;

            // Update the fill amount of the health bar image
            healthBarImage.fillAmount = healthPercentage;

            // Update the displayed lives value
            livesText.text =  playerHealth.lives.ToString();
        }
    }
}


