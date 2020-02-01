using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public List<GameplaySequence> Sequences;
    private GameplaySequence _currentSequence;

    public Text SequenceScoreText;
    public Text GameOverText;

    [SerializeField]
    private LegsController _player1LegsController;
    [SerializeField]
    private LegsController _player2LegsController;

    private GameplayBoard _player1Board;
    private GameplayBoard _player2Board;

    [SerializeField]
    private Sprite[] _targetSprites;
    [SerializeField]
    private Color[] _targetColors;

    public void Init()
    {
        GameOverText.enabled = false;

        InitGameplayBoards();

        _currentSequence = GetNextSequence();
        foreach (GameplaySequence sequence in Sequences)
        {
            sequence.Init(_player1Board, _player2Board, SequenceScoreText);
        }

        _player1LegsController.Init();
        _player2LegsController.Init();
    }

    public void StartGame()
    {
        _currentSequence.Start(OnGameplaySequenceFinished);
    }

    private void OnGameplaySequenceFinished(GameplaySequence sequence)
    {
        StartCoroutine(SequenceEndCoroutine(sequence));

    }

    private IEnumerator SequenceEndCoroutine(GameplaySequence sequence)
    {
        GameplaySequence sequenceToPlay = null;


        if (sequence.Success)
        {
            Debug.Log("Sequence " + sequence.RequiredScore.ToString() + " won.");
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.StartingBoard, "Nice sentence"));
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.OtherBoard, "Nice answer"));
        }
        else
        {
            Debug.Log("Sequence " + sequence.RequiredScore.ToString() + " lost.");
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.StartingBoard, "Mean sentence"));
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.OtherBoard, "Mean answer"));
        }


        LegsController legsToMove = sequence.Success ? GetLowestProgressPlayer() : GetHighestProgressPlayer();

        legsToMove.StepToPosition(sequence.Success);

        while (sequence.Legs.IsMoving)
        {
            yield return null;
        }

        _currentSequence = GetNextSequence();

        if (_currentSequence != null)
        {
            _currentSequence.Start(OnGameplaySequenceFinished);
        }
        else
        {
            EndGame(sequence.Success);
        }
    }

    private IEnumerator DisplayBoardTextCoroutine(GameplayBoard board, string text)
    {
        board.DisplayText(text);
        yield return new WaitForSeconds(2f);
    }


    private GameplaySequence GetNextSequence()
    {
        LegsController lowestProgressLegsController = GetLowestProgressPlayer();
        int newIndex = lowestProgressLegsController.Progress - 1;
        if (newIndex >= 0 && newIndex < Sequences.Count)
        {
            return Sequences[lowestProgressLegsController.Progress - 1];
        }
        return null;
    }

    private void InitGameplayBoards()
    {
        GameObject player1BoardGO = GameObject.FindGameObjectWithTag("Player1Board");
        _player1Board = player1BoardGO.GetComponent<GameplayBoard>();
        _player1Board.Init(_targetSprites, _targetColors, _player1LegsController);

        GameObject player2BoardGO = GameObject.FindGameObjectWithTag("Player2Board");
        _player2Board = player2BoardGO.GetComponent<GameplayBoard>();
        _player2Board.Init(_targetSprites, _targetColors, _player2LegsController);
    }

    private void EndGame(bool success)
    {
        if (success)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        GameOverText.enabled = true;
        GameOverText.text = "Good to be back together :)";
    }

    private void LoseGame()
    {
        GameOverText.enabled = true;
        GameOverText.text = "We'll never talk to each other again :(";
    }

    private LegsController GetLowestProgressPlayer()
    {
        if (_player1LegsController.Progress <= _player2LegsController.Progress)
        {
            return _player1LegsController;
        }
        else
        {
            return _player2LegsController;
        }
    }

    private LegsController GetHighestProgressPlayer()
    {
        if (_player1LegsController.Progress >= _player2LegsController.Progress)
        {
            return _player1LegsController;
        }
        else
        {
            return _player2LegsController;
        }
    }
}
