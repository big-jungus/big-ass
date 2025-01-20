using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CombatBase
{
    public override void TakeDamage(int amount)
    {
        PlayerManager.playerManager.playerStats.currentHealth -= amount;
        PlayerManager.playerManager.playerUI.UpdateHealth();
    }
}
