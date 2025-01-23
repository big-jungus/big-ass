using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Other References")]
    [SerializeField] private InputActionReference charge;
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private InputActionReference stop;
    public Rigidbody2D rb;
    private DirectionArrow arrow;

    private float currentChargeDuration;
    private bool canCharge;
    private bool isCharging;

    private float currentSpeedTier;

    [Header("Other")]
    [SerializeField] private float stopDrag;
    [SerializeField] private float lastBounceCheckDuration;
    [SerializeField] private float bounceTierReduction;
    private Coroutine lastBounceRoutine;

    private Vector2 chargeStartLocation;

    public Action ChargeStarted;
    public Action<float> Charging;
    public Action ChargeEnded;
    public Action<float> VelocityUpdated;
    public Action<int> SpeedTierChanged;

    public Action<Vector2> CollisionOccured;

    void Start(){
        arrow = GetComponentInChildren<DirectionArrow>();
    }

    private void OnEnable()
    {
        charge.action.started += Charge;
        charge.action.canceled += Release;
        stop.action.performed += Stop;
        canCharge = true;
    }

    private void OnDisable()
    {
        charge.action.started -= Charge;
        charge.action.canceled -= Release;
        stop.action.performed -= Stop;
    }

    private void Update()
    {
        TryCharge();
    }

    private void Charge(InputAction.CallbackContext context)
    {
        if (PlayerManager.playerManager.playerUI.isPaused)
            return;

        if (!canCharge)
            return;

        isCharging = true;
        ChargeStarted?.Invoke();
    }

    private void TryCharge()
    {
        if (PlayerManager.playerManager.playerUI.isPaused)
            return;

        if (!isCharging)
        {
            if (charge.action.IsPressed())
                Charge(new InputAction.CallbackContext());

            return;
        }

        currentChargeDuration = Mathf.Clamp(currentChargeDuration + Time.deltaTime, 0, PlayerManager.playerManager.playerStats.maxChargeDuration);
        PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);
        Charging?.Invoke(currentChargeDuration);

        float ChargePercent = (currentChargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
        int potentialTier = Mathf.Clamp(Mathf.FloorToInt(PlayerManager.playerManager.playerStats.maxChargeTier * ChargePercent - 1), 0, PlayerManager.playerManager.playerStats.maxChargeTier);
        if (potentialTier > currentSpeedTier)
        {
            currentSpeedTier = potentialTier;
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier());
        }
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (PlayerManager.playerManager.playerUI.isPaused)
            return;

        if (!canCharge)
            return;

        // Charge Meter
        float ChargePercent = (currentChargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
        currentSpeedTier = PlayerManager.playerManager.playerStats.maxChargeTier * ChargePercent - 1;
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier());

        // Movement
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 normalDir = (mouseWorldPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 launchVelocity = normalDir * PlayerManager.playerManager.playerStats.GetSpeedValue(GetCurrentSpeedTier());

        rb.velocity = launchVelocity;
        VelocityUpdated?.Invoke(rb.velocity.magnitude);

        chargeStartLocation = transform.position;


        // Reset Charge
        ChargeEnded?.Invoke();
        currentChargeDuration = 0;
        isCharging = false;
        canCharge = false;

        // Last Bounce Check
        if (lastBounceRoutine != null)
            StopCoroutine(lastBounceRoutine);

        lastBounceRoutine = StartCoroutine(MinSpeedLockoutTimer());

        arrow.Hide();
    }

    private void Stop(InputAction.CallbackContext context)
    {
        if (!PlayerManager.playerManager.playerStats.StopUpgrade)
            return;

        rb.velocity = Vector2.zero;
    }

    public void SpikeCollisison(int damageAmount)
    {
        rb.velocity = Vector2.zero;
        transform.position = chargeStartLocation;
        PlayerManager.playerManager.playerCombat.TakeDamage(damageAmount);

        currentChargeDuration = 0;
        isCharging = false;
    }

    private IEnumerator MinSpeedLockoutTimer()
    {
        float currentTime = 0f;

        while (currentTime < lastBounceCheckDuration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        rb.drag = stopDrag;

        yield return new WaitForSeconds(1);

        currentSpeedTier = 0;
        rb.velocity = Vector2.zero;
        rb.drag = 0;
        canCharge = true;
        VelocityUpdated?.Invoke(rb.velocity.magnitude);
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier());


        arrow.Show();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionOccured?.Invoke(rb.velocity);

        if (currentSpeedTier > 0)
        {
            currentSpeedTier -= bounceTierReduction;

            rb.velocity = rb.velocity.normalized * PlayerManager.playerManager.playerStats.GetSpeedValue(GetCurrentSpeedTier());
            VelocityUpdated?.Invoke(rb.velocity.magnitude);
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier());
        }

        if (lastBounceRoutine != null)
            StopCoroutine(lastBounceRoutine);

        lastBounceRoutine = StartCoroutine(MinSpeedLockoutTimer());
    }

    public float GetCurrentCharge()
    {
        return currentChargeDuration;
    }

    public int GetCurrentSpeedTier()
    {
        return Mathf.Clamp(Mathf.FloorToInt(currentSpeedTier), 0, PlayerManager.playerManager.playerStats.maxChargeTier);
    }

    public void AttackReceived(Vector2 attackDir)
    {
        rb.AddForce(attackDir, ForceMode2D.Impulse);
    }
}