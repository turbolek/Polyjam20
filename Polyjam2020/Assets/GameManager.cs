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
    public Text GameOverText;
    public Text DistanceText;

    private void Start()
    {
        DistanceText.text = StepsApart.ToString();
        GameOverText.enabled = false;
        Player1CorrectButton.onClick.AddListener(DecreaseDistance);
        Player1WrongButton.onClick.AddListener(IncreaseDistance);
    }

    private void DecreaseDistance()
    {
        StepsApart--;
        DistanceText.text = StepsApart.ToString();
        CheckEndGame();
    }

    private void IncreaseDistance()
    {
        StepsApart++;
        DistanceText.text = StepsApart.ToString();
        CheckEndGame();
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
}
