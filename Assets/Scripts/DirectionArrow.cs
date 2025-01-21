using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private Transform parentTransform;

    private float currentChargeDuration;
    private Coroutine expansionRoutine;

    private void Start()
    {
        PlayerManager.playerManager.playerController.ChargeStarted += StartCharge;
        PlayerManager.playerManager.playerController.ChargeEnded += EndCharge;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.ChargeStarted -= StartCharge;
        PlayerManager.playerManager.playerController.ChargeEnded -= EndCharge;
    }

    void Update()
    {
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mouseWorldPos - PlayerManager.playerManager.playerObj.transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        parentTransform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void StartCharge()
    {
        if (expansionRoutine != null)
            StopCoroutine(expansionRoutine);

        expansionRoutine = StartCoroutine(Charge());
    }

    private IEnumerator Charge()
    {
        while (true)
        {
            yield return null;

            currentChargeDuration = Mathf.Clamp(currentChargeDuration + Time.deltaTime, 0, PlayerManager.playerManager.playerStats.maxChargeDuration);
        }
    }

    private void EndCharge()
    {
        if (expansionRoutine != null)
            StopCoroutine(expansionRoutine);
    }
}
