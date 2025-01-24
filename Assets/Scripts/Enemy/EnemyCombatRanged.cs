using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatRanged : CombatBase
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float launchSpeed;
    private bool canAttack = true;
    private Transform target;

    private void FixedUpdate()
    {
        if (canAttack && target != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform, false);
            bullet.transform.localPosition = Vector3.zero;
            Vector2 launchVector = (target.position - transform.position).normalized * launchSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = launchVector;

            StartCoroutine(AttackCooldown());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            target = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            target = null;
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
