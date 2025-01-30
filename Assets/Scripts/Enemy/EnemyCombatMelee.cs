using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMelee : CombatBase
{
    SpriteRenderer sr;
    [SerializeField] bool canDie = true;
    [SerializeField] private int damage;
    [SerializeField] private float launchForce;
    [SerializeField] private float attackCooldown;
    private bool canAttack = true;

    void Start(){
        sr = GetComponentInChildren<SpriteRenderer>();
    }
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
        if(canDie)
            StartCoroutine(_DeathFlash());
        // Destroy(this.gameObject);
    }

    public IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    IEnumerator _DeathFlash(){
        float deathFadeDuration = .3f;
        float timeElapsed = 0;
        float flashTime = .05f;
        int numFlashes = Mathf.CeilToInt(deathFadeDuration/flashTime);
        for(int i = 0; i < numFlashes; i++){
            sr.enabled = !sr.enabled;
            float time = 0;
            while(time < flashTime){
                yield return null;
                time += Time.deltaTime;
                timeElapsed += Time.deltaTime;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - timeElapsed / deathFadeDuration);
            }
        }
            Destroy(this.gameObject);
    }

}
