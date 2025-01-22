using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Jitter Animation")]
    [SerializeField] private Vector2 maxMovementConstraints;
    [SerializeField] private AnimationCurve movementSpeedCurve;

    private Vector2 currentMovementConstraints;
    private float currentMovementSpeed;

    private bool shake;

    [Header("Return Animation")]
    [SerializeField] private float returnDuration;
    [SerializeField] private AnimationCurve returnCurve;

    [Header("Squash N Stretch Variables")]
    [SerializeField] private float minVelocity;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float squashTime;
    private Coroutine squashRoutine;


    private void Start()
    {
        PlayerManager.playerManager.playerController.Charging += Shake;
        PlayerManager.playerManager.playerController.ChargeStarted += Started;
        PlayerManager.playerManager.playerController.ChargeEnded += Ended;
        PlayerManager.playerManager.playerController.VelocityUpdated += SquashNStretch;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.Charging -= Shake;
        PlayerManager.playerManager.playerController.ChargeStarted -= Started;
        PlayerManager.playerManager.playerController.ChargeEnded -= Ended;
        PlayerManager.playerManager.playerController.VelocityUpdated -= SquashNStretch;
    }

    private void Update()
    {
        RotateToDirection();
    }

    private void RotateToDirection()
    {
        if (PlayerManager.playerManager.playerController.rb.velocity.magnitude >= minVelocity)
        {
            Vector2 dir = PlayerManager.playerManager.playerController.rb.velocity;
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, 0, rotZ);
        }
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
        Vector2 randLocation = new Vector2(Random.Range(-currentMovementConstraints.x, currentMovementConstraints.x), Random.Range(-currentMovementConstraints.y, currentMovementConstraints.y));
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

            transform.localPosition = Vector3.Lerp(startingPosition, Vector3.zero, returnCurve.Evaluate(currentTime / returnDuration));
        }
    }

    private void SquashNStretch(float velocity)
    {
        if (squashRoutine != null)
            StopCoroutine(squashRoutine);

        squashRoutine = StartCoroutine(UpdateScale(velocity));
    }

    private IEnumerator UpdateScale(float velocity)
    {
        float newScale = scaleCurve.Evaluate(velocity);
        float oldScale = transform.localScale.y;

        float currentTime = 0f;
        while (currentTime < squashTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.localScale = new Vector3(1, Mathf.Lerp(oldScale, newScale, currentTime / squashTime), 1);
        }
    }
}
