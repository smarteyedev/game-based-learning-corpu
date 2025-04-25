using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smarteye.Manager.taufiq;

namespace Smarteye.SceneController.taufiq
{
    public abstract class SceneControllerAbstract : MonoBehaviour
    {
        protected GameManager gameManager;

        private void Awake()
        {
            gameManager = GameManager.instance;
        }

        private void Start()
        {
            Init();
        }

        protected abstract void Init();

        public void ChangeScene(int sceneArrayId)
        {
            gameManager.LoadScene(sceneArrayId);
        }
    }
}