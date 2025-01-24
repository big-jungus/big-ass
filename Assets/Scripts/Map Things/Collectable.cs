using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int score;
    public CollectableTypes collectableType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.playerManager.playerController.Pickup(this);
        }
    }

    public enum CollectableTypes
    {
        BigCoin,
        SmallCoin,
        Win,
    }
}
