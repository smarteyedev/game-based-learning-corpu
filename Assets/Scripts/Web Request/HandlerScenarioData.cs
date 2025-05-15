using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Smarteye.RestAPI;

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

        public List<Company> companyList = new List<Company>();

        [SerializeField] private string authToken = "aW1tZXJzaXOlhzV3bdb3d4YYlFCRUNIVlBMNFhQkxEbXlXTFFoQQFrSTFFS0FtM21BRVpVTmdwcHN6";

        public void GetCompanyList()
        {
            restAPI.GetWithAuthorization("GetCompanies", authToken, OnSuccessResult, OnProtocolErr);
        }

        public override void OnSuccessResult(JObject result)
        {
            if (result != null && result.Type == JTokenType.Array)
            {
                ParseCompanyList(result);
            }
            else if (result != null && result["data"] != null && result["data"].Type == JTokenType.Array)
            {
                ParseCompanyList(result["data"]);
            }
            else
            {
                Debug.LogWarning("Unexpected response format: " + result.ToString());
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
    }
}
