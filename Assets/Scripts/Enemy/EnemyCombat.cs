using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CombatBase
{
    [SerializeField] private int damage;
    [SerializeField] private float launchForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.playerManager.playerCombat.TakeDamage(damage);

            Vector2 launchVector = PlayerManager.playerManager.playerCombat.transform.position - transform.position;
            PlayerManager.playerManager.playerController.AttackReceived(launchVector.normalized * launchForce);
        }
    }

    public override void TakeDamage(int amount)
    {
        Destroy(this.gameObject);
    }
}
