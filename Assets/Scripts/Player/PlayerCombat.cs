using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CombatBase
{
    public Action<int> DamageTaken;
    public Action PlayerDeath;

    private bool isDead = false;
    private bool canTakeDamage = true;

    public override void TakeDamage(int amount)
    {
        if (isDead || !canTakeDamage)
            return;

        //PlayerManager.playerManager.playerStats.currentHealth -= amount;
        StartCoroutine(InvulnerabilityCooldown());
        DamageTaken?.Invoke(amount);

        /*
        if (PlayerManager.playerManager.playerStats.currentHealth <= 0)
        {
            PlayerDeath?.Invoke();
            isDead = true;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bool speedCheck = PlayerManager.playerManager.playerController.rb.velocity.magnitude >= PlayerManager.playerManager.playerStats.minVelocityToAttack;
            bool dirCheck = Vector2.Dot(collision.transform.position - transform.parent.position, PlayerManager.playerManager.playerController.rb.velocity) > 0;

            if (speedCheck && dirCheck)
            {
                collision.gameObject.GetComponent<CombatBase>().TakeDamage(0);
                PlayerManager.playerManager.effectsManager.EnemyKill(collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));
                PlayerManager.playerManager.soundManager.ObjectKilled(collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));
            }
        }
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(PlayerManager.playerManager.playerStats.invulnerabilityDuration);
        canTakeDamage = true;
    }
}
