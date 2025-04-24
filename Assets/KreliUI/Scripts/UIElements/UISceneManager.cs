using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KreliStudio
{
    [ExecuteInEditMode]
    public class UISceneManager :  MonoBehaviour
    {

        [Serializable]
        public class OnSceneLoaded : UnityEvent<Scene,LoadSceneMode> { }
        [Serializable]
        public class OnSceneUnloaded : UnityEvent<Scene> { }


        #region Variables
        private static UISceneManager _instance;
        public static UISceneManager Instance { get { return _instance; } }

        public int startScene;
        public SceneProperties previousScene;
        public SceneProperties currentScene;
        public List<LoadingScreenProperties> loadingScreens = new List<LoadingScreenProperties>(); // add,remove by hand in inspector
        //public int startLoadingScreen=0;
        public List<SceneProperties> sceneProperties = new List<SceneProperties>();// add automaticly when scene was added to buildSettings
        public OnSceneLoaded onSceneLoaded = new OnSceneLoaded();
        public OnSceneUnloaded onSceneUnloaded = new OnSceneUnloaded();

        public static int numbers;
        

        [SerializeField] private int openTab;
        #endregion

        #region Main Methods
        void OnEnable()
        {
            Singleton();
        }

        void Start()
        {
            if (Application.isPlaying)
            {
                LoadScene(startScene);
                foreach (LoadingScreenProperties loadingScreens in loadingScreens)
                {
                    if (loadingScreens.canvasGroup)
                    {
                        loadingScreens.canvasGroup.gameObject.SetActive(true);
                        loadingScreens.canvasGroup.alpha = 0;
                    }
                }
            }
            
        }

        void OnDisable()
        {// delegate was added in singleton
            SceneManager.sceneLoaded -= OnSceneLoadedEvent;
            SceneManager.sceneUnloaded -= OnSceneUnloadedEvent;
            _instance = null;
        }

        public void LoadScene(int _buildIndex)
        {
            int _sceneIndex = sceneProperties.FindIndex(obj => obj.buildIndex == _buildIndex);


            //Debug.Log("_sceneIndex: " + _sceneIndex);
            if (_sceneIndex < sceneProperties.Count && _sceneIndex >= 0)
            {
                    if (sceneProperties[_sceneIndex].buildIndex <= SceneManager.sceneCountInBuildSettings && sceneProperties[_sceneIndex].buildIndex >= 0)
                    {
                    //Debug.Log("Scene to load: " + sceneProperties[_sceneIndex].scene.name);
                    //UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "LoadScene", "Start loading scene build index " + _buildIndex + ".");
                    StartCoroutine(LoadSceneAsync(sceneProperties[_sceneIndex]));
                    }
                    else
                        Debug.LogWarning("Can not load scene " + sceneProperties[_sceneIndex].scene.name + " because it is not added to Build Settings.");
            }
            else
                Debug.LogWarning("Can not load scene because Scene Index " + _sceneIndex + " do not exist in Ui Scene Manager.");
        }
        public void LoadScene(SceneProperties sceneProperties)
        {
            if (sceneProperties.buildIndex <= SceneManager.sceneCountInBuildSettings && sceneProperties.buildIndex >= 0)
            {
                //Debug.Log("Scene to load: " + sceneProperties.scene.name);
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "LoadScene", "Start loading scene build index " + sceneProperties.buildIndex +".");
                StartCoroutine(LoadSceneAsync(sceneProperties));
            }
            else
                Debug.LogWarning("Can not load scene " + sceneProperties.scene.name + " because it is not added to Build Settings.");            
        }
        public void LoadPreviousScene()
        {
            UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "LoadPreviousScene", "Start loading previous scene build index " + previousScene.buildIndex + ".");
            LoadScene(previousScene);
        }


        public void UnloadScene(int _buildIndex)
        {
            Debug.Log("UnloadScene: " + Time.time);
            if (_buildIndex < sceneProperties.Count)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "UnloadScene", "Start unloading scene build index " + _buildIndex + ".");
                StartCoroutine(UnloadSceneAsync(_buildIndex));
            }
            else
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "UnloadScene", "Can not unload scene " +_buildIndex +" because it is not added to build settings.");
        }
        public void UnloadPreviousScene()
        {
            Debug.Log("UnloadPreviousScene: " + Time.time);
            if (previousScene != null)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "UnloadPreviousScene", "Start unloading previous scene build index " + previousScene.buildIndex + ".");
                StartCoroutine(UnloadPreviousSceneAsync());
            }
            else
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "UnloadPreviousScene", "Can not unload previous scene because history is empty.");
        }
        #endregion

        #region Helper Methods
        private void FadeIn(LoadingScreenProperties _loadingScreen)
        {
            if (_loadingScreen.canvasGroup)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "FadeIn", "Fade in loading screen " + _loadingScreen.canvasGroup.name +".");
                StartCoroutine(_FadeIn(_loadingScreen));
            }
            else
                Debug.LogWarning("Cant fade in loading screen because is not selected");
        }
        private void FadeOut(LoadingScreenProperties _loadingScreen)
        {
            if (_loadingScreen.canvasGroup)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "FadeOut", "Fade out loading screen " + _loadingScreen.canvasGroup.name +".");
                StartCoroutine(_FadeOut(_loadingScreen));
            }
            else
                Debug.LogWarning("Cant fade out loading screen because is not selected");
        }

        private IEnumerator _FadeIn(LoadingScreenProperties _loadingScreen)
        {
            if (_loadingScreen.fadeInDuration == 0)
            {
                _loadingScreen.canvasGroup.alpha = 0f;
                yield return null;
            }
            else
            {
                _loadingScreen.OnLoading(1f);
                float lerpTime = 0f;
                while (lerpTime < _loadingScreen.fadeInDuration)
                {
                    _loadingScreen.canvasGroup.alpha = Mathf.Lerp(1f, 0f, (lerpTime / _loadingScreen.fadeInDuration));
                    lerpTime += Time.deltaTime;
                    yield return null;
                }
                _loadingScreen.canvasGroup.alpha = 0f;
            }
        }
        private IEnumerator _FadeOut(LoadingScreenProperties _loadingScreen)
        {
            if (_loadingScreen.fadeInDuration == 0)
            {
                _loadingScreen.canvasGroup.alpha = 1f;
                yield return null;
            }
            else
            {
                _loadingScreen.OnLoading(0f);
                float lerpTime = 0f;
                while (lerpTime < _loadingScreen.fadeInDuration)
                {
                    _loadingScreen.canvasGroup.alpha = Mathf.Lerp(0f, 1f, (lerpTime / _loadingScreen.fadeInDuration));
                    lerpTime += Time.deltaTime;
                    yield return null;
                }
                _loadingScreen.canvasGroup.alpha = 1f;
            }
        }
        private IEnumerator LoadSceneAsync(SceneProperties _scene)
        {
            previousScene = currentScene;
            currentScene = _scene;
            LoadingScreenProperties _loadingScreen=null;
            // if this scene have loading screen
            if (_scene.isLoadingScreen)
            {
                // if is anything in loading screens array
                if (loadingScreens.Count > 0)
                {
                    // if scane loading screen indeks is in this array
                    if (_scene.loadingScreenIndex < loadingScreens.Count)
                        _loadingScreen = loadingScreens[_scene.loadingScreenIndex];
                    else // if not then set first loading screen
                        _loadingScreen = loadingScreens[0];
                }
                // if this scene is first in game dont fade out from previouse
                if (previousScene.scene != null)
                {
                    // if nextscene has loading screen fadeOut from scene
                    FadeOut(_loadingScreen);
                    yield return new WaitForSeconds(_loadingScreen.fadeOutDuration);
                }
            }
            
            // load scene asynch
            AsyncOperation ao = SceneManager.LoadSceneAsync(_scene.buildIndex, _scene.loadSceneMode);

            // while loading
            while (!ao.isDone)
            {
                float progress = Mathf.Clamp01(ao.progress / 0.9f);
                //Debug.Log("Loading progress: " + (progress * 100) + "%");
                // if is loading screen then push there progress e.g for progress bars
                if (_scene.isLoadingScreen)
                    _loadingScreen.OnLoading(progress);
                yield return null;
            }
            // if scene is loaded and has loading screen then FadeIn to scene
            if (_scene.isLoadingScreen)
                FadeIn(_loadingScreen);
             // close opened ui when loaded new scene
             if (_scene.closeAllUIScreenImmediately)
                UIManager.Instance.CloseAllUIScreenImmediately();
            UIDebug.PrintDebug(UIDebug.DebugType.SCENE, transform, "UISceneManager", "LoadSceneAsync", "Scene loaded build index " + _scene.buildIndex + ".");
            // invoke event on loaded scene (open some new ui or something)
            _scene.OnSceneLoaded();

        }
        private IEnumerator UnloadSceneAsync(int _buildIndex)
        {
            Debug.Log("Start UnloadScene: " + Time.time);
            // load scene asynchs
            AsyncOperation ao = SceneManager.UnloadSceneAsync(_buildIndex);
            yield return ao;
            Debug.Log("End UnloadScene: " + Time.time);
        }
        private IEnumerator UnloadPreviousSceneAsync()
        {
            Debug.Log("Start UnloadPreviousScene: " + Time.time);
                AsyncOperation ao = SceneManager.UnloadSceneAsync(previousScene.buildIndex);
                yield return ao;
            Debug.Log("End UnloadPreviousScene: " + Time.time);
        }
        
        public void Singleton()
        {
            if (_instance == null)
            {
                _instance = this;
                SceneManager.sceneLoaded += OnSceneLoadedEvent;
                SceneManager.sceneUnloaded += OnSceneUnloadedEvent;
            }
            else if (_instance != this)
                Destroy(gameObject);
        }
        private void OnSceneLoadedEvent(Scene _scene, LoadSceneMode _mode)
        {
            if (onSceneLoaded != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UISceneManager", "On Scene Loaded", "Event invoke.");
                    onSceneLoaded.Invoke(_scene, _mode);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onSceneLoaded. Error: " + exception.Message);
                }
            }


        }
        private void OnSceneUnloadedEvent(Scene _scene)
        {
            if (onSceneUnloaded != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UISceneManager", "On Scene Unloaded", "Event invoke.");
                    onSceneUnloaded.Invoke(_scene);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onSceneUnloaded. Error: " + exception.Message);
                }
            }
        }
        #endregion


       

    }
}
