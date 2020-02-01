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
    private PlayerShredder _shredder;
    public static Action<GameplayBoard, TargetTrigger, bool> BoardFinished;

    private Sprite[] _targetSprites;
    private Color[] _targetColors;

    private TargetTrigger _correctTrigger;

    public void Init(Sprite[] targetSprites, Color[] targetColors)
    {
        _targetSprites = targetSprites;
        _targetColors = targetColors;

        GameObject playerGameObject = Instantiate(_playerPrefab, transform);
        playerGameObject.transform.position = _playerSpawnPoint.position;

        _shredder = GetComponentInChildren<PlayerShredder>();
        _shredder.ShredderEntered += OnShredderEntered;

        _player = playerGameObject.GetComponent<PlayerController>();
        _player.Init();

        _triggers = GetComponentsInChildren<TargetTrigger>();

        foreach (TargetTrigger trigger in _triggers)
        {
            trigger.Init();
        }

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
            BoardFinished?.Invoke(this, trigger, trigger == _correctTrigger || _correctTrigger == null);
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

    public void Restart(TargetTrigger previousTrigger)
    {
        List<TargetTrigger> triggersToSet = new List<TargetTrigger>(_triggers).Shuffle();
        List<Sprite> availableSprites = new List<Sprite>(_targetSprites).Shuffle();
        List<Color> availableColors = new List<Color>(_targetColors).Shuffle();


        if (previousTrigger != null)
        {
            _correctTrigger = triggersToSet[0];
            _correctTrigger.CorrectAnswerMarker.SetActive(true);
            triggersToSet.RemoveAt(0);

            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {
                Sprite previousSprite = GetSameSpriteFromPool(availableSprites, previousTrigger.Sprite);
                availableSprites.Remove(previousSprite);

                Color correctColor = GetSameColorFromPool(availableColors, previousTrigger.Color);
                _correctTrigger.SetColor(correctColor);
                availableColors.Remove(correctColor);

                _correctTrigger.SetSprite(availableSprites[0]);
                availableSprites.RemoveAt(0);
            }
            else
            {
                Color previousColor = GetSameColorFromPool(availableColors, previousTrigger.Color);
                availableColors.Remove(previousColor);

                Sprite correctSprite = GetSameSpriteFromPool(availableSprites, previousTrigger.Sprite);
                _correctTrigger.SetSprite(correctSprite);
                availableSprites.Remove(correctSprite);

                _correctTrigger.SetColor(availableColors[0]);
                availableColors.RemoveAt(0);
            }
        }
        else
        {
            _correctTrigger = null;
        }

        for (int i = triggersToSet.Count - 1; i >= 0; i--)
        {
            triggersToSet[i].SetColor(availableColors[i]);
            triggersToSet[i].SetSprite(availableSprites[i]);
            triggersToSet[i].CorrectAnswerMarker.SetActive(false);
            availableColors.RemoveAt(i);
            availableSprites.RemoveAt(i);
        }

        _player.ResetSpeed();
        _player.transform.position = _playerSpawnPoint.position;
    }

    public void Activate(TargetTrigger previousTrigger)
    {
        Sprite playerSprite = previousTrigger == null ? null : previousTrigger.Sprite;
        Color playerColor = previousTrigger == null ? Color.white : previousTrigger.Color;

        _player.SetSprite(playerSprite, playerColor);

        Restart(previousTrigger);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnShredderEntered()
    {
        BoardFinished?.Invoke(this, null, false);
    }

    private Color GetSameColorFromPool(List<Color> colorsPool, Color referenceColor)
    {
        foreach (Color color in colorsPool)
        {
            if (color.ToString() == referenceColor.ToString())
            {
                return color;
            }
        }
        return Color.white;
    }

    private Sprite GetSameSpriteFromPool(List<Sprite> spritesPool, Sprite referenceSprite)
    {
        foreach (Sprite sprite in spritesPool)
        {
            if (sprite.name == referenceSprite.name)
            {
                return sprite;
            }
        }
        return null;
    }
}
