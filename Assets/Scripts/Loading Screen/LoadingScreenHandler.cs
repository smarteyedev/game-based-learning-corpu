using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

namespace Smarteye.Manager.taufiq
{
    public class LoadingScreenHandler : MonoBehaviour
    {
        [Header("Loading Config")]
        [SerializeField] private List<LoadingHint> _loadingMessages;

        [Serializable]
        public class LoadingHint
        {
            public Stage currentStage;
            public string hintMessage;
        }

        [Range(1f, 10f)]
        [SerializeField] private float loadingTime = 3f;


        [Header("UI Component References")]
        [SerializeField] private GameObject screenPanel;
        [SerializeField] private TextMeshProUGUI _MessageText;
        [SerializeField] private Image _loadingBar;

        private AsyncOperation _currentSceneLoadOp;

        public void AddSceneAdditive(SceneField _targetNextScene)
        {
            _currentSceneLoadOp = SceneManager.LoadSceneAsync(_targetNextScene, LoadSceneMode.Additive);
        }

        public SceneField LoadSceneWithLoadingScreen(SceneField _targetNextScene, Stage _currentStage, SceneField _targetUnloadScene = null)
        {
            SceneManager.UnloadSceneAsync(_targetUnloadScene);

            screenPanel.SetActive(true);

            LoadingHint msg = _loadingMessages.First((x) => x.currentStage == _currentStage);
            if (!String.IsNullOrEmpty(msg.hintMessage))
                _MessageText.text = msg.hintMessage;
            else
            {
                _MessageText.text = $"nunggu yaa????";
                Debug.LogWarning($"loading message is null");
            }

            // Start loading scene and track the operation
            _currentSceneLoadOp = SceneManager.LoadSceneAsync(_targetNextScene, LoadSceneMode.Additive);
            _currentSceneLoadOp.allowSceneActivation = false;

            StartCoroutine(HandleSceneLoading(_targetNextScene));

            return _targetNextScene;
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