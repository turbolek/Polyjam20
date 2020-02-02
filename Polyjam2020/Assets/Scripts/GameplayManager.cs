using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameplayManager : MonoBehaviour
{
    public List<Dialogue> PositiveDialogues;
    private List<Dialogue> PositiveDialoguesCopy = new List<Dialogue>();
    public List<Dialogue> NegativeDialogues;
    private List<Dialogue> NegativeDialoguesCopy = new List<Dialogue>();

    public List<GameplaySequence> Sequences;
    private GameplaySequence _currentSequence;

    public ScoreDisplayer ScoreDisplayer;
    public Text GameOverText;

    [SerializeField]
    private Bubble _player1Bubble;
    [SerializeField]
    private Bubble _player2Bubble;

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

    private Action _onGameLost;
    private Action _onGameWon;

    public void Init()
    {
        GameOverText.enabled = false;

        InitGameplayBoards();

        ScoreDisplayer.Init();

        foreach (GameplaySequence sequence in Sequences)
        {
            sequence.Init(_player1Board, _player2Board, ScoreDisplayer);
        }

        _player1Bubble.Init();
        _player2Bubble.Init();

        _player1LegsController.Init();
        _player2LegsController.Init();
    }

    public void StartGame(Action onGameLost, Action onGameWon)
    {
        _player1LegsController.ResetProgress();
        _player2LegsController.ResetProgress();

        _player1Board.DisplayText("");
        _player2Board.DisplayText("");

        _currentSequence = GetNextSequence();
        _onGameLost = onGameLost;
        _onGameWon = onGameWon;
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        _player1LegsController.StepToPosition(true, true);
        _player2LegsController.StepToPosition(true, true);
        yield return new WaitForSeconds(2f);

        _player1Bubble.Show();
        yield return new WaitForSeconds(1f);
        _player2Bubble.Show();
        yield return new WaitForSeconds(1f);
        _currentSequence.Start(OnGameplaySequenceFinished);
    }

    private void OnGameplaySequenceFinished(GameplaySequence sequence)
    {
        StartCoroutine(SequenceEndCoroutine(sequence));
    }

    private List<Dialogue> GetRandomizedCopy(List<Dialogue> originalList)
    {
        List<Dialogue> randomizedList = new List<Dialogue>(originalList);
        randomizedList = randomizedList.Shuffle();
        return randomizedList;
    }

    private Dialogue GetRandomPositiveDialogue()
    {
        if (PositiveDialoguesCopy == null || PositiveDialoguesCopy.Count < 1)
        {
            PositiveDialoguesCopy = GetRandomizedCopy(PositiveDialogues);
        }
        Dialogue dialogue = PositiveDialoguesCopy[0];
        PositiveDialoguesCopy.Remove(dialogue);
        return dialogue;
    }

    private Dialogue GetRandomNegativeDialogue()
    {
        if (NegativeDialoguesCopy == null || NegativeDialoguesCopy.Count < 1)
        {
            NegativeDialoguesCopy = GetRandomizedCopy(NegativeDialogues);
        }
        Dialogue dialogue = NegativeDialoguesCopy[0];
        NegativeDialoguesCopy.Remove(dialogue);
        return dialogue;
    }

    private IEnumerator SequenceEndCoroutine(GameplaySequence sequence)
    {
        GameplaySequence sequenceToPlay = null;


        if (sequence.Success)
        {
            Debug.Log("Sequence " + sequence.RequiredScore.ToString() + " won.");
            Dialogue dialogue = GetRandomPositiveDialogue();
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.StartingBoard, dialogue.Sentence));
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.OtherBoard, dialogue.Answer));
        }
        else
        {
            Debug.Log("Sequence " + sequence.RequiredScore.ToString() + " lost.");
            Dialogue dialogue = GetRandomNegativeDialogue();
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.StartingBoard, dialogue.Sentence));
            yield return StartCoroutine(DisplayBoardTextCoroutine(sequence.OtherBoard, dialogue.Answer));
        }


        LegsController legsToMove = sequence.Success ? GetLowestProgressPlayer() : GetHighestProgressPlayer();

        legsToMove.StepToPosition(sequence.Success, false);

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
        int newIndex = lowestProgressLegsController.Progress;
        if (newIndex > 0 && newIndex < Sequences.Count)
        {
            return Sequences[newIndex];
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
        StartCoroutine(WinGameCoroutine());
    }

    private IEnumerator WinGameCoroutine()
    {
        _player1Bubble.Hide();
        _player2Bubble.Hide();

        yield return new WaitForSeconds(2f);

        _onGameWon.Invoke();
    }

    private void LoseGame()
    {
        StartCoroutine(LoseGameCoroutine());
    }

    private IEnumerator LoseGameCoroutine()
    {
        _player2LegsController.Progress = 0;
        _player2LegsController.StepToPosition(false, true);

        _player1Bubble.Hide();
        _player2Bubble.Hide();

        yield return new WaitForSeconds(2f);

        _onGameLost.Invoke();
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

    public void ResetLegs()
    {
        _player1LegsController.ResetPosition();
        _player2LegsController.ResetPosition();
    }
}
