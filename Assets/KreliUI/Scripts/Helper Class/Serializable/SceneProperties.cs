using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace KreliStudio
{
    [Serializable]
    public class SceneProperties
    {

        public UnityEngine.Object scene;
        public int buildIndex;
        public bool isLoadingScreen;
        public int loadingScreenIndex;
        public LoadSceneMode loadSceneMode;
        public UnityEvent onSceneLoaded;
        public bool closeAllUIScreenImmediately = true;

        public SceneProperties(int buildIndex)
        {
            this.buildIndex = buildIndex;
        }
        public void OnSceneLoaded()
        {
            if (onSceneLoaded != null)
            {
                try
                {
                    onSceneLoaded.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onSceneLoaded. Error: " + exception.Message);
                }
            }


        }
    }
}
