using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat Stats")]
    public float currentHealth;
    public float maxHealth;
    public float damage;
    public float minVelocityToAttack;
    public float invulnerabilityDuration;

    [Header("Movement Variables")]
    public float maxChargeDuration;
    [SerializeField] private float chargeMultiplier;
    [SerializeField] private float maxForceMagnitude;
    public int maxChargeTier;
    [SerializeField] private List<int> speedTierValues = new List<int>();

    [Header("Upgrades")]
    public bool ChargeUpgrade;
    [SerializeField] private float chargeUpgradeAmount;
    [SerializeField] private float velocityUpgradeAmount;
    public bool StopUpgrade;

    [Header("Resources")]
    public int bigCoinCount;
    public int smallCoinCount;

    public float GetSpeedValue(int tier)
    {
        return speedTierValues[tier];
    }

    public float GetMaxVelocity()
    {
        return ChargeUpgrade ? maxForceMagnitude + (maxForceMagnitude * velocityUpgradeAmount) : maxForceMagnitude;
    }

    public void CollectableAdded(Collectable.CollectableTypes c, int amount)
    {
        switch (c)
        {
            case Collectable.CollectableTypes.SmallCoin:
                smallCoinCount += amount;
                break;
            case Collectable.CollectableTypes.BigCoin:
                bigCoinCount += amount;
                break;
        }
    }

    public int GetCollectableAmount(Collectable.CollectableTypes c)
    {
        switch (c)
        {
            case Collectable.CollectableTypes.SmallCoin:
                return smallCoinCount;
            case Collectable.CollectableTypes.BigCoin:
                return bigCoinCount;
            default:
                return 0;
        }
    }
}
