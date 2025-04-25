using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Smarteye.Manager.taufiq
{
    public class LoadingScreenHandler : MonoBehaviour
    {
        [Header("Loading Config")]
        [SerializeField] private List<string> _loadingMessages;
        [Range(1f, 10f)]
        [SerializeField] private float loadingTime = 3f;


        [Header("UI Component References")]
        [SerializeField] private GameObject screenPanel;
        [SerializeField] private TextMeshProUGUI _MessageText;
        [SerializeField] private Image _loadingBar;

        private AsyncOperation _currentSceneLoadOp;

        public SceneField LoadSceneWithLoadingScreen(SceneField targetNextScene, int messageId = 0, SceneField targetUnloadScene = null)
        {
            SceneManager.UnloadSceneAsync(targetUnloadScene);

            screenPanel.SetActive(true);

            _MessageText.text = _loadingMessages[messageId];

            // Start loading scene and track the operation
            _currentSceneLoadOp = SceneManager.LoadSceneAsync(targetNextScene, LoadSceneMode.Additive);
            _currentSceneLoadOp.allowSceneActivation = false;

            StartCoroutine(HandleSceneLoading(targetNextScene));

            return targetNextScene;
        }

        private IEnumerator HandleSceneLoading(SceneField targetScene)
        {
            while (_currentSceneLoadOp.progress < 0.9f)
            {
                _loadingBar.fillAmount = _currentSceneLoadOp.progress;
                yield return null;
            }

            float timer = 0;

            while (timer < loadingTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / loadingTime);
                _loadingBar.fillAmount = t;
                yield return null;
            }

            _loadingBar.fillAmount = 1f;

            _currentSceneLoadOp.allowSceneActivation = true;

            while (!_currentSceneLoadOp.isDone)
            {
                yield return null;
            }

            Scene newScene = SceneManager.GetSceneByName(targetScene.SceneName);
            if (newScene.IsValid())
            {
                SceneManager.SetActiveScene(newScene);
            }

            screenPanel.SetActive(false);
        }
    }
}