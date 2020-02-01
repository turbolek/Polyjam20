using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration = 6f;
    public float MaxSpeed = 30f;
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    private Sprite _originalSprite;

    public void Init()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalSprite = _spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x = -1f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            x = 1f;
        }

        _rigidbody2d.velocity += (Vector2.right * x * Time.deltaTime * Acceleration);
        _rigidbody2d.velocity = new Vector2(Mathf.Clamp(_rigidbody2d.velocity.x, -MaxSpeed, MaxSpeed), _rigidbody2d.velocity.y);
    }

    public void SetSprite(Sprite sprite, Color color)
    {
        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }
        else
        {
            _spriteRenderer.sprite = _originalSprite;
        }

        _spriteRenderer.color = color;
    }

    public void ResetSpeed()
    {
        _rigidbody2d.velocity = Vector2.zero;
    }
}
