using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public List<GameplaySequence> Sequences;
    public int StartingSequenceIndex;
    private GameplaySequence _currentSequence;

    public Text GameOverText;
    public Text DistanceText;

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

        _currentSequence = Sequences[StartingSequenceIndex];
        foreach (GameplaySequence sequence in Sequences)
        {
            sequence.Init(_player1Board, _player2Board);
        }

    }

    public void StartGame()
    {
        _currentSequence.Start(OnGameplaySequenceFinished);
    }

    private void OnGameplaySequenceFinished(GameplaySequence sequence)
    {
        GameplaySequence sequenceToPlay = null;
        if (sequence.Success)
        {
            sequenceToPlay = GetNextSequence();
        }
        else
        {
            sequenceToPlay = GetPreviousSequence();
        }

        if (sequenceToPlay != null)
        {
            sequenceToPlay.Start(OnGameplaySequenceFinished);
        }
        else
        {
            EndGame(sequence.Success);
        }
    }

    private GameplaySequence GetNextSequence()
    {
        return GetOffsetSequence(1);
    }

    private GameplaySequence GetPreviousSequence()
    {
        return GetOffsetSequence(-1);
    }

    private GameplaySequence GetOffsetSequence(int offset)
    {
        int currentSequenceIndex = Sequences.FindIndex(s => s == _currentSequence);
        int newIndex = currentSequenceIndex + offset;

        if (newIndex >= 0 && newIndex < Sequences.Count)
        {
            return Sequences[newIndex];
        }
        return null;
    }

    private void InitGameplayBoards()
    {
        GameObject player1BoardGO = GameObject.FindGameObjectWithTag("Player1Board");
        _player1Board = player1BoardGO.GetComponent<GameplayBoard>();
        _player1Board.Init(_targetSprites, _targetColors);

        GameObject player2BoardGO = GameObject.FindGameObjectWithTag("Player2Board");
        _player2Board = player2BoardGO.GetComponent<GameplayBoard>();
        _player2Board.Init(_targetSprites, _targetColors);
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
}
