using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Smarteye.Manager.taufiq
{
    public enum GameStage
    {
        None = 0, IVCA = 1, Profiling = 2, Rapport = 3, Probing = 4, Solution = 5, Closing = 6
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Progress")]
        [SerializeField] private GameStage m_currentGameStage = GameStage.None;
        public GameStage currentGameStage
        {
            get
            {
                return m_currentGameStage;
            }
            set
            {
                switch ((int)value)
                {
                    case 1:
                        m_currentGameStage = GameStage.IVCA;
                        break;
                    case 2:
                        m_currentGameStage = GameStage.Profiling;
                        break;
                    case 3:
                        m_currentGameStage = GameStage.Rapport;
                        break;
                    case 4:
                        m_currentGameStage = GameStage.Probing;
                        break;
                    case 5:
                        m_currentGameStage = GameStage.Solution;
                        break;
                    case 6:
                        m_currentGameStage = GameStage.Closing;
                        break;
                }
            }
        }

        [Header("Scenes")]
        [SerializeField] private SceneField[] sceneArray;
        private SceneField m_currentActiveScene = null;

        [Header("Player Data")]
        public PlayerData playerData;


        [Header("Component References")]
        [SerializeField] private LoadingScreenHandler loadingScreenHandler;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            AddScene(0);
            m_currentActiveScene = sceneArray[0];
        }

        public String GenerateGameStageName(GameStage _stage)
        {
            string result = _stage switch
            {
                GameStage.IVCA => "GENERATE IVCA",
                GameStage.Profiling => "PROSPECTING & PROFILING",
                GameStage.Rapport => "RAPPORT",
                GameStage.Probing => "PROBING",
                GameStage.Solution => "SOLUTION",
                GameStage.Closing => "OBJECTION & CLOSING",
                _ => "none"
            };

            return result;
        }

        #region Load-Scene-Function

        /// <summary>
        /// Mengganti scene berdasarkan index element pada variable "Scene Array" di component Game Manager. 
        /// untuk melihat lebih detail silahkan lihat Game Manger di scene "00-main-scene"
        /// </summary>
        /// <param name="targetSceneArrayId">index element, scene pertama dimulai dari 0</param>
        public void LoadScene(int targetSceneArrayId)
        {
            m_currentActiveScene = loadingScreenHandler.LoadSceneWithLoadingScreen(sceneArray[targetSceneArrayId], currentGameStage, m_currentActiveScene);

            // Debug.Log($"load scene {sceneArray[targetSceneArrayId].SceneName}");
        }

        /// <summary>
        /// Menambahkan scene ke dalam game, dengan memanggil scene yang ada di Game Manager. 
        /// untuk melihat lebih detail silahkan lihat Game Manger di scene "00-main-scene"
        /// </summary>
        /// <param name="targetSceneArrayId">index element, scene pertama dimulai dari 0</param>
        public void AddScene(int targetSceneArrayId)
        {
            loadingScreenHandler.AddSceneAdditive(sceneArray[targetSceneArrayId]);
        }

        #endregion
    }
}