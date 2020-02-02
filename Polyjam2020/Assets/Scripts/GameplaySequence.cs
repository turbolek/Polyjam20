using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class GameplaySequence
{
    private Action<GameplaySequence> _onFinish;
    public int RequiredScore;
    private int _currentScore = 0;
    private ScoreDisplayer _scoreDisplayer;

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
        StartingBoard.Activate(null, false);
        GameplayBoard.BoardFinished += OnBoardFinished;
        _scoreDisplayer.ResetDisplayer();
        Debug.Log("Starting sequence " + RequiredScore.ToString());
    }

    public void Init(GameplayBoard startingBoard, GameplayBoard otherBoard, ScoreDisplayer scoreDisplayer)
    {
        StartingBoard = startingBoard;
        OtherBoard = otherBoard;
        _scoreDisplayer = scoreDisplayer;
    }

    private void OnBoardFinished(GameplayBoard board, TargetTrigger trigger, bool success)
    {
        SwitchBoards(board, trigger);

        if (success)
        {
            _currentScore++;
            _scoreDisplayer.DisplayScore(_currentScore);
            
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
            OtherBoard.Activate(trigger, true);
        }
        else
        {
            OtherBoard.Deactivate();
            StartingBoard.Activate(trigger, true);
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
