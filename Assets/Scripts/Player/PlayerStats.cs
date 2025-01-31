using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat Stats")]
    //public float currentHealth;
    //public float maxHealth;
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

    public float currentLevelTimer = 0f;
    public bool timerStart = false;

    private void Update()
    {
        if (!timerStart)
            return;

        currentLevelTimer += Time.deltaTime;
        PlayerManager.playerManager.playerUI.UpdateTimer(currentLevelTimer);
    }

    public float GetSpeedValue(int tier)
    {
        return speedTierValues[tier];
    }

    public int GetTierFromSpeed(float speed)
    {
        int roundedSpeed = Mathf.RoundToInt(Mathf.Clamp(speed, 0, speedTierValues[speedTierValues.Count - 1]));

        for (int i = 0; i < speedTierValues.Count; i++)
        {
            if (roundedSpeed <= speedTierValues[i])
                return i;
        }

        return speedTierValues.Count - 1;
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
