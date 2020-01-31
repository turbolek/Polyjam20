using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Velocity = 3f;
    private Rigidbody2D _rigidbody2d;
    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
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
}
