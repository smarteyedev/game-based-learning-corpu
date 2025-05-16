using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Smarteye.RestAPI.Sample;
using Smarteye.GBL.Corpu;
using System.Runtime.InteropServices;

namespace Smarteye.Manager.taufiq
{
    public enum GameStage
    {
        None = 0, IVCA = 1, PROSPECTINGANDPROFILING = 2, RAPPORT = 3, PROBING = 4, SOLUTION = 5, OBJECTIONANDCLOSING = 6, FINISH = 7
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
                        m_currentGameStage = GameStage.PROSPECTINGANDPROFILING;
                        break;
                    case 3:
                        m_currentGameStage = GameStage.RAPPORT;
                        break;
                    case 4:
                        m_currentGameStage = GameStage.PROBING;
                        break;
                    case 5:
                        m_currentGameStage = GameStage.SOLUTION;
                        break;
                    case 6:
                        m_currentGameStage = GameStage.OBJECTIONANDCLOSING;
                        break;
                    case 7:
                        m_currentGameStage = GameStage.FINISH;
                        break;
                }
            }
        }

        [Header("Scenes")]
        [SerializeField] private SceneField[] sceneArray;
        private SceneField m_currentActiveScene = null;

        [Header("Player Data")]
        public HandlerPlayerData handlerPlayerData;
        public PlayerData playerData;
        public ScenarioLoader scenarioLoader;
        public HandlerScenarioData handlerScenarioData;
        [Space(10f)]
        [SerializeField] private string unityEditorToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6NiwiZW1haWwiOiJhZG1pbnVuaXR5QGdtYWlsLmNvbSIsInJvbGUiOiJ1c2VyIiwiaWF0IjoxNzQ3MzE2OTczLCJleHAiOjE3NDc1NzYxNzN9.iB9YP8Iv9DZxsX1QwDaiwE9lTykXZ6TCTav09dnenLE";

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

            GetPlayerToken();
        }

        private void Start()
        {
            StartGame();

            handlerPlayerData.GetPlayerData();
            handlerScenarioData.GetCompanyListFromAPI();
            //? handlerScenarioData.GetCompanySummary(1); buat ngetes
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
                GameStage.PROSPECTINGANDPROFILING => "PROSPECTING & PROFILING",
                GameStage.RAPPORT => "RAPPORT",
                GameStage.PROBING => "PROBING",
                GameStage.SOLUTION => "SOLUTION",
                GameStage.OBJECTIONANDCLOSING => "OBJECTION & CLOSING",
                _ => "none"
            };

            return result;
        }

        #region Web-Request

        public void StorePlayerDataToDatabase()
        {
            handlerPlayerData.PostPlayerData(playerData.GetPlayerData);
        }

        public void ResetPlayerData()
        {
            playerData.ResetPlayerProgressData();

            StorePlayerDataToDatabase();
        }

        public void CheckAndLoadScenario()
        {
            if (playerData.GetPlayerGameStageProgress() > GameStage.IVCA && !String.IsNullOrEmpty(playerData.GetScenarioString()))
            {
                scenarioLoader.LoadJsonString(playerData.GetScenarioString());
                // scenarioLoader.LoadJsonFileManually();
            }
        }

        public GameStage GenerateStringToGameStage(string _str)
        {
            GameStage result = _str switch
            {
                "None" => GameStage.None,
                "IVCA" => GameStage.IVCA,
                "PROSPECTINGANDPROFILING" => GameStage.PROSPECTINGANDPROFILING,
                "RAPPORT" => GameStage.RAPPORT,
                "PROBING" => GameStage.PROBING,
                "SOLUTION" => GameStage.SOLUTION,
                "OBJECTIONANDCLOSING" => GameStage.OBJECTIONANDCLOSING,
                "FINISH" => GameStage.FINISH,
                _ => GameStage.None
            };

            return result;
        }

        private void GetPlayerToken()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string currentTokenFromWeb = PlayerTokenFromLocalStorage();
            if (!string.IsNullOrEmpty(currentTokenFromWeb))
            {
                playerData.PlayerToken = currentTokenFromWeb;
            }
#endif

#if UNITY_EDITOR 
            playerData.PlayerToken = unityEditorToken;
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string PlayerTokenFromLocalStorage();
#endif

        #endregion

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