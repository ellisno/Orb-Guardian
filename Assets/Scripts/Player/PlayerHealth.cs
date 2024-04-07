using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private float maxHealth = 10f;

    public float currentHealth;

    public int knockOutCount;

    public bool hasTakenDamage { get; set; }

    private Animator animator;

    public GameObject orbPrefab;



    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void Damage(float damageAmount, int playerNumber)
    {
        animator.SetTrigger("Hurt");
        hasTakenDamage = true;
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            Invoke("Die", 3f); // Call Die() function after 3 seconds
        }
    }

    public void KnockOut()
    {
        animator.SetTrigger("Hurt");
        hasTakenDamage = true;
        knockOutCount += 1;
        if (knockOutCount >= 3)
        {
            SpawnOrb();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void SpawnOrb()
    {
        if (orbPrefab != null)
        {
            // Instantiate the orb prefab at the player's position
            GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
            // Optionally, you can perform additional setup on the orb GameObject here
            GameObject player1Object = GameObject.FindGameObjectWithTag("Player1");
            PlayerAttack player1Attack = player1Object.GetComponent<PlayerAttack>();

            GameObject player2Object = GameObject.FindGameObjectWithTag("Player2");
            PlayerAttack player2Attack = player2Object.GetComponent<PlayerAttack>();

            player1Attack.attackingPlayer = false;
            player1Attack.defendingPlayer = false;

            player2Attack.attackingPlayer = false;
            player2Attack.defendingPlayer = false;


        }
        else
        {
            Debug.LogError("Orb Prefab not assigned in the PlayerHealth script.");
        }
    }
}
