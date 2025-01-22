using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseLocation;
    [SerializeField] private float baseOffset;
    [SerializeField] private float minArrowOffset;

    [Header("Arrow Expansion")]
    [SerializeField] private List<ArrowDot> dots = new List<ArrowDot>();
    [SerializeField] private GameObject arrow;
    [SerializeField] private float maxSpace;

    [Header("Arrow Compaction")]
    [SerializeField] private float compactionDuration;
    [SerializeField] private AnimationCurve compactionCurve;

    private void Start()
    {
        PlayerManager.playerManager.playerController.Charging += Charge;
        PlayerManager.playerManager.playerController.ChargeEnded += EndCharge;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.Charging -= Charge;
        PlayerManager.playerManager.playerController.ChargeEnded -= EndCharge;
    }

    void Update()
    {
        Vector2 mousePosition = mouseLocation.action.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mouseWorldPos - PlayerManager.playerManager.playerObj.transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void EndCharge()
    {
        StartCoroutine(Compaction());
    }


    private void Charge(float currentCharge)
    {
        float percent = currentCharge / PlayerManager.playerManager.playerStats.maxChargeDuration;
        float spacing = Mathf.Lerp(0, maxSpace, percent);

        for (int i = 0; i < dots.Count; i++)
            dots[i].gameObject.transform.parent.localPosition = new Vector3(spacing * i + baseOffset, 0, 0);

        if (spacing >= minArrowOffset)
            arrow.transform.localPosition = new Vector3(spacing * dots.Count + baseOffset, 0, 0);
        else
            arrow.transform.localPosition = new Vector3(spacing * (dots.Count - 1) + minArrowOffset + baseOffset, 0, 0);
    }

    private IEnumerator Compaction()
    {
        float percent = PlayerManager.playerManager.playerController.GetCurrentCharge() / PlayerManager.playerManager.playerStats.maxChargeDuration;
        float spacing = Mathf.Lerp(0, maxSpace, percent);

        float currentTime = 0f;
        while (currentTime < compactionDuration)
        {
            currentTime += Time.deltaTime;
            yield return null;

            float compactionSpace = spacing * compactionCurve.Evaluate(currentTime / compactionDuration);

            for (int i = 0; i < dots.Count; i++)
                dots[i].gameObject.transform.parent.localPosition = new Vector3(compactionSpace * i + baseOffset, 0, 0);

            if (compactionSpace >= minArrowOffset)
                arrow.transform.localPosition = new Vector3(compactionSpace * dots.Count + baseOffset, 0, 0);
            else
                arrow.transform.localPosition = new Vector3(compactionSpace * (dots.Count - 1) + minArrowOffset + baseOffset, 0, 0);
        }
    }
}
