using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Animator _animator;
    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        _animator.SetTrigger("hide");
    }

    public void Show()
    {
        _animator.SetTrigger("show");
    }
}
