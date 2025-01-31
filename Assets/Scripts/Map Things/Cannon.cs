using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private InputActionReference charge;
    [SerializeField] private float lockoutDuration;

    private bool isPlayerAttached = false;
    private bool isLockedOut = false;

    private void Start()
    {
        charge.action.canceled += Fire;
    }

    private void OnDestroy()
    {
        charge.action.canceled -= Fire;
    }

    private void Update()
    {
        if (!isPlayerAttached)
            return;

        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mouseWorldPos - PlayerManager.playerManager.playerObj.transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!isPlayerAttached)
            return;

        PlayerManager.playerManager.playerController.CannonFire();
        isPlayerAttached = false;
        StartCoroutine(LockoutTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || isPlayerAttached || isLockedOut)
            return;

        isPlayerAttached = true;
        PlayerManager.playerManager.playerController.AttachedToCannon(this);
    }

    private IEnumerator LockoutTimer()
    {
        isLockedOut = true;
        yield return new WaitForSeconds(lockoutDuration);
        isLockedOut = false;
    }
}
