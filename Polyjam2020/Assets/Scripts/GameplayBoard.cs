using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayBoard : MonoBehaviour
{
    public static Action<GameplayBoard, bool> DialogueDisplayed;
    public bool IsPlayerOne;

    [SerializeField]
    private PlayerController.ControlScheme _controlScheme;

    [SerializeField]
    private Text _dialogueText;
    [SerializeField]
    private GameObject _gameplayParent;
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private Transform _playerSpawnPoint;

    private Animator _animator;

    private PlayerController _player;
    private TargetTrigger[] _triggers;
    private PlayerShredder _shredder;
    public static Action<GameplayBoard, TargetTrigger, bool> BoardFinished;

    private Sprite[] _targetSprites;
    private Color[] _targetColors;

    private TargetTrigger _correctTrigger;

    [HideInInspector]
    public LegsController Legs;

    public void Init(Sprite[] targetSprites, Color[] targetColors, LegsController legsController)
    {
        _animator = GetComponent<Animator>();
        _targetSprites = targetSprites;
        _targetColors = targetColors;
        Legs = legsController;

        GameObject playerGameObject = Instantiate(_playerPrefab, _gameplayParent.transform);
        playerGameObject.transform.position = _playerSpawnPoint.position;

        _shredder = GetComponentInChildren<PlayerShredder>();
        _shredder.ShredderEntered += OnShredderEntered;

        _player = playerGameObject.GetComponent<PlayerController>();
        _player.Init(_controlScheme);

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
            FinishBoard(trigger == _correctTrigger || _correctTrigger == null, trigger);
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

    public void Restart(TargetTrigger previousTrigger, bool instant)
    {
        StartCoroutine(RestartCoroutine(previousTrigger, instant));
    }

    private IEnumerator RestartCoroutine(TargetTrigger previousTrigger, bool instant)
    {
        List<TargetTrigger> triggersToSet = new List<TargetTrigger>(_triggers).Shuffle();
        List<Sprite> availableSprites = new List<Sprite>(_targetSprites).Shuffle();
        List<Color> availableColors = new List<Color>(_targetColors).Shuffle();


        if (previousTrigger == null)
        {
            previousTrigger = _triggers[0];
            previousTrigger.SetSprite(availableSprites[0]);
            previousTrigger.SetColor(availableColors[0]);
        }

        _player.SetSprite(previousTrigger.Sprite, previousTrigger.Color);

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


        for (int i = triggersToSet.Count - 1; i >= 0; i--)
        {
            triggersToSet[i].SetColor(availableColors[i]);
            triggersToSet[i].SetSprite(availableSprites[i]);
            triggersToSet[i].CorrectAnswerMarker.SetActive(false);
            availableColors.RemoveAt(i);
            availableSprites.RemoveAt(i);
        }

        ShowGameplayParent(true);

        if (!instant)
        {
            _animator.SetTrigger("show");
            yield return new WaitForSeconds(1f);
        }

        _player.ResetSpeed();
        _player.transform.position = _playerSpawnPoint.position;
        _player.Release();
        DisplayText("", true);
    }

    public void Activate(Sprite sprite, Color color, bool instant)
    {
        Activate(null, instant);
        _player.SetSprite(sprite, color);
    }

    public void Activate(TargetTrigger previousTrigger, bool instant)
    {
        Restart(previousTrigger, instant);
        gameObject.SetActive(true);
    }


    public void Deactivate()
    {
        ShowGameplayParent(false);
        _player.transform.position = _playerSpawnPoint.position;
        _player.Halt();
    }

    private void OnShredderEntered()
    {
        FinishBoard(false, null);
    }

    private void FinishBoard(bool success, TargetTrigger trigger)
    {
        BoardFinished?.Invoke(this, trigger, success);
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

    public void DisplayText(string text, bool positive)
    {
        _dialogueText.text = text;

        if (text != "")
        {
            DialogueDisplayed?.Invoke(this, positive);
        }
    }

    private void ShowGameplayParent(bool show)
    {
        _gameplayParent.SetActive(show);
    }
}
