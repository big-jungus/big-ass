using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CombatBase
{
    public Action<int> DamageTaken;

    public override void TakeDamage(int amount)
    {
        PlayerManager.playerManager.playerStats.currentHealth -= amount;
        DamageTaken?.Invoke(amount);
    }
}
