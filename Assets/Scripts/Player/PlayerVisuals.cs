using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Squash N Stretch Variables")]
    [SerializeField] private List<float> scaleTiers = new List<float>();
    [SerializeField] private float minVelocity;
    [SerializeField] private float squashTime;
    private Coroutine squashRoutine;


    private void Start()
    {
        PlayerManager.playerManager.playerController.VelocityUpdated += SquashNStretch;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.VelocityUpdated -= SquashNStretch;
    }

    private void SquashNStretch(float velocity, Vector2 direction)
    {
        if (squashRoutine != null)
            StopCoroutine(squashRoutine);

        squashRoutine = StartCoroutine(UpdateScale(velocity, direction));
    }

    private IEnumerator UpdateScale(float velocity, Vector2 direction)
    {
        float newScale = scaleTiers[PlayerManager.playerManager.playerController.GetCurrentSpeedTier()];
        float oldScale = transform.localScale.y;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.parent.localRotation = Quaternion.Euler(0, 0, rotZ);
        transform.localRotation = Quaternion.Euler(0, 0, -rotZ);

        float currentTime = 0f;
        while (currentTime < squashTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.parent.localScale = new Vector3(Mathf.Lerp(oldScale, newScale, currentTime / squashTime), 1, 1);
        }

        if (PlayerManager.playerManager.playerController.GetCurrentSpeedTier() == 0)
        {
            transform.parent.localEulerAngles = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }
    }
}
