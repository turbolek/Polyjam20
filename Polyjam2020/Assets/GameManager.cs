using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _mainMenuCanvasGroup;

    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _quitButton;
    private GameplayManager _gameplayManager;
    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _gameWonText;


    private ParticleSystem[] _particles;

    // Start is called before the first frame update
    void Start()
    {
        _particles = FindObjectsOfType<ParticleSystem>();
        StopParticles();

        _gameOverText.enabled = false;
        _gameWonText.enabled = false;
        _startButton.interactable = false;
        _startButton.onClick.AddListener(StartGame);
        _quitButton.onClick.AddListener(Application.Quit);
        _audioManager.Init();
        StartCoroutine(LoadScenesCoroutine());
    }

    private IEnumerator LoadScenesCoroutine()
    {
        SceneManager.LoadScene("GameplayMainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("GameplayBoardScene", LoadSceneMode.Additive);

        yield return null;

        _gameplayManager = FindObjectOfType<GameplayManager>();
        _gameplayManager.Init();
        _startButton.interactable = true;
    }

    private void StartGame()
    {
        if (_gameplayManager != null)
        {
            _gameplayManager.StartGame(OnGameLost, OnGameWon);
            HideMenu();
        }
    }

    private void OnGameLost()
    {
        _gameOverText.enabled = true;
        _gameWonText.enabled = false;
        ShowMenu();
    }

    private void OnGameWon()
    {
        StartCoroutine(OnGameWonCoroutine());
    }

    private IEnumerator OnGameWonCoroutine()
    {
        _gameOverText.enabled = false;
        StartParticles();
        yield return new WaitForSeconds(3f);
        _gameWonText.enabled = true;
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        StopParticles();
        _gameWonText.enabled = false;
        _gameplayManager.ResetLegs();
        ShowMenu();
    }

    private void HideMenu()
    {

        _mainMenuCanvasGroup.alpha = 0f;
    }

    private void ShowMenu()
    {
        _mainMenuCanvasGroup.alpha = 1f;
    }


    private void StopParticles()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Stop();
        }
    }

    private void StartParticles()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();
        }
    }
}
