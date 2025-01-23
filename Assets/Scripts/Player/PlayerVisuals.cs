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

    private void SquashNStretch(float velocity)
    {
        if (squashRoutine != null)
            StopCoroutine(squashRoutine);

        squashRoutine = StartCoroutine(UpdateScale(velocity));
    }

    private IEnumerator UpdateScale(float velocity)
    {
        float newScale = scaleTiers[PlayerManager.playerManager.playerController.GetCurrentSpeedTier()];
        float oldScale = transform.localScale.y;

        float currentTime = 0f;
        while (currentTime < squashTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            transform.localScale = new Vector3(Mathf.Lerp(oldScale, newScale, currentTime / squashTime), 1, 1);
        }
    }
}
