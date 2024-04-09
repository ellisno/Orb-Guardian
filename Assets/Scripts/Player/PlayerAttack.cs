using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask atackableLayer;
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float attackBuffer = .15f;

    
    private List<IDamageable> damageables = new List<IDamageable>();
    private RaycastHit2D[] hits;

    private Animator animator;

    private float attackTimeCounter;

    public bool shouldCauseDamage { get; private set; } = false;

    private bool isAttacking = false;

   
    public bool attackingPlayer = false;
    public bool defendingPlayer = false;
    public int playerNumber;
   

    public KeyCode attackButton;
    public AudioClip slashNoise;

    private void Start()
    {
        animator = GetComponent<Animator>();

        attackTimeCounter = attackBuffer;

        if (gameObject.CompareTag("Player1"))
        {
            playerNumber = 1;
        }
        else
        {
            playerNumber = 2;
        }
    }

  
    private void Update()
    {
        if (!isAttacking && Input.GetKeyDown(attackButton) && attackTimeCounter >= attackBuffer)
        {
            Attack();
            AudioManager.instance.PlaySoundEffect(slashNoise);
        }

        attackTimeCounter += Time.deltaTime;
    }


    private void Attack()
    {
        attackTimeCounter = 0f;
        isAttacking = true;
        animator.SetTrigger("Attack");


    }

    //damage while slashing in animation
    public IEnumerator DamageWhileSlashing()
    {
       
        shouldCauseDamage = true;

        while (shouldCauseDamage)
        {
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, atackableLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();

                if (damageable != null && !damageable.hasTakenDamage)
                {
                    if (attackingPlayer || hits[i].collider.CompareTag("Orb"))
                    {
                       
                        damageable.Damage(damageAmount, playerNumber);
                        damageables.Add(damageable);

                    }
                    else if(defendingPlayer)
                    {
                        //*****************
                        Vector2 bumpFromPosition = gameObject.transform.position;
                        damageable.KnockOut(bumpFromPosition);
                        damageables.Add(damageable);
                    }

                }
            }

            yield return null;
        }

        MakeDamageableAgain();
        isAttacking = false;

    }

    //makes enemy damageable again after attack
    private void MakeDamageableAgain()
    {
        foreach (IDamageable wasDamaged in damageables)
        {
            wasDamaged.hasTakenDamage = false;
        }

        damageables.Clear();
    }

    //draw attack area
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }

    //should cause damage during this part of animation
    public void shouldCauseDamageToTrue()
    {
        shouldCauseDamage = true;
    }

    //stop causing damage
    public void shouldCauseDamageToFalse()
    {
        shouldCauseDamage = false;
    }
}
