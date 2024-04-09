using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public void Damage(float damageAmount, int playerNumber);
    public void KnockOut(Vector2 bumpFromPosition);
    public bool hasTakenDamage { get; set; }
}
