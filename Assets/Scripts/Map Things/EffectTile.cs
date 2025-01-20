using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTile : MonoBehaviour
{
    [SerializeField] protected float maxIntervalDuration;
    protected float currentIntervalDuration;

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryEffect();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentIntervalDuration = 0;
    }

    private void TryEffect()
    {
        currentIntervalDuration += Time.deltaTime;

        if (currentIntervalDuration < maxIntervalDuration)
            return;

        currentIntervalDuration = 0;
        Effect();
    }

    protected virtual void Effect()
    {
    }
}
