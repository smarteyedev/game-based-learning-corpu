using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scenes")]
    private SceneField _currentActiveScene = null;
    [SerializeField] private SceneField _sceneMenu;
    [SerializeField] private SceneField _sceneMaps;

    [Header("Component References")]
    [SerializeField] private LoadingScreenHandler loadingScreenHandler;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync(_sceneMenu, LoadSceneMode.Additive);
    }

    public void LoadScene()
    {
        _currentActiveScene = Instantiate(loadingScreenHandler).LoadSceneWithLoadingScreen(_sceneMaps, 0, _sceneMenu);
    }
}
