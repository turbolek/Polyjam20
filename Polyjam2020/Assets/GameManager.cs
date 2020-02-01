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

        _player1Board.Activate();
    }

    private void InitGameplayBoards()
    {
        GameObject player1BoardGO = GameObject.FindGameObjectWithTag("Player1Board");
        _player1Board = player1BoardGO.GetComponent<GameplayBoard>();
        _player1Board.Init();

        GameObject player2BoardGO = GameObject.FindGameObjectWithTag("Player2Board");
        _player2Board = player2BoardGO.GetComponent<GameplayBoard>();
        _player2Board.Init();
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

    private void OnBoardFinished(GameplayBoard board, bool success)
    {
        if (success)
        {
            DecreaseDistance();
        }
        else
        {
            IncreaseDistance();
        }

        SwitchBoards(board);
    }

    private void SwitchBoards(GameplayBoard finishedBoard)
    {
        if (finishedBoard == _player1Board)
        {
            _player1Board.Deactivate();
            _player2Board.Activate();
        }
        else
        {
            _player2Board.Deactivate();
            _player1Board.Activate();
        }
    }
}
