using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scenes")]
    private SceneField _currentActiveScene = null;
    [SerializeField] private SceneField[] sceneArray;

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
        SceneManager.LoadSceneAsync(sceneArray[0], LoadSceneMode.Additive);
        _currentActiveScene = sceneArray[0];
    }

    public void LoadScene(int targetId)
    {
        _currentActiveScene = Instantiate(loadingScreenHandler).LoadSceneWithLoadingScreen(sceneArray[targetId], targetId, _currentActiveScene);
    }
}
