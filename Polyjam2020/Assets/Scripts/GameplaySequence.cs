﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class GameplaySequence
{
    private Action<GameplaySequence> _onFinish;
    public int RequiredScore;
    private int _currentScore = 0;
    private Text _sequenceScoreText;

    [HideInInspector]
    public LegsController Player1Legs;
    [HideInInspector]
    public LegsController Player2Legs;
    [HideInInspector]
    public LegsController Legs
    {
        get
        {
            return StartingBoard.Legs;
        }
    }

    [HideInInspector]
    public GameplayBoard StartingBoard;
    [HideInInspector]
    public GameplayBoard OtherBoard;

    [HideInInspector]
    public bool Success;

    public void Start(Action<GameplaySequence> onFinish)
    {
        //NOT A MONOBEHAVIOUR START METHOD
        _currentScore = 0;
        _onFinish = onFinish;

        StartingBoard.DisplayText("");
        OtherBoard.DisplayText("");

        StartingBoard.Activate(null);
        GameplayBoard.BoardFinished += OnBoardFinished;
        _sequenceScoreText.text = _currentScore.ToString();
        Debug.Log("Starting sequence " + RequiredScore.ToString());
    }

    public void Init(GameplayBoard startingBoard, GameplayBoard otherBoard, Text sequenceScoreText)
    {
        StartingBoard = startingBoard;
        OtherBoard = otherBoard;
        _sequenceScoreText = sequenceScoreText;
    }

    private void OnBoardFinished(GameplayBoard board, TargetTrigger trigger, bool success)
    {
        SwitchBoards(board, trigger);

        if (success)
        {
            _currentScore++;
            _sequenceScoreText.text = _currentScore.ToString();
            CheckWin();
        }
        else
        {
            Finish(false);
        }

    }

    private void CheckWin()
    {
        if (_currentScore >= RequiredScore)
        {
            Finish(true);
        }
    }

    private void SwitchBoards(GameplayBoard finishedBoard, TargetTrigger trigger)
    {
        if (finishedBoard == StartingBoard)
        {
            StartingBoard.Deactivate();
            OtherBoard.Activate(trigger);
        }
        else
        {
            OtherBoard.Deactivate();
            StartingBoard.Activate(trigger);
        }
    }

    private void Finish(bool success)
    {
        Success = success;
        GameplayBoard.BoardFinished -= OnBoardFinished;
        StartingBoard.Deactivate();
        OtherBoard.Deactivate();
        _onFinish.Invoke(this);

    }
}
