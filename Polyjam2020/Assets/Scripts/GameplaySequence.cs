using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameplaySequence
{
    public enum StartingBoardType
    {
        Payer1 = 0,
        Player2 = 1
    }

    public StartingBoardType StartingBoard;

    private Action<GameplaySequence> _onFinish;
    public int RequiredScore;
    private int _currentScore = 0;

    private GameplayBoard _player1Board;
    private GameplayBoard _player2Board;

    [HideInInspector]
    public bool Success;

    public void Start(Action<GameplaySequence> onFinish)
    {
        //NOT A MONOBEHAVIOUR START METHOD
        _onFinish = onFinish;
        GetStartingBoard().Activate(null);
        GameplayBoard.BoardFinished += OnBoardFinished;
    }

    public void Init(GameplayBoard player1Board, GameplayBoard player2Board)
    {
        _player1Board = player1Board;
        _player2Board = player2Board;
    }

    private GameplayBoard GetStartingBoard()
    {
        if (StartingBoard == StartingBoardType.Payer1)
        {
            return _player1Board;
        }

        return _player2Board;
    }

    private void OnBoardFinished(GameplayBoard board, TargetTrigger trigger, bool success)
    {
        if (success)
        {
            _currentScore++;
            CheckWin();
        }
        else
        {
            Finish(false);
        }

        SwitchBoards(board, trigger);
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
        if (finishedBoard == _player1Board)
        {
            _player1Board.Deactivate();
            _player2Board.Activate(trigger);
        }
        else
        {
            _player2Board.Deactivate();
            _player1Board.Activate(trigger);
        }
    }

    private void Finish(bool success)
    {
        Success = success;
        GameplayBoard.BoardFinished -= OnBoardFinished;
        _onFinish.Invoke(this);
    }
}
