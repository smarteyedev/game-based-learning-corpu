using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Smarteye.RestAPI
{
    public class RestAPI : MonoBehaviour
    {
        public TargetAPIConfig targetAPIConfig;
        private string endpointTitle = "";
        private string tempDataJson;

        #region RestAPI Services

        public void PostAction(Dictionary<string, string> _data, Action<JObject> success, Action<JObject> err, Action<JObject> dataErr, string _endpointTitle, string uniqValue = "")
        {
            endpointTitle = _endpointTitle;
            tempDataJson = JsonConvert.SerializeObject(_data);

            object[] callbacks = new object[4] { success, err, dataErr, uniqValue };
            StartCoroutine(nameof(Post), callbacks);
        }

        public void PostActionCustom(Dictionary<string, object> _data, Action<JObject> success, Action<JObject> err, Action<JObject> dataErr, string _endpointTitle, string uniqValue = "")
        {
            endpointTitle = _endpointTitle;
            tempDataJson = JsonConvert.SerializeObject(_data);

            object[] callbacks = new object[4] { success, err, dataErr, uniqValue };
            StartCoroutine(nameof(Post), callbacks);
        }

        public void GetActionByID(string _endpointTitle, string userId, Action<JObject> success, Action<JObject> err)
        {
            object[] callbacks = new object[3] { targetAPIConfig.url + targetAPIConfig.GetEndpoint(_endpointTitle) + userId, err, success };
            StartCoroutine(nameof(Get), callbacks);
        }

        public void PatchAction(string _endpointTitle, string patchData, Action<JObject> success, Action<JObject> err)
        {
            object[] callbacks = new object[3] { targetAPIConfig.url + targetAPIConfig.GetEndpoint(_endpointTitle) + patchData, success, err };
            StartCoroutine(nameof(Patch), callbacks);
        }

        public void PatchActionWithBody(Dictionary<string, string> _data, string _endpointTitle, string patchData, Action<JObject> success, Action<JObject> err)
        {
            tempDataJson = JsonConvert.SerializeObject(_data);

            object[] callbacks = new object[3] { targetAPIConfig.url + targetAPIConfig.GetEndpoint(_endpointTitle) + patchData, success, err };
            StartCoroutine(nameof(PatchWithBody), callbacks);
        }

        public void GetWithAuthorization(string _endpointTitle, string authTokenBase64, Action<JObject> success, Action<JObject> err)
        {
            object[] callbacks = new object[4] { targetAPIConfig.url + targetAPIConfig.GetEndpoint(_endpointTitle), authTokenBase64, err, success };
            StartCoroutine(nameof(GetWithAuthCoroutine), callbacks);
        }

        public void GetWithAuthorization(string _endpointTitle, string _authTokenBase64, string _id, Action<JObject> _success, Action<JObject> _err)
        {
            object[] callbacks = new object[4] { targetAPIConfig.url + targetAPIConfig.GetEndpoint(_endpointTitle) + _id, _authTokenBase64, _err, _success };
            StartCoroutine(nameof(GetWithAuthCoroutine), callbacks);
        }

        #endregion

        #region Coroutines

        private IEnumerator Get(object[] callback)
        {
            string uri = (string)callback[0];
            Action<JObject> err = (Action<JObject>)callback[1];
            Action<JObject> success = (Action<JObject>)callback[2];

            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                HandleResponse(request, success, err);
            }
        }

        private IEnumerator GetWithAuthCoroutine(object[] callback)
        {
            string uri = (string)callback[0];
            string authTokenBase64 = (string)callback[1];
            Action<JObject> err = (Action<JObject>)callback[2];
            Action<JObject> success = (Action<JObject>)callback[3];

            string username = "immersive";
            string password = "XsWwoowxbQBECHWPp4SGBLDmyWLQhCAkI1EKKE3mAEZUNgppsz";

            string combined = $"{username}:{password}";
            string encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(combined));

            // Debug.Log($"request to: {uri}");

            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                request.SetRequestHeader("Accept", "application/json");
                request.SetRequestHeader("Authorization", $"Basic {encoded}");

                yield return request.SendWebRequest();

                HandleResponse(request, success, err);
            }
        }

        private IEnumerator Post(object[] callback)
        {
            Action<JObject> successCallback = (Action<JObject>)callback[0];
            Action<JObject> errCallback = (Action<JObject>)callback[1];
            Action<JObject> dataErrCallback = (Action<JObject>)callback[2];
            string uniqVal = (string)callback[3];

            string uri = targetAPIConfig.url + targetAPIConfig.GetEndpoint(endpointTitle) + uniqVal;
            Debug.Log($"Start POST request to: {uri}");

            using UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            byte[] rowData = Encoding.UTF8.GetBytes(tempDataJson);
            webRequest.uploadHandler = new UploadHandlerRaw(rowData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                JObject dataSuccess = JObject.Parse(webRequest.downloadHandler.text);
                successCallback?.Invoke(dataSuccess);
            }
            else
            {
                HandlePostError(webRequest, errCallback, dataErrCallback);
            }
        }

        private IEnumerator Patch(object[] callback)
        {
            string url = (string)callback[0];
            Action<JObject> successCallback = (Action<JObject>)callback[1];
            Action<JObject> errCallback = (Action<JObject>)callback[2];

            using UnityWebRequest webRequest = new UnityWebRequest(url, "PATCH");
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                JObject dataSuccess = JObject.Parse("{\"Pesan\":\"success\"}");
                successCallback?.Invoke(dataSuccess);
            }
            else
            {
                JObject dataError = JObject.Parse("{\"Pesan\":\"error\"}");
                errCallback?.Invoke(dataError);
            }
        }

        private IEnumerator PatchWithBody(object[] callback)
        {
            string url = (string)callback[0];
            Action<JObject> successCallback = (Action<JObject>)callback[1];
            Action<JObject> errCallback = (Action<JObject>)callback[2];

            using UnityWebRequest webRequest = new UnityWebRequest(url, "PATCH");
            byte[] bodyRaw = new UTF8Encoding().GetBytes(tempDataJson);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                JObject dataSuccess = JObject.Parse("{\"Pesan\":\"success\"}");
                successCallback?.Invoke(dataSuccess);
            }
            else
            {
                JObject dataError = JObject.Parse("{\"Pesan\":\"error\"}");
                errCallback?.Invoke(dataError);
            }
        }

        #endregion

        #region Helper Functions

        private void HandleResponse(UnityWebRequest request, Action<JObject> success, Action<JObject> err)
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string text = request.downloadHandler.text;

                try
                {
                    var token = JToken.Parse(text);

                    if (token is JObject obj)
                    {
                        success?.Invoke(obj);
                    }
                    else
                    {
                        // Kalau result berupa array, bungkus ke dalam objek baru
                        JObject arrayWrapper = new JObject
                        {
                            ["data"] = token
                        };
                        success?.Invoke(arrayWrapper);
                    }
                }
                catch (JsonReaderException e)
                {
                    Debug.LogError("Failed to parse response: " + e.Message);
                    err?.Invoke(new JObject { ["error"] = "Invalid JSON format" });
                }
            }
            else
            {
                Debug.LogError($"HTTP Error: {request.responseCode} - {request.error}");
                try
                {
                    JObject errorObj = JObject.Parse(request.downloadHandler.text);
                    err?.Invoke(errorObj);
                }
                catch
                {
                    err?.Invoke(new JObject { ["error"] = "Unknown Error" });
                }
            }
        }

        private void HandlePostError(UnityWebRequest webRequest, Action<JObject> errCallback, Action<JObject> dataErrCallback)
        {
            int responseCode = (int)webRequest.responseCode;
            string responseError = webRequest.downloadHandler.text;

            try
            {
                JObject dataError = JObject.Parse(responseError);

                if (responseCode == 500)
                {
                    Debug.LogError("Internal Server Error (500): " + dataError);
                    errCallback?.Invoke(dataError);
                }
                else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    errCallback?.Invoke(dataError);
                }
                else if (webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    dataErrCallback?.Invoke(dataError);
                }
            }
            catch (JsonReaderException e)
            {
                Debug.LogError("Failed to parse error response: " + e.Message);
            }
        }

        #endregion
    }
}
