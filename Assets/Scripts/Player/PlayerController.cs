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
    [SerializeField] private GameObject arrow;
    public Rigidbody2D rb;

    private float currentChargeDuration;
    private bool isCharging;

    [Header("Other")]
    [SerializeField] private float minChargeSpeed;
    [SerializeField] private float stopSpeed;
    [SerializeField] private float minSpeedLockoutDuration;
    private bool canCheckSpeed;
    private Coroutine speedLockoutRoutine;

    private Vector2 chargeStartLocation;

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
        CheckMinimumSpeed();
    }

    private void Charge(InputAction.CallbackContext context)
    {
        if (rb.velocity.magnitude > minChargeSpeed)
            return;

        isCharging = true;
        arrow.SetActive(true);
    }

    private void TryCharge()
    {
        if (!isCharging)
        {
            if (charge.action.IsPressed())
                Charge(new InputAction.CallbackContext());

            return;
        }

        currentChargeDuration = Mathf.Clamp(currentChargeDuration + Time.deltaTime, 0, PlayerManager.playerManager.playerStats.maxChargeDuration);
        PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);

        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.Log(mouseWorldPos);

        Vector3 trueRot = Quaternion.LookRotation(mouseWorldPos - transform.position, Vector3.forward).eulerAngles;
        trueRot.x = 0;
        arrow.transform.eulerAngles = trueRot;
    }

    private void Release(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 dir = mouseWorldPos - new Vector2(transform.position.x, transform.position.y);
        Vector2 force = dir * (currentChargeDuration / PlayerManager.playerManager.playerStats.maxChargeDuration) * PlayerManager.playerManager.playerStats.GetChargeMultiplier();

        rb.AddForce(force, ForceMode2D.Force);
        CheckMaxSpeed();

        chargeStartLocation = transform.position;

        // Reset Charge
        currentChargeDuration = 0;
        isCharging = false;
        PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);
        arrow.SetActive(false);

        // Lock Min Speed Check
        if (speedLockoutRoutine != null)
            StopCoroutine(speedLockoutRoutine);

        speedLockoutRoutine = StartCoroutine(MinSpeedLockoutTimer());
    }

    private void Stop(InputAction.CallbackContext context)
    {
        if (!PlayerManager.playerManager.playerStats.StopUpgrade)
            return;

        rb.velocity = Vector2.zero;
        arrow.SetActive(false);
    }

    private void CheckMaxSpeed()
    {
        if (rb.velocity.magnitude > PlayerManager.playerManager.playerStats.GetMaxVelocity())
        {
            Vector2 newDir = rb.velocity.normalized * PlayerManager.playerManager.playerStats.GetMaxVelocity();
            rb.velocity = newDir;
        }
    }

    private void CheckMinimumSpeed()
    {
        if (!canCheckSpeed)
            return;

        if (rb.velocity.magnitude <= stopSpeed)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void SpikeCollisison(int damageAmount)
    {
        rb.velocity = Vector2.zero;
        transform.position = chargeStartLocation;
        PlayerManager.playerManager.playerCombat.TakeDamage(damageAmount);

        currentChargeDuration = 0;
        isCharging = false;
        PlayerManager.playerManager.playerUI.UpdateCharge(currentChargeDuration);
        arrow.SetActive(false);
    }

    private IEnumerator MinSpeedLockoutTimer()
    {
        float currentTime = 0f;
        canCheckSpeed = false;

        while (currentTime < minSpeedLockoutDuration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        canCheckSpeed = true;
    }
}