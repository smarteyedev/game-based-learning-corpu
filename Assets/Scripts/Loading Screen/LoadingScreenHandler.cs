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

    private AsyncOperation _currentSceneLoadOp;

    public SceneField LoadSceneWithLoadingScreen(SceneField targetScene, int messageId = 0, SceneField targetUnloadScene = null)
    {
        screenPanel.SetActive(true);

        _MessageText.text = _loadingMessages[messageId];

        // Start loading scene and track the operation
        _currentSceneLoadOp = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        _currentSceneLoadOp.allowSceneActivation = false;

        StartCoroutine(HandleSceneLoading(targetScene, targetUnloadScene));

        return targetScene;
    }

    private IEnumerator HandleSceneLoading(SceneField targetScene, SceneField sceneWillDisappear)
    {
        while (_currentSceneLoadOp.progress < 0.9f)
        {
            _loadingBar.fillAmount = _currentSceneLoadOp.progress;
            yield return null;
        }

        float timer = 0f;

        while (timer < 3)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / 3);
            _loadingBar.fillAmount = t;
            yield return null;
        }

        _loadingBar.fillAmount = 1f;

        _currentSceneLoadOp.allowSceneActivation = true;

        while (!_currentSceneLoadOp.isDone)
        {
            yield return null;
        }

        if (sceneWillDisappear != null)
        {
            SceneManager.UnloadSceneAsync(sceneWillDisappear);
        }

        Scene newScene = SceneManager.GetSceneByName(targetScene.SceneName);
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }

        Destroy(this.gameObject);
    }
}
