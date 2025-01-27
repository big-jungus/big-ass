using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShake : MonoBehaviour
{
    [Header("Shake Animation")]
    [SerializeField] private AnimationCurve shakeMoveCurve;
    [SerializeField] private float moveDuration;
    [SerializeField] private List<float> shakeIntensities = new List<float>();

    [Header("Return Animation")]
    [SerializeField] private AnimationCurve returnMoveCurve;
    [SerializeField] private float returnDuration;

    private Coroutine shakeRoutine;

    private Vector3 homePosition;

    private void Start()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
        homePosition = transform.position;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeAnimation(dir));
    }

    private IEnumerator ShakeAnimation(Vector2 dir)
    {
        float currentTime = 0f;
        Vector3 startingPos = transform.position;
        Vector3 endPos = startingPos - (Vector3)(dir * shakeIntensities[PlayerManager.playerManager.playerController.GetCurrentSpeedTier()]);

        while (currentTime < moveDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startingPos, endPos, shakeMoveCurve.Evaluate(currentTime / moveDuration));
        }

        currentTime = 0f;
        Vector3 counterShakePos = startingPos + (Vector3)(dir * shakeIntensities[PlayerManager.playerManager.playerController.GetCurrentSpeedTier()]);
        startingPos = transform.position;
        while (currentTime < moveDuration * 2)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startingPos, counterShakePos, shakeMoveCurve.Evaluate(currentTime / (moveDuration * 2)));
        }

        // Return
        startingPos = transform.position;
        currentTime = 0f;
        while (currentTime < returnDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startingPos, homePosition, returnMoveCurve.Evaluate(currentTime / returnDuration));
        }
    }
}
