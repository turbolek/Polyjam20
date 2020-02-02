using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsController : MonoBehaviour
{
    public static Action<LegsController, bool> LegsMoved;

    public Transform[] progressPositions;

    public int Progress = 1;
    public float StepAnimationTime = 1f;
    private Animator _animatorController;

    public bool IsMoving { get; private set; } = false;

    public void Init()
    {
        _animatorController = GetComponent<Animator>();
    }

    public void StepToPosition(bool forward)
    {
        Progress += forward ? 1 : -1;
        StartCoroutine(StepCoroutine(forward));
    }

    private IEnumerator StepCoroutine(bool forward)
    {
        LegsMoved?.Invoke(this, forward);

        string trigger = forward ? "forward" : "backward";
        _animatorController.SetTrigger(trigger);
        IsMoving = true;
        float xPosition = progressPositions[Progress].position.x;
        Vector2 direction = new Vector2(xPosition - transform.position.x, 0f).normalized;
        float distance = Mathf.Abs(xPosition - transform.position.x);
        float speed = distance / StepAnimationTime;
        float shift = Mathf.Abs(speed * Time.deltaTime * direction.x);
        while (shift <= distance)
        {
            transform.Translate(Vector2.right * direction * shift);
            distance -= shift;
            yield return null;
        }
        transform.position = new Vector2(xPosition, transform.position.y);
        IsMoving = false;
    }
}
