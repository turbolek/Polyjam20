using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Velocity = 6f;
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

        _rigidbody2d.transform.Translate(Vector2.right * x * Velocity * Time.deltaTime);
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
}
