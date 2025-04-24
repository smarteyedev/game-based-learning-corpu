using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenHandler : MonoBehaviour
{
    [SerializeField] private List<string> _loadingMessages;

    [Header("UI Component References")]
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private TextMeshProUGUI _MessageText;
    [SerializeField] private Image _loadingBar;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    public SceneField LoadSceneWithLoadingScreen(SceneField targetScene, int messageId = 0, SceneField targetUnloadScene = null)
    {
        screenPanel.SetActive(true);

        if (targetUnloadScene != null)
        {
            UnloadScene(targetUnloadScene);
        }

        SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        StartCoroutine(ProgressLoadingBar());

        _MessageText.text = _loadingMessages[messageId];

        return targetScene;
    }

    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                loadProgress += _scenesToLoad[i].progress;
                _loadingBar.fillAmount = loadProgress / _scenesToLoad.Count;
                yield return null;
            }
        }

        Invoke(nameof(DestroyGameObject), 1f);
    }

    private void UnloadScene(SceneField targetUnloadScene)
    {
        SceneManager.UnloadSceneAsync(targetUnloadScene);
    }

    private void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
