using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private Transform parentTransform;

    

    void Update()
    {
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mouseWorldPos - PlayerManager.playerManager.playerObj.transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        parentTransform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
