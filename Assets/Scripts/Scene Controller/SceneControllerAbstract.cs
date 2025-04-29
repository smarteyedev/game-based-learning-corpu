using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smarteye.Manager.taufiq;
using Smarteye.MycoonController.taufiq;

namespace Smarteye.SceneController.taufiq
{
    public abstract class SceneControllerAbstract : MonoBehaviour
    {
        [Header("Dubugging Components")]
        [SerializeField] protected bool isForDebugging = false;
        [SerializeField] protected GameObject debugObjectParent;

        [Header("Component References")]
        public MycoonHandler mycoonHandler;

        protected GameManager gameManager;

        private void Awake()
        {
            gameManager = GameManager.instance;
        }

        private void Start()
        {
            StartOnDebugging();
            Init();
        }

        protected virtual void StartOnDebugging()
        {
            if (isForDebugging && debugObjectParent)
            {
                debugObjectParent.SetActive(true);
                Debug.LogWarning($"Scene on Debugging Mode");
            }
            else
            {
                debugObjectParent.SetActive(false);
            }
        }

        protected abstract void Init();

        public void ChangeSceneTo(int sceneArrayId)
        {
            if (gameManager == null) Debug.LogWarning($"scene controller can't get game manager");

            gameManager.LoadScene(sceneArrayId - 1);
        }

        public void AddScene(int sceneArrayId)
        {
            if (gameManager == null) Debug.LogWarning($"scene controller can't get game manager");

            gameManager.AddScene(sceneArrayId);
        }
    }
}