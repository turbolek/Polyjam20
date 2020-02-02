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

    // Start is called before the first frame update
    void Start()
    {
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
        _mainMenuCanvasGroup.alpha = 0f;
        if (_gameplayManager != null)
        {
            _gameplayManager.StartGame();
        }
    }
}
