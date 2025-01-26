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

    [SerializeField] private ShootingTypes shootingType;
    [SerializeField] private TargetingTypes targetingType;

    [SerializeField] private Vector2 shootDir;
    [SerializeField] private int numBullets;
    [SerializeField] private float maxAngle;

    private void FixedUpdate()
    {
        if (canAttack && target != null)
        {
            switch (shootingType)
            {
                case ShootingTypes.Single:
                    SingleShot();
                    break;

                case ShootingTypes.Fan:
                    FanShot();
                    break;

                case ShootingTypes.Spread:
                    SpreadShot();
                    break;
            }

            StartCoroutine(AttackCooldown());
        }
    }

    private void SingleShot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform, false);
        bullet.transform.localPosition = Vector3.zero;
        bullet.GetComponent<Rigidbody2D>().velocity = GetLaunchVector();
    }

    private void FanShot()
    {
        Vector2 centerVector = GetLaunchVector();
        float deg = maxAngle / (numBullets - 1);

        for (int i = 0; i < numBullets; i++)
        {
            float targetAngle = (maxAngle / 2) - (deg * i);
            Vector2 newVector = Quaternion.Euler(0, 0, targetAngle) * centerVector;

            GameObject bullet = Instantiate(bulletPrefab, transform, false);
            bullet.transform.localPosition = Vector3.zero;
            bullet.GetComponent<Rigidbody2D>().velocity = newVector;
        }
    }

    private void SpreadShot()
    {
        Vector2 centerVector = GetLaunchVector();

        for (int i = 0; i < numBullets; i++)
        {
            float targetAngle = Random.Range(-1 * maxAngle / 2, maxAngle / 2);
            Vector2 newVector = Quaternion.Euler(0, 0, targetAngle) * centerVector;

            GameObject bullet = Instantiate(bulletPrefab, transform, false);
            bullet.transform.localPosition = Vector3.zero;
            bullet.GetComponent<Rigidbody2D>().velocity = newVector;
        }
    }

    private Vector2 GetLaunchVector()
    {
        switch (targetingType)
        {
            case TargetingTypes.Player:
                return (target.position - transform.position).normalized * launchSpeed;
            case TargetingTypes.Direction:
                return shootDir.normalized * launchSpeed;
            default:
                return (target.position - transform.position).normalized * launchSpeed;
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

    private enum ShootingTypes
    {
        Single,
        Fan,
        Spread
    }

    private enum TargetingTypes
    {
        Player,
        Direction
    }
}
