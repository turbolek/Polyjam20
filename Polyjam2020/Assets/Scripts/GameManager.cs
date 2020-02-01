using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int StepsApart;
    public int LosingDistance;
    public Text GameOverText;
    public Text DistanceText;

    private GameplayBoard _player1Board;
    private GameplayBoard _player2Board;

    [SerializeField]
    private Sprite[] _targetSprites;
    [SerializeField]
    private Color[] _targetColors;

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        DistanceText.text = StepsApart.ToString();
        GameOverText.enabled = false;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("GameplayBoardScene", LoadSceneMode.Additive);

        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        InitGameplayBoards();
        GameplayBoard.BoardFinished += OnBoardFinished;

        _player1Board.Activate(null);
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

    private void DecreaseDistance()
    {
        ChangeDistance(-1);
    }

    private void IncreaseDistance()
    {
        ChangeDistance(1);
    }

    private void CheckEndGame()
    {
        if (StepsApart <= 0)
        {
            WinGame();
        }
        else if (StepsApart >= LosingDistance)
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

    private void ChangeDistance(int value)
    {
        StepsApart += value;
        DistanceText.text = StepsApart.ToString();
        CheckEndGame();
    }

    private void OnBoardFinished(GameplayBoard board, TargetTrigger trigger, bool success)
    {
        if (success)
        {
            DecreaseDistance();
        }
        else
        {
            IncreaseDistance();
        }

        SwitchBoards(board, trigger);
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
}
