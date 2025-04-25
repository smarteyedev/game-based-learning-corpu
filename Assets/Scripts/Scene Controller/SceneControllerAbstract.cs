using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smarteye.Manager.taufiq;

namespace Smarteye.SceneController.taufiq
{
    public abstract class SceneControllerAbstract : MonoBehaviour
    {
        [Header("Dubugging Components")]
        [SerializeField] protected bool isForDebugging = false;
        [SerializeField] protected GameObject debugObjectParent;

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
        }

        protected abstract void Init();

        public void ChangeScene(int sceneArrayId)
        {
            if (gameManager == null) Debug.LogWarning($"scene controller can't get game manager");

            gameManager.LoadScene(sceneArrayId);
        }
    }
}