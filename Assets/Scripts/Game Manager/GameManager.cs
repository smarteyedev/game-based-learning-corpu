using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    private SceneField _currentActiveScene = null;
    [SerializeField] private SceneField _sceneMenu;
    [SerializeField] private SceneField _sceneMaps;

    [Header("Component References")]
    public LoadingScreenHandler loadingScreenHandler;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        // SceneManager.LoadSceneAsync(_sceneMenu);

        _currentActiveScene = Instantiate(loadingScreenHandler).LoadSceneWithLoadingScreen(_sceneMenu, 0);
    }
}
