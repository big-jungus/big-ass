using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    #region Refs
    Transform t;
    SpriteRenderer sr;
    Animator a;
    #endregion
    public float activeTime = 0;
    public bool wobble = false;
    public bool fade = false;
    float alphaStep;
    // float animationTime;

    void Start(){
        t = transform;
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        Spawn();
    }
    public virtual void Spawn( float alpha = 1){
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
        
        if(wobble)
            Wobble();
        if(fade)
            StartCoroutine(Fade());
        else{
            StartCoroutine(Despawn());
        }
        // animationTime = a.GetCurrentAnimatorStateInfo(0).length;
    }
    IEnumerator Despawn(){
        float timeElapsed = 0;
        float despawnTime = activeTime > 0 ? activeTime : a.GetCurrentAnimatorStateInfo(0).length;                //THERE'S A CHANMCE THIS DOESN'T WORK WHEN MODIFYING ANIMATION SPEED OR WHATEVER
        while(timeElapsed < despawnTime){
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        Destroy(gameObject);
    }
    void Wobble(float duration = .25f){
        StartCoroutine(_BlackWhite(duration));
        StartCoroutine(_Wobble(duration));
    }
    IEnumerator Fade(){
        float timeElapsed = 0;
        Color color = sr.color;
        color.a = 1;
        while(timeElapsed < activeTime){
            yield return null;
            alphaStep = Time.deltaTime/activeTime;
            color.a -= alphaStep;
            sr.color = color;
            timeElapsed += Time.deltaTime;
        }
        Destroy(gameObject);
    }
    IEnumerator _Wobble(float duration){
        float angle = Random.Range(0f, 360f);
        
        float timeElapsed = 0;
        float x = .5f;
        float y = 2;
        while(timeElapsed < duration){
            yield return null;
            timeElapsed += Time.deltaTime;
            t.localScale = new Vector2(
                x - (Mathf.Sin(timeElapsed/duration * (Mathf.PI/2)) * x),
                y + (Mathf.Sin(timeElapsed/duration * (Mathf.PI/2)) * x * 2f)
            );
        t.rotation = Quaternion.Euler(0f, 0f, angle + timeElapsed*50);
        }
    }
    IEnumerator _BlackWhite(float duration){
        float timeElapsed = 0;
        while(timeElapsed < duration){
            sr.color = sr.color == Color.white ? Color.black : Color.white;
            yield return new WaitForSeconds(.1f);
            timeElapsed += Time.deltaTime;
        }
    }
}