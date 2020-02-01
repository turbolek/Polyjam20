using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public static Action<TargetTrigger> TriggerEntered;
    [HideInInspector]
    public Sprite Sprite { get { return _spriteRenderer.sprite; } }
    [HideInInspector]
    public Color Color { get { return _spriteRenderer.color; } }

    private SpriteRenderer _spriteRenderer;
    public GameObject CorrectAnswerMarker;

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEntered?.Invoke(this);
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
