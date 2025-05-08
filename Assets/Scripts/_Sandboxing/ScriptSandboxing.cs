using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;

namespace Sandboxing
{
    public class ScriptSandboxing : MonoBehaviour
    {
        public static ScriptSandboxing Instance;
        public string playerTokenURL;
        public string playerTokenLocalStorage;
        public TextMeshProUGUI textTokenURL;
        public TextMeshProUGUI textTokenLocal;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string currentToken1 =  PlayerTokenFromUrl();
            if (!string.IsNullOrEmpty(currentToken1))
            {
                playerTokenURL = currentToken1;
                textTokenURL.text = playerTokenURL;
            }

            string currentToken2 =  PlayerTokenFromLocalStorage();
            if (!string.IsNullOrEmpty(currentToken2))
            {
                playerTokenLocalStorage = currentToken2;
                textTokenLocal.text = playerTokenLocalStorage;
            }
#endif
        }

        #region browserInteraction

#if UNITY_WEBGL && !UNITY_EDITOR

        [DllImport("__Internal")]
        private static extern string PlayerTokenFromUrl();

        [DllImport("__Internal")]
        private static extern string PlayerTokenFromLocalStorage();

#endif

        #endregion
    }
}