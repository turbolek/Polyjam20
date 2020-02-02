using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip CorrectAnswer;
    public AudioClip WrongAnswer;
    public AudioClip StepForward;
    public AudioClip StepBackward;
    [Header("Audio Sources")]
    public AudioSource MusicAudioSource;
    public AudioSource SFXAudioSource;

    public void Init()
    {
        GameplayBoard.BoardFinished += OnGameplayBoardFinished;
        LegsController.LegsMoved += OnLegsMoved;

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
            SFXAudioSource.PlayOneShot(StepForward);
        }
        else
        {
            SFXAudioSource.PlayOneShot(StepBackward);
        }
    }
}
