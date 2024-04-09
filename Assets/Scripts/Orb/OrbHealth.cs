using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private float maxHealth = 3f;

    private float currentHealth;

    public bool hasTakenDamage { get; set; }

    private SpriteRenderer spriteRenderer;

    private Color originalColor;


    public AudioClip hurtSound;
    public AudioClip destroyedSound;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Damage(float damageAmount, int playerNumber)
    {
        StartCoroutine(FlashWhite(0.1f, 3));
        hasTakenDamage = true;
        currentHealth -= damageAmount;
        AudioManager.instance.PlaySoundEffect(hurtSound);

        GameObject player1Object = GameObject.FindGameObjectWithTag("Player1");
        PlayerAttack player1Attack = player1Object.GetComponent<PlayerAttack>();

        GameObject player2Object = GameObject.FindGameObjectWithTag("Player2");
        PlayerAttack player2Attack = player2Object.GetComponent<PlayerAttack>();

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySoundEffect(destroyedSound);

            if (playerNumber == 1)
            {
                player1Attack.attackingPlayer = true;
                player2Attack.defendingPlayer = true;
                Die();
            }
            else
            {
                player2Attack.attackingPlayer = true;
                player1Attack.defendingPlayer = true;
                Die();
            }

        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Knockout(Vector2 bumpFromPosition)
    {
        return;
    }

    IEnumerator FlashWhite(float duration, int flashes)
    {
        for (int i = 0; i < flashes; i++)
        {
            // Set color to white
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(duration);

            // Revert to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(duration);
        }
    }

    public void KnockOut()
    {
        throw new System.NotImplementedException();
    }

    public void KnockOut(Vector2 bumpFromPosition)
    {
        throw new System.NotImplementedException();
    }
}
