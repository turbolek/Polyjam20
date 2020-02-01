using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class GameplaySequence
{
    public enum BoardType
    {
        Payer1 = 0,
        Player2 = 1
    }

    public BoardType StartingBoardType;

    private Action<GameplaySequence> _onFinish;
    public int RequiredScore;
    private int _currentScore = 0;
    private Text _sequenceScoreText;

    [HideInInspector]
    public GameplayBoard Player1Board;
    [HideInInspector]
    public GameplayBoard Player2Board;
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
        SetBoards();

        Player1Board.DisplayText("");
        Player2Board.DisplayText("");

        StartingBoard.Activate(null);
        GameplayBoard.BoardFinished += OnBoardFinished;
        _sequenceScoreText.text = _currentScore.ToString();
        Debug.Log("Starting sequence " + RequiredScore.ToString());
    }

    private void SetBoards()
    {
        if (StartingBoardType == BoardType.Payer1)
        {
            StartingBoard = Player1Board;
            OtherBoard = Player2Board;
        }
        else
        {
            StartingBoard = Player2Board;
            OtherBoard = Player1Board;
        }
    }

    public void Init(GameplayBoard player1Board, GameplayBoard player2Board, Text sequenceScoreText)
    {
        Player1Board = player1Board;
        Player2Board = player2Board;
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
        if (finishedBoard == Player1Board)
        {
            Player1Board.Deactivate();
            Player2Board.Activate(trigger);
        }
        else
        {
            Player2Board.Deactivate();
            Player1Board.Activate(trigger);
        }
    }

    private void Finish(bool success)
    {
        Success = success;
        GameplayBoard.BoardFinished -= OnBoardFinished;
        Player1Board.Deactivate();
        Player2Board.Deactivate();
        _onFinish.Invoke(this);

    }
}
