using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : BreakableWall
{
    [SerializeField] private Collectable.CollectableTypes requiredCurrency;
    [SerializeField] private int currencyAmount;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.playerManager.playerController.rb.velocity.magnitude >= minVelocity)
            {
                if (PlayerManager.playerManager.playerStats.GetCollectableAmount(requiredCurrency) >= currencyAmount)
                {
                    PlayerManager.playerManager.playerStats.CollectableAdded(requiredCurrency, -currencyAmount);
                    PlayerManager.playerManager.playerUI.CollectableAdded(requiredCurrency);
                    DestroyWall();
                }
            }
        }
    }
}
