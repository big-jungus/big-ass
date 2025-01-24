using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMelee : CombatBase
{
    [SerializeField] private int damage;
    [SerializeField] private float launchForce;
    [SerializeField] private float attackCooldown;
    private bool canAttack = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!canAttack)
                return;

            PlayerManager.playerManager.playerCombat.TakeDamage(damage);

            Vector2 launchVector = PlayerManager.playerManager.playerCombat.transform.position - transform.position;
            PlayerManager.playerManager.playerController.AttackReceived(launchVector.normalized * launchForce);

            StartCoroutine(AttackCooldown());
        }
    }

    public override void TakeDamage(int amount)
    {
        Destroy(this.gameObject);
    }

    public IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
