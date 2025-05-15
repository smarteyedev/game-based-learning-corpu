using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Smarteye.RestAPI;
using Smarteye.Manager.taufiq;

namespace Smarteye.GBL.Corpu
{
    public class HandlerPlayerData : RestAPIHandler
    {
        [Serializable]
        public class UserData
        {
            public int id;
            public string name;
            public string division;
            public string witel;
        }

        [Serializable]
        public class PlayerData
        {
            public int id;
            public int user_id;
            public string game_stage_progress;
            public string target_company_selected;
            public string ivca_company_result;
            public int prospecting_and_profiling_score;
            public int rapport_score;
            public int probing_score;
            public int solution_score;
            public int objection_and_closing_score;
            public string created_at;
            public string scenario_json;
            public List<JournalNote> journalDatas;
        }

        [Serializable]
        public class PlayerResponse
        {
            public UserData user;
            public List<PlayerData> players;
        }

        public PlayerResponse currentPlayerData;

        [Header("Component References")]
        [SerializeField] private GameManager gameManager;

        public void GetPlayerData()
        {
            restAPI.GetActionByID("GetPlayerData", gameManager.playerData.PlayerToken, OnSuccessResult, OnProtocolErr);
        }

        public void PostPlayerData(PlayerDataRoot _PlayerData)
        {
            List<Dictionary<string, object>> journalList = new List<Dictionary<string, object>>();

            // Loop semua JournalNote di _PlayerData.journalDatas
            foreach (var journal in _PlayerData.journalDatas)
            {
                journalList.Add(new Dictionary<string, object>
                {
                    { "gameStage", journal.gameStage.ToString() }, // Pastikan GameStage di-convert ke string
                    { "content", journal.content }
                });
            }

            Dictionary<string, object> bodyData = new Dictionary<string, object>
            {
                { "userId", _PlayerData.userId },
                { "gameStageProgress", _PlayerData.gameStageProgress.ToString() },
                { "targetCompanySelected", _PlayerData.targetCompany },
                { "ivcaCompanyResult", _PlayerData.IVCAResult },
                { "prospectingAndProfilingScore", _PlayerData.profilingScore },
                { "rapportScore", _PlayerData.rapportScore },
                { "probingScore", _PlayerData.probingScore },
                { "solutionScore", _PlayerData.solutionScore },
                { "objectionAndClosingScore", _PlayerData.closingScore },
                { "scenarioData", _PlayerData.scenarioJsonString },
                { "journalDatas", journalList }
            };

            RowDataObject dataObject = new RowDataObject(bodyData);

            string jsonString = JsonConvert.SerializeObject(dataObject.baseData, Formatting.Indented);
            Debug.Log($"Post player data: {jsonString}");

            restAPI.PostActionCustom(
                dataObject.baseData,
                OnPostSuccessResult,
                OnPostProtocolErr,
                PostDataProcessingErr,
                "PostPlayerData",
                $"{gameManager.playerData.PlayerToken}/players"
            );
        }

        public override void OnSuccessResult(JObject result)
        {
            currentPlayerData = JsonConvert.DeserializeObject<PlayerResponse>(result.ToString());

            PlayerData currentPlayer = currentPlayerData.players[currentPlayerData.players.Count - 1];

            gameManager.playerData.SetupPlayerData(
                _userId: currentPlayerData.user.id,
                _playerName: currentPlayerData.user.name,
                _gameStageProgress: gameManager.GenerateStringToGameStage($"{currentPlayer.game_stage_progress}"),
                _targetCompany: $"{currentPlayer.target_company_selected}",
                _IVCAResult: $"{currentPlayer.ivca_company_result}",
                _profilingScore: currentPlayer.prospecting_and_profiling_score,
                _rapportScore: currentPlayer.rapport_score,
                _probingScore: currentPlayer.probing_score,
                _solutionScore: currentPlayer.solution_score,
                _closingScore: currentPlayer.objection_and_closing_score,
                _scenarioData: currentPlayer.scenario_json,
                _journalDatas: currentPlayer.journalDatas
            );
        }

        public override void OnProtocolErr(JObject result)
        {
            Debug.LogError("Protocol Error: " + result);
        }

        public override void DataProcessingErr(JObject result)
        {
            Debug.LogError("Data Processing Error: " + result);
        }

        public void OnPostSuccessResult(JObject result)
        {
            Debug.Log("Get Player Data Success: " + result);
        }
        public void OnPostProtocolErr(JObject result)
        {
            Debug.LogError("Protocol Error: " + result);
        }
        public void PostDataProcessingErr(JObject result)
        {
            Debug.LogError("Data Processing Error: " + result);
        }
    }
}
