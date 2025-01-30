using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform t;
    Transform playerTransform;
    [SerializeField] GameObject[] stagnantObjects;

    [SerializeField] private Transform shakeTransform;

    [Header("Shake Animation")]
    [SerializeField] private AnimationCurve shakeMoveCurve;
    [SerializeField] private float moveDuration;
    [SerializeField] private List<float> shakeIntensities = new List<float>();

    [Header("Return Animation")]
    [SerializeField] private AnimationCurve returnMoveCurve;
    [SerializeField] private float returnDuration;

    private Coroutine shakeRoutine;


    public void Setup()
    {
        t = transform;
        playerTransform = PlayerManager.playerManager.playerObj.transform;

        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = PlayerManager.playerManager.playerObj.transform;
            return;
        }

        t.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10) ;

        if (stagnantObjects != null && stagnantObjects.Length > 0)
        {
            foreach (GameObject o in stagnantObjects)
            {
                o.transform.position = new Vector3(t.position.x, t.position.y, o.transform.position.z);
            }
        }
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
        Vector3 startingPos = shakeTransform.localPosition;
        Vector3 endPos = (Vector3)dir * shakeIntensities[PlayerManager.playerManager.playerController.GetCurrentSpeedTier()];

        while (currentTime < moveDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            shakeTransform.localPosition = Vector3.Lerp(startingPos, endPos, shakeMoveCurve.Evaluate(currentTime / moveDuration));
        }

        currentTime = 0f;
        startingPos = shakeTransform.localPosition;
        Vector3 counterShakePos = endPos * -0.5f;
        while (currentTime < moveDuration * 2)
        {
            yield return null;
            currentTime += Time.deltaTime;

            shakeTransform.localPosition = Vector3.Lerp(startingPos, counterShakePos, shakeMoveCurve.Evaluate(currentTime / (moveDuration * 2)));
        }

        // Return
        startingPos = shakeTransform.localPosition;
        currentTime = 0f;
        while (currentTime < returnDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            shakeTransform.localPosition = Vector3.Lerp(startingPos, Vector3.zero, returnMoveCurve.Evaluate(currentTime / returnDuration));
        }
    }
}
