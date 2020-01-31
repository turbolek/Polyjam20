using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBoard : MonoBehaviour
{
    private Vector2 _playerOriginPosition;
    private PlayerController _player;
    private TargetTrigger[] _triggers;
    public static Action<GameplayBoard> BoardFinished;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInChildren<PlayerController>();
        _playerOriginPosition = _player.transform.position;
        _triggers = GetComponentsInChildren<TargetTrigger>();
        TargetTrigger.TriggerEntered += OnTriggerEntered;
    }

    private void OnDestroy()
    {
        TargetTrigger.TriggerEntered -= OnTriggerEntered;
    }

    private void OnTriggerEntered(TargetTrigger trigger)
    {
        if (IsParentOfTrigger(trigger))
        {
            BoardFinished?.Invoke(this);
        }
    }

    private bool IsParentOfTrigger(TargetTrigger trigger)
    {
        foreach (TargetTrigger trig in _triggers)
        {
            if (trigger == trig)
            {
                return true;
            }
        }
        return false;
    }

    public void Restart()
    {
        _player.transform.position = _playerOriginPosition;
    }
}
