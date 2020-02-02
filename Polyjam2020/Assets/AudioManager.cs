using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip CorrectAnswer;
    public AudioClip WrongAnswer;
    public AudioClip Player1Positive;
    public AudioClip Player1Negative;
    public AudioClip Player2Positive;
    public AudioClip Player2Negative;
    public AudioClip StepBackward;
    public AudioClip GameWonClip;
    [Header("Audio Sources")]
    public AudioSource MusicAudioSource;
    public AudioSource SFXAudioSource;


    public void Init()
    {
        GameplayBoard.BoardFinished += OnGameplayBoardFinished;
        LegsController.LegsMoved += OnLegsMoved;
        GameplayManager.GameWon += OnGameWon;
        GameplayBoard.DialogueDisplayed += OnDialogueDisplayed;
    }

    private void OnGameplayBoardFinished(GameplayBoard board, TargetTrigger trigger, bool success)
    {
        if (success)
        {
            SFXAudioSource.PlayOneShot(CorrectAnswer);
        }
        else
        {
            SFXAudioSource.PlayOneShot(WrongAnswer);
        }
    }

    private void OnLegsMoved(LegsController legs, bool forward)
    {
        if (forward)
        {
        }
        else
        {
        }
    }

    private void OnGameWon()
    {
        SFXAudioSource.PlayOneShot(GameWonClip);
    }

    private void OnDialogueDisplayed(GameplayBoard board, bool positive)
    {
        AudioClip clip = null;
        if (board.IsPlayerOne)
        {
            clip = positive ? Player1Positive : Player1Negative;
        }
        else
        {
            clip = positive ? Player2Positive : Player2Negative;
        }

        if (clip != null)
        {
            SFXAudioSource.PlayOneShot(clip);
        }
    }

}
