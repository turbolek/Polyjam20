using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public static Action<TargetTrigger> TriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        TriggerEntered?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Trigger");
    }

}
