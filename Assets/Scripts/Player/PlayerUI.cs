using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider chargeBar;

    [SerializeField] private TMP_Text velocityDebug;
    [SerializeField] private TMP_Text chargeDebug;

    private void Start()
    {
        PlayerManager.playerManager.playerCombat.DamageTaken += UpdateHealth;

        UpdateHealth(0);
        UpdateCharge(0);
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerCombat.DamageTaken -= UpdateHealth;
    }

    private void Update()
    {
        velocityDebug.text = "Velocity: " + PlayerManager.playerManager.playerController.rb.velocity.magnitude.ToString();
        chargeDebug.text = "Charge: " + (chargeBar.value / chargeBar.maxValue).ToString() + "%";
    }

    public void UpdateHealth(int amount)
    {
        healthBar.value = PlayerManager.playerManager.playerStats.currentHealth / PlayerManager.playerManager.playerStats.maxHealth;
    }

    public void UpdateCharge(float currentCharge)
    {
        chargeBar.value = currentCharge / PlayerManager.playerManager.playerStats.maxChargeDuration;
    }
}
