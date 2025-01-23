using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CombatBase
{
    public Action<int> DamageTaken;

    public override void TakeDamage(int amount)
    {
        PlayerManager.playerManager.playerStats.currentHealth -= amount;
        DamageTaken?.Invoke(amount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bool speedCheck = PlayerManager.playerManager.playerController.rb.velocity.magnitude >= PlayerManager.playerManager.playerStats.minVelocityToAttack;
            bool dirCheck = Vector2.Dot(collision.transform.position - transform.parent.position, PlayerManager.playerManager.playerController.rb.velocity) > 0;

            if (speedCheck && dirCheck)
                collision.gameObject.GetComponent<EnemyCombat>().TakeDamage(0);
        }
    }
}
