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
    public float chargeMultiplier;
    public float maxVelocity;
}
