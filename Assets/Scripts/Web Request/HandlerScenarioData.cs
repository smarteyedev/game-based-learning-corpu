using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Smarteye.RestAPI;
using Smarteye.VisualNovel;
using Smarteye.RestAPI.Sample;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Smarteye.Manager.taufiq;

namespace Smarteye.GBL.Corpu
{
    public class HandlerScenarioData : RestAPIHandler
    {
        [Serializable]
        public class Company
        {
            public int id_company;
            public string company_name;
        }

        [Header("Datas")]
        [SerializeField] private List<Company> companyList = new List<Company>();
        [SerializeField] private List<SceneScenarioDataRoot> selectedScenarioList;

        [HideInInspector]
        public string scenarioJson = "";

        [Header("Admin Token")]
        [SerializeField] private string authToken = "aW1tZXJzaXOlhzV3bdb3d4YYlFCRUNIVlBMNFhQkxEbXlXTFFoQQFrSTFFS0FtM21BRVpVTmdwcHN6";

        [Header("Component References")]
        [SerializeField] private ScenarioLoader scenarioLoader;
        [SerializeField] private GameManager gameManager;

        #region Local-Query

        public List<Company> GetCompanyList()
        {
            return companyList.OrderBy(company => company.id_company).ToList();
        }

        #endregion

        #region Web-Request
        public void GetCompanyListFromAPI()
        {
            restAPI.GetWithAuthorization("GetCompanies", authToken, OnSuccessResult, OnProtocolErr);
        }

        public void GetScenarioById(int _companyId)
        {
            restAPI.GetWithAuthorization("GetScenario", authToken, _companyId.ToString(), OnSuccessGetScenario, OnProtocolErrGetScenario);
        }

        public void GetCompanySummary(int _companyId, Action<string, string, string> onGotScenario)
        {
            Debug.LogWarning($"fungsi company IVCA resume masih belum bisa dihit");

            var tCompany = companyList.First((x) => x.id_company == _companyId);
            onGotScenario?.Invoke($"{tCompany.company_name}", $"ini summary sementara untuk company {tCompany.company_name}", $"ini penjelasanan panjangnyaa {tCompany.company_name} ....");
        }
        #endregion

        #region CallBack-GetCompany
        public override void OnSuccessResult(JObject result)
        {
            try
            {
                if (result != null)
                {
                    if (result.Type == JTokenType.Array)
                    {
                        ParseCompanyList(result);
                    }
                    else if (result["data"] != null && result["data"].Type == JTokenType.Array)
                    {
                        ParseCompanyList(result["data"]);
                    }
                    else
                    {
                        Debug.LogWarning("Unexpected response format: " + result.ToString());
                    }
                }
            }
            catch (JsonSerializationException jsonEx)
            {
                Debug.LogError("JSON Serialization Error: " + jsonEx.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("General Error parsing company list: " + ex.Message);
            }
        }

        public override void OnProtocolErr(JObject result)
        {
            Debug.LogError($"Protocol Error: {GetFormattedError(result)}");
        }

        public override void DataProcessingErr(JObject result)
        {
            Debug.LogError($"Data Processing Error: {GetFormattedError(result)}");
        }

        // Utility untuk format error agar lebih rapi
        private string GetFormattedError(JObject errorObj)
        {
            try
            {
                return JsonConvert.SerializeObject(errorObj, Formatting.Indented);
            }
            catch
            {
                return errorObj.ToString();
            }
        }

        private void ParseCompanyList(JToken token)
        {
            companyList = JsonConvert.DeserializeObject<List<Company>>(token.ToString());

            foreach (var company in companyList)
            {
                // Debug.Log($"ID: {company.id_company} - Name: {company.company_name}");
            }
        }
        #endregion

        #region CallBack-GetScenarioById

        public void OnSuccessGetScenario(JObject result)
        {
            try
            {
                if (result != null)
                {
                    // Sebelum parsing, fix JSON dulu
                    string fixedJson = FixJsonForParsing(result.ToString());

                    MasterData currentScenario = JsonConvert.DeserializeObject<MasterData>(fixedJson);

                    if (currentScenario != null && currentScenario.sceneScenarioDataRoots != null)
                    {
                        // selectedScenarioList = currentScenario.sceneScenarioDataRoots;

                        scenarioJson = fixedJson;
                        gameManager.playerData.SetPlayerScenario(scenarioJson);
                        // scenarioLoader.sampleScenarios = currentScenario.sceneScenarioDataRoots;
                        Debug.Log($"Berhasil parsing : {scenarioJson}");
                    }
                    else
                    {
                        Debug.LogWarning("Scenario data kosong atau tidak terformat dengan benar.");
                    }
                }
            }
            catch (JsonSerializationException jsonEx)
            {
                Debug.LogError("JSON Serialization Error: " + jsonEx.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("General Error parsing scenario list: " + ex.Message);
            }
        }

        public void OnProtocolErrGetScenario(JObject result)
        {
            Debug.LogError($"Protocol Error: {GetFormattedError(result)}");
        }

        private static readonly Dictionary<string, string> enumCorrections = new Dictionary<string, string>
        {
            // Mapping SpeakerRoot
            { "\"REKAN\"", "\"ASISTEN\"" },
            { "\"CLIENT_ASSISTANT\"", "\"CLIENT\"" },
            { "\"CUSTOMER SERVICE\"", "\"ASISTEN\"" },
            { "\"PAK RUDI\"", "\"BOS\"" },
            { "\"ASSISTANT\"", "\"ASISTEN\"" },
            { "\"KLIEN\"", "\"CLIENT\"" },
            { "\"TIM TEKNIS\"", "\"ASISTEN\"" },
            { "\"TEMAN\"", "\"BOS\"" },
            { "\"IT STAFF\"", "\"BOS\"" }, 

            // Mapping SceneProgress
            { "\"FAILEDRESULT\"", "\"FAILRESULT\"" },
            { "\"UNEXPECTEDRESULT\"", "\"FAILRESULT\"" },

            // Mapping stage
            { "\"INITIAL_MEETING\"", "\"RAPPORT\"" },
            { "\"FOLLOW_UP_MEETING_PREPARATION\"", "\"PROBING\"" },
            { "\"FOLLOW-UP\"", "\"PROBING\"" },
            { "\"WAITING\"", "\"PROBING\"" }
        };

        private string FixJsonForParsing(string rawJson)
        {
            if (string.IsNullOrEmpty(rawJson))
                return rawJson;

            foreach (var correction in enumCorrections)
            {
                rawJson = rawJson.Replace(correction.Key, correction.Value);
            }

            return rawJson;
        }

        #endregion
    }
}
