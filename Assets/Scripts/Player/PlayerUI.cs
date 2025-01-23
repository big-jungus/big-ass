using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Health Hearts")]
    [SerializeField] private Transform healthHolder;
    [SerializeField] private GameObject healthHeartPrefab;
    [SerializeField] private float spaceAmount;
    private List<HealthHeart> healthHearts = new List<HealthHeart>();

    [Header("UI References")]
    [SerializeField] private Slider chargeBar;

    [SerializeField] private TMP_Text velocityDebug;
    [SerializeField] private TMP_Text chargeDebug;

    private void Start()
    {
        PlayerManager.playerManager.playerCombat.DamageTaken += UpdateHealth;

        SetupHealth();
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

    private void SetupHealth()
    {
        for (int i = 0; i < PlayerManager.playerManager.playerStats.maxHealth; i++)
        {
            AddHealthHeart();
        }

        UpdateHealth(0);
    }

    public void AddHealthHeart()
    {
        GameObject health = Instantiate(healthHeartPrefab, healthHolder, false);
        health.transform.localPosition = new Vector3(healthHolder.childCount * spaceAmount, 0, 0);
        healthHearts.Add(health.GetComponent<HealthHeart>());
    }

    public void RemoveHealthHeart()
    {
        healthHearts[healthHearts.Count - 1].RemoveHeart();
        healthHearts.RemoveAt(healthHearts.Count - 1);
    }

    private void ResetHealthPositions()
    {
        for (int i = 0; i < healthHearts.Count; i++)
        {
            healthHearts[i].transform.localPosition = new Vector3(i * spaceAmount, 0, 0);
        }
    }

    public void UpdateHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            RemoveHealthHeart();
        }
    }

    public void UpdateCharge(float currentCharge)
    {
        chargeBar.value = currentCharge / PlayerManager.playerManager.playerStats.maxChargeDuration;
    }
}
