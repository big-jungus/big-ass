using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    Collider2D c2d;
    SpriteRenderer sr;
    [SerializeField] protected float minVelocity;
    [SerializeField] float deathTime = .8f;
    [SerializeField] float interval = .045f;
    // [SerializeField] float offsetRadius = .5f;

    void Start(){
        c2d = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.playerManager.playerController.rb.velocity.magnitude >= minVelocity)
            {
                PlayerManager.playerManager.effectsManager.WallBreak(collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));
                PlayerManager.playerManager.soundManager.ObjectKilled(collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));

                DestroyWall();
            }
        }
    }

    protected void DestroyWall()
    {
        c2d.enabled = false;
        StartCoroutine(_DeathFlash());
        // StartCoroutine(_DeathAnim());
    }
    IEnumerator _DeathAnim(){
        StartCoroutine(_Flash());
        // StartCoroutine(_Jitter());
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
    IEnumerator _Flash(){
        float timeElapsed = 0;
        Color temp = sr.color;
        while(timeElapsed < deathTime){
            temp.a = 0;
            sr.color = temp;
            yield return new WaitForSeconds(interval);
            timeElapsed += interval;
            temp.a = 1;
            sr.color = temp;
            yield return new WaitForSeconds(interval);
            timeElapsed += interval;
        }
    }
    
    IEnumerator _DeathFlash(){
        float deathFadeDuration = .5f;
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
    // IEnumerator _Jitter(){
    //     float angle = Mathf.Deg2Rad * Random.Range(0,360);
    //     Vector2 randomAngle = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    //     Vector2 jitterPosA = (Vector2)transform.position + offsetRadius * randomAngle;
    //     Vector2 jitterPosB = (Vector2)transform.position - offsetRadius * randomAngle;

    //     float timeElapsed = 0;
    //     while(timeElapsed < deathTime){
    //         transform.position = jitterPosA;
    //         yield return new WaitForSeconds(interval);
    //         timeElapsed += interval;
    //         transform.position = jitterPosB;
    //         yield return new WaitForSeconds(interval);
    //         timeElapsed += interval;
    //     }
    // }
}
