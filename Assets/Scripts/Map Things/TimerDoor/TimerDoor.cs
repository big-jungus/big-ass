using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDoor : MonoBehaviour
{
    [SerializeField] private Transform[] doors = new Transform[2];
    [SerializeField] private Vector3[] doorClosedLocations = new Vector3[2];
    [SerializeField] private Vector3[] doorRetractedLocations = new Vector3[2];

    [Header("Door Open")]
    [SerializeField] private float doorOpenDuration;

    [Header("Door Close")]
    [SerializeField] private float doorCloseDuration;

    private bool isDoorActivated = false;

    public void SwitchActivated(Switch caller)
    {
        if (isDoorActivated)
            return;

        caller.SwitchSuccess();
        isDoorActivated = true;

        StartCoroutine(DoorOpenAnimation());
    }

    private IEnumerator DoorOpenAnimation()
    {
        float currentTime = 0f;
        while (currentTime < doorOpenDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            doors[0].transform.localPosition = Vector3.Lerp(doorClosedLocations[0], doorRetractedLocations[0], currentTime / doorOpenDuration);
            doors[1].transform.localPosition = Vector3.Lerp(doorClosedLocations[1], doorRetractedLocations[1], currentTime / doorOpenDuration);
        }

        StartCoroutine(DoorCloseAnimation());
    }

    private IEnumerator DoorCloseAnimation()
    {
        float currentTime = 0f;
        while (currentTime < doorCloseDuration)
        {
            yield return null;
            currentTime += Time.deltaTime;

            doors[0].transform.localPosition = Vector3.Lerp(doorRetractedLocations[0], doorClosedLocations[0], currentTime / doorCloseDuration);
            doors[1].transform.localPosition = Vector3.Lerp(doorRetractedLocations[1], doorClosedLocations[1], currentTime / doorCloseDuration);
        }

        isDoorActivated = false;
    }
}
