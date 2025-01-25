using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [Serializable]
    public class SwitchActivatedEvent : UnityEvent
    {
    }

    [Space(10)]
    [SerializeField]
    protected SwitchActivatedEvent SwitchEvent = new SwitchActivatedEvent();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SwitchEvent.Invoke();
    }

    public void SwitchSuccess()
    {

    }
}
