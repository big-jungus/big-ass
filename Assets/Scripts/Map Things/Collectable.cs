using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int score;
    public CollectableTypes collectableType;
    public enum CollectableTypes
    {
        BigCoin,
        SmallCoin,
        Win,
    }
    [Header("Idle")]
    [SerializeField] float moveAmount = 1;
    [SerializeField] float moveFrequency = 1;
    [SerializeField] bool randomTime = true;
    Vector2 origin;
    float timeOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.playerManager.playerController.Pickup(this, collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));
            PlayerManager.playerManager.effectsManager.CollectableCollected(collision.ClosestPoint(PlayerManager.playerManager.playerObj.transform.position));
        }
    }

    void Start(){
        origin = transform.position;
        if(randomTime)
            timeOffset = Random.Range(0, 360);
    }
    void Update(){
        transform.position = origin + new Vector2(0, moveAmount * Mathf.Sin(moveFrequency * (Time.time + timeOffset)));
    }

}
