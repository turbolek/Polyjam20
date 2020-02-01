using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private Transform _playerSpawnPoint;

    private PlayerController _player;
    private TargetTrigger[] _triggers;
    public static Action<GameplayBoard, bool> BoardFinished;

    public void Init()
    {
        GameObject playerGameObject = Instantiate(_playerPrefab, transform);
        playerGameObject.transform.position = _playerSpawnPoint.position;

        _player = playerGameObject.GetComponent<PlayerController>();
        _triggers = GetComponentsInChildren<TargetTrigger>();
        TargetTrigger.TriggerEntered += OnTriggerEntered;
        Deactivate();
    }

    private void OnDestroy()
    {
        TargetTrigger.TriggerEntered -= OnTriggerEntered;
    }

    private void OnTriggerEntered(TargetTrigger trigger)
    {
        if (IsParentOfTrigger(trigger))
        {
            BoardFinished?.Invoke(this, true);
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
        _player.transform.position = _playerSpawnPoint.position;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Restart();
    }
}
