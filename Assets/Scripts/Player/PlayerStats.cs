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

    public float GetSpeedValue(int tier)
    {
        return speedTierValues[tier];
    }

    public float GetMaxVelocity()
    {
        return ChargeUpgrade ? maxForceMagnitude + (maxForceMagnitude * velocityUpgradeAmount) : maxForceMagnitude;
    }
}
