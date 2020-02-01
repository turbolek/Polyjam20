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
    private GameplayManager _gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        _startButton.interactable = false;
        _startButton.onClick.AddListener(StartGame);
        StartCoroutine(LoadScenesCoroutine());
    }

    private IEnumerator LoadScenesCoroutine()
    {
        yield return StartCoroutine(LoadSceneCoroutine("GameplayMainScene"));
        yield return StartCoroutine(LoadSceneCoroutine("GameplayBoardScene"));

        _gameplayManager = FindObjectOfType<GameplayManager>();
        _gameplayManager.Init();
        _startButton.interactable = true;
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadingSceneOperation.isDone)
        {
            yield return null;
        }
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
