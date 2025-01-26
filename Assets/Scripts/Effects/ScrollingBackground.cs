using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    Transform t;
    SpriteRenderer sr;
    [SerializeField] Vector2 scrollDirection;
    [SerializeField] float scrollSpeed;
    [SerializeField] Vector2 resetBounds;
    Vector2 temp;
    void Start()
    {
        t = transform;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        t.localPosition = (Vector2)t.localPosition + scrollDirection.normalized * scrollSpeed * Time.deltaTime;
        if(Mathf.Abs(t.localPosition.x) >= resetBounds.x){
            temp = t.localPosition;
            temp.x = 0;
            t.localPosition = temp;
        }
        if(Mathf.Abs(t.localPosition.y) >= resetBounds.y){
            temp = t.localPosition;
            temp.y = 0;
            t.localPosition = temp;
        }
    }
}
