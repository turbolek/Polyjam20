using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShredder : MonoBehaviour
{
    public Action ShredderEntered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShredderEntered?.Invoke();
    }
}
