using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat Stats")]
    public float currentHealth;
    public float maxHealth;
    public float damage;

    [Header("Movement Variables")]
    public float maxChargeDuration;
    [SerializeField] private float chargeMultiplier;
    [SerializeField] private float maxVelocity;

    [Header("Upgrades")]
    public bool ChargeUpgrade;
    [SerializeField] private float chargeUpgradeAmount;
    [SerializeField] private float velocityUpgradeAmount;
    public bool StopUpgrade;

    public float GetChargeMultiplier()
    {
        return ChargeUpgrade ? chargeMultiplier + (chargeMultiplier * chargeUpgradeAmount) : chargeMultiplier;
    }

    public float GetMaxVelocity()
    {
        return ChargeUpgrade ? maxVelocity + (maxVelocity * velocityUpgradeAmount) : maxVelocity;
    }
}
