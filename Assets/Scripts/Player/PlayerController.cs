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
    public DirectionArrow arrow;

    private float currentChargeDuration;
    private bool canCharge = true;
    private bool isCharging = false;
    private bool isCannon = false;

    private float currentSpeedTier;

    private Coroutine cannonCharge;

    [Header("Other")]
    [SerializeField] private float stopDrag;
    [SerializeField] private float lastBounceCheckDuration;
    [SerializeField] private float bounceTierReduction;
    [SerializeField] private float chargeResetTimer;
    private Coroutine lastBounceRoutine;

    private Vector2 chargeStartLocation;

    public Action ChargeStarted;
    public Action<float> Charging;
    public Action ChargeEnded;
    public Action<float, Vector2> VelocityUpdated;
    public Action<int, bool> SpeedTierChanged;
    public Action<Vector2, Collision2D> CollisionOccured;
    public Action<Vector3, Collectable.CollectableTypes> CollectableCollected;
    public Action CannonShot;

    void Start(){
        arrow = GetComponentInChildren<DirectionArrow>();
    }

    private void OnEnable()
    {
        charge.action.started += Charge;
        charge.action.canceled += Release;
        stop.action.performed += Stop;
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

        if (!canCharge || isCannon)
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

        if (isCannon)
            return;

        currentChargeDuration = Mathf.Clamp(currentChargeDuration + Time.deltaTime, 0, PlayerManager.playerManager.playerStats.maxChargeDuration);
        PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);
        Charging?.Invoke(currentChargeDuration);

        float ChargePercent = (currentChargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
        int potentialTier = Mathf.Clamp(Mathf.FloorToInt(PlayerManager.playerManager.playerStats.maxChargeTier * ChargePercent), 0, PlayerManager.playerManager.playerStats.maxChargeTier - 1);
        if (potentialTier > currentSpeedTier)
        {
            currentSpeedTier = potentialTier;
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), true);
        }
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (PlayerManager.playerManager.playerUI.isPaused)
            return;

        if (!canCharge || isCannon)
            return;

        // Charge Meter
        float ChargePercent = (currentChargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration);
        currentSpeedTier = PlayerManager.playerManager.playerStats.maxChargeTier * ChargePercent - 1;
        //SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), true);

        // Movement
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 normalDir = (mouseWorldPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 launchVelocity = normalDir * PlayerManager.playerManager.playerStats.GetSpeedValue(GetCurrentSpeedTier());

        rb.velocity = launchVelocity;
        VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);

        chargeStartLocation = transform.position;

        PlayerManager.playerManager.soundManager.ChargeReleased(transform.position);

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

        if (isCharging)
        {
            currentChargeDuration = 0;
            currentSpeedTier = 0;
            isCharging = false;

            StartCoroutine(ChargeResetTimer());

            PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), false);
            ChargeEnded?.Invoke();
        }
        else
        {
            currentSpeedTier = 0;
            rb.velocity = Vector2.zero;
            rb.drag = 0;
            canCharge = true;
            VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), false);

            currentChargeDuration = 0;
            PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);

            arrow.Show();
        }
    }

    private IEnumerator ChargeResetTimer()
    {
        canCharge = false;
        yield return new WaitForSeconds(chargeResetTimer);
        canCharge = true;
    }

    public void SpikeCollisison(int damageAmount)
    {
        rb.velocity = Vector2.zero;
        transform.position = chargeStartLocation;
        PlayerManager.playerManager.playerCombat.TakeDamage(damageAmount);

        currentSpeedTier = 0;
        rb.velocity = Vector2.zero;
        rb.drag = 0;
        canCharge = true;
        VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), false);

        if (lastBounceRoutine != null)
            StopCoroutine(lastBounceRoutine);

        arrow.Show();
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
        VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), false);

        arrow.Show();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionOccured?.Invoke(rb.velocity.normalized, collision);

        if (currentSpeedTier > 0)
        {
            currentSpeedTier -= bounceTierReduction;

            rb.velocity = rb.velocity.normalized * PlayerManager.playerManager.playerStats.GetSpeedValue(GetCurrentSpeedTier());
            rb.drag = 0f;
            VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);
            SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), false);
        }

        if (currentSpeedTier <= 0)
            rb.drag = stopDrag;

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


    public void Pickup(Collectable c, Vector3 location){
        //pickup
        PlayerManager.playerManager.CollectableCollected(c);
        CollectableCollected?.Invoke(location, c.collectableType);
        Destroy(c.gameObject);
    }
    public void BulletCollisison(Bullet b)
    {
        PlayerManager.playerManager.playerCombat.TakeDamage(b.damageAmount);
        Destroy(b.gameObject);
    }

    public void AttachedToCannon(Cannon cannon)
    {
        if (lastBounceRoutine != null)
            StopCoroutine(lastBounceRoutine);

        canCharge = true;
        rb.drag = 0f;

        isCannon = true;

        arrow.Show();
        arrow.shouldFlash = false;

        PlayerManager.playerManager.playerUI.UpdateCharge(PlayerManager.playerManager.playerStats.maxChargeDuration);
        ChargeStarted?.Invoke();
        cannonCharge = StartCoroutine(CannonChargeEventCall());

        rb.velocity = Vector2.zero;
        PlayerManager.playerManager.playerObj.transform.position = cannon.transform.position;

        currentChargeDuration = PlayerManager.playerManager.playerStats.maxChargeDuration;
        currentSpeedTier = PlayerManager.playerManager.playerStats.maxChargeTier - 1;
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), true);
    }

    private IEnumerator CannonChargeEventCall()
    {
        while (true)
        {
            yield return null;
            Charging?.Invoke(PlayerManager.playerManager.playerStats.maxChargeDuration);
        }
    }

    public void CannonFire()
    {
        if (cannonCharge != null)
            StopCoroutine(cannonCharge);

        CannonShot?.Invoke();
        isCannon = false;
        arrow.shouldFlash = true;

        Release(new InputAction.CallbackContext());
    }

    public void OnPushTile(Vector2 pushForce)
    {
        rb.AddForce(pushForce);

        VelocityUpdated?.Invoke(rb.velocity.magnitude, rb.velocity.normalized);

        currentSpeedTier = PlayerManager.playerManager.playerStats.GetTierFromSpeed(rb.velocity.magnitude);
        SpeedTierChanged?.Invoke(GetCurrentSpeedTier(), true);
    }
}