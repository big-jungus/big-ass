using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference charge;
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Rigidbody2D rb;

    [Header("Charge Variables")]
    [SerializeField] private float maxChargeDuration;
    [SerializeField] private float chargeScale;
    private float currentChargeDuration;
    private bool isCharging;

    [Header("Other")]
    [SerializeField] private float minChargeSpeed;
    [SerializeField] private float stopSpeed;

    private void OnEnable()
    {
        charge.action.started += Charge;
        charge.action.canceled += Release;
    }

    private void OnDisable()
    {
        charge.action.started -= Charge;
        charge.action.canceled -= Release;
    }

    private void Update()
    {
        TryCharge();
        CheckForce();
    }

    private void Charge(InputAction.CallbackContext context)
    {
        if (rb.velocity.magnitude > minChargeSpeed)
            return;

        isCharging = true;
    }

    private void TryCharge()
    {
        if (!isCharging)
            return;

        currentChargeDuration = Mathf.Clamp(currentChargeDuration + Time.deltaTime, 0, maxChargeDuration);

        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log(mouseWorldPos);

        Vector3 trueRot = Quaternion.LookRotation(mouseWorldPos - transform.position, Vector3.forward).eulerAngles;
        trueRot.x = 0;
        arrow.transform.eulerAngles = trueRot;
    }

    private void Release(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 dir = mouseWorldPos - new Vector2(transform.position.x, transform.position.y);
        Vector2 force = dir * (currentChargeDuration / maxChargeDuration) * chargeScale;

        rb.AddForce(force, ForceMode2D.Force);

        // Reset Charge
        currentChargeDuration = 0;
        isCharging = false;
    }

    private void CheckForce()
    {
        if (rb.velocity.magnitude <= stopSpeed)
        {
            rb.velocity = Vector2.zero;
        }
    }
}