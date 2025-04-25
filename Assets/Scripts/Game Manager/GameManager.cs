using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Smarteye.Manager.taufiq
{
    public enum Stage
    {
        None = 0, Profiling = 1, Rapport = 2, Probing = 3, Solution = 4, Closing = 5
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Progress")]
        private Stage m_currentStage = Stage.None;
        public Stage currentStage
        {
            get
            {
                return m_currentStage;
            }
            set
            {
                switch ((int)value)
                {
                    case 1:
                        m_currentStage = Stage.Profiling;
                        break;
                    case 2:
                        m_currentStage = Stage.Rapport;
                        break;
                    case 3:
                        m_currentStage = Stage.Probing;
                        break;
                    case 4:
                        m_currentStage = Stage.Solution;
                        break;
                    case 5:
                        m_currentStage = Stage.Closing;
                        break;
                }
            }
        }

        [Header("Scenes")]
        private SceneField m_currentActiveScene = null;
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
            m_currentActiveScene = sceneArray[0];
        }

        /// <summary>
        /// Mengganti scene berdasarkan index element pada variable "Scene Array" di component Game Manager. 
        /// untuk melihat lebih detail silahkan lihat Game Manger di scene "00-main-scene"
        /// </summary>
        /// <param name="targetSceneArrayId">index element, scene pertama dimulai dari 0</param>
        public void LoadScene(int targetSceneArrayId)
        {
            m_currentActiveScene = loadingScreenHandler.LoadSceneWithLoadingScreen(sceneArray[targetSceneArrayId], targetSceneArrayId, m_currentActiveScene);
        }
    }
}