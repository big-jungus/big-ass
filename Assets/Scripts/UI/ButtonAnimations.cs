using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float scaleDuration;
    [SerializeField] private float enterScale;
    [SerializeField] private float exitScale;
    [SerializeField] private float downScale;
    [SerializeField] private float upScale;
    private Coroutine animRoutine;

    private bool isDown;
    private bool isInside;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayAnimation(downScale);
        isDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAnimation(enterScale);
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;

        if (isDown)
            return;

        PlayAnimation(exitScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInside)
            PlayAnimation(upScale);
        else
            PlayAnimation(exitScale);

        isDown = false;
    }

    private void PlayAnimation(float endScale)
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(ScaleAnimation(endScale));
    }

    private IEnumerator ScaleAnimation(float endScale)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 goalScale = new Vector3(endScale, endScale, endScale);

        float currentTime = 0f;

        while (currentTime < scaleDuration)
        {
            yield return null;
            currentTime += Time.unscaledDeltaTime;

            transform.localScale = Vector3.Lerp(currentScale, goalScale, scaleCurve.Evaluate(currentTime / scaleDuration));
        }
    }
}
