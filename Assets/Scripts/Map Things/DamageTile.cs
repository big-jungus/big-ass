using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : EffectTile
{
    [SerializeField] private int damage;

    protected override void Effect()
    {
        PlayerManager.playerManager.playerCombat.TakeDamage(damage);
    }
}
