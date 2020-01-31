using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int StepsApart;
    public int LosingDistance;
    public Button Player1CorrectButton;
    public Button Player1WrongButton;
    public Button Player2CorrectButton;
    public Button Player2WrongButton;
    public Text GameOverText;
    public Text DistanceText;

    private void Start()
    {
        DistanceText.text = StepsApart.ToString();
        GameOverText.enabled = false;
        Player1CorrectButton.onClick.AddListener(DecreaseDistance);
        Player1WrongButton.onClick.AddListener(IncreaseDistance);
        Player2CorrectButton.onClick.AddListener(DecreaseDistance);
        Player2WrongButton.onClick.AddListener(IncreaseDistance);
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
        Player1CorrectButton.interactable = !Player1CorrectButton.interactable;
        Player2WrongButton.interactable = !Player2WrongButton.interactable;
        Player1WrongButton.interactable = !Player1WrongButton.interactable;
        Player2CorrectButton.interactable = !Player2CorrectButton.interactable;
        DistanceText.text = StepsApart.ToString();
        CheckEndGame();
    }
}
