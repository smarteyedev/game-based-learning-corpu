using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Smarteye.RestAPI;
using Smarteye.VisualNovel;
using Smarteye.RestAPI.Sample;
using System.Linq;
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

        [Header("Admin Token")]
        [SerializeField] private string authToken = "aW1tZXJzaXOlhzV3bdb3d4YYlFCRUNIVlBMNFhQkxEbXlXTFFoQQFrSTFFS0FtM21BRVpVTmdwcHN6";

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

        public void GetScenarioById(int _companyId, Action<JObject> _onSuccess, Action<JObject> _onFail)
        {
            restAPI.GetWithAuthorization("GetScenario", authToken, _companyId.ToString(), _onSuccess, _onFail);
        }

        public void GetCompanySummary(int _companyId, Action<JObject> _onSuccess, Action<JObject> _onFail)
        {
            restAPI.GetWithAuthorization("GetSummary", authToken, _companyId.ToString(), _onSuccess, _onFail);
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

        private void ParseCompanyList(JToken token)
        {
            companyList = JsonConvert.DeserializeObject<List<Company>>(token.ToString());

            foreach (var company in companyList)
            {
                // Debug.Log($"ID: {company.id_company} - Name: {company.company_name}");
            }
        }
        #endregion
    }
}
