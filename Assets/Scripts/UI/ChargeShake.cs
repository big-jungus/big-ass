using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShake : MonoBehaviour
{
    [Header("Jitter Animation")]
    [SerializeField] private Vector2 maxMovementConstraints;
    [SerializeField] private AnimationCurve movementSpeedCurve;

    private Vector2 currentMovementConstraints;
    private float currentMovementSpeed;

    private bool shake;

    private Vector2 startPosition;

    [Header("Return Animation")]
    [SerializeField] private float returnDuration;
    [SerializeField] private AnimationCurve returnCurve;


    private void Start()
    {
        PlayerManager.playerManager.playerController.Charging += Shake;
        PlayerManager.playerManager.playerController.ChargeStarted += Started;
        PlayerManager.playerManager.playerController.ChargeEnded += Ended;

        startPosition = transform.localPosition;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.Charging -= Shake;
        PlayerManager.playerManager.playerController.ChargeStarted -= Started;
        PlayerManager.playerManager.playerController.ChargeEnded -= Ended;
    }

    private void Started()
    {
        shake = true;
        StartCoroutine(ShakeAnimation());
    }

    private void Shake(float currentChargeAmount)
    {
        float percent = currentChargeAmount / PlayerManager.playerManager.playerStats.maxChargeDuration;
        currentMovementSpeed = movementSpeedCurve.Evaluate(percent);
        currentMovementConstraints = Vector2.Lerp(Vector2.zero, maxMovementConstraints, percent);
    }

    private void Ended()
    {
        shake = false;

        StartCoroutine(ReturnAnimation());
    }

    private IEnumerator ShakeAnimation()
    {
        while (shake)
        {
            yield return MovePosition();
        }
    }

    private IEnumerator MovePosition()
    {
        Vector2 randLocation = new Vector2(Random.Range(-currentMovementConstraints.x, currentMovementConstraints.x), Random.Range(-currentMovementConstraints.y, currentMovementConstraints.y)) + startPosition;
        Vector2 currLocation = transform.localPosition;

        float currentTime = 0f;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime * currentMovementSpeed;
            yield return null;

            transform.localPosition = Vector2.Lerp(currLocation, randLocation, currentTime);
        }
    }

    private IEnumerator ReturnAnimation()
    {
        Vector3 startingPosition = transform.localPosition;

        float currentTime = 0f;
        while (currentTime < returnDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startingPosition, startPosition, returnCurve.Evaluate(currentTime / returnDuration));
        }
    }
}
