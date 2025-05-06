using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Smarteye.Manager.taufiq;
using System.Linq;
using Smarteye.VisualNovel.Character;

namespace Smarteye.MycoonController.taufiq
{
    public class MycoonHandler : MonoBehaviour
    {
        [Header("Config")]
        public List<HandlerContentDatas> handlerContentDatas;

        [Serializable]
        public class HandlerContentDatas
        {
            public Stage contentStage;
            public List<MycoonController.ContentData> contentItem;
        }

        [Header("Unity Event")]
        public UnityEvent OnInfoOpened;
        public UnityEvent OnInfoClosed;

        [Header("Component References")]
        public MycoonController mycoonPrefab;
        public Canvas targetCanvas;

        private void Update()
        {
            /* if (Input.GetKeyDown(KeyCode.Space)) ShowMycoonInfo(() =>
            {
                OnInfoClosed?.Invoke();
            }); */
        }

        public void ShowMycoonInfo(Stage targetStage, Action _onCompleted = null)
        {
            List<MycoonController.ContentData> datas = handlerContentDatas
                .Where((x) => x.contentStage == targetStage)
                .SelectMany((y) => y.contentItem)
                .ToList();

            if (datas.Count == 0)
            {
                Debug.LogWarning($"mycoon handler data is for stage {targetStage} has not been assigned");
                return;
            }

            // Menginstansiasi objek dan membuatnya menjadi anak dari canvas
            MycoonController panelMycoon = Instantiate(mycoonPrefab, Vector3.zero, Quaternion.identity, targetCanvas.transform);

            RectTransform rectTransform = panelMycoon.GetComponent<RectTransform>();
            SetPanelReactTransformDefault(rectTransform);

            OnInfoOpened?.Invoke();

            panelMycoon.SetupPanelInfo(datas, () =>
            {
                _onCompleted?.Invoke();
                OnInfoClosed?.Invoke();
            });
        }

        public void ShowMycoonInfo(string _title, string _message, CharacterIdentity.Action.ActionType _actionType, Action _onCompleted = null)
        {
            // Menginstansiasi objek dan membuatnya menjadi anak dari canvas
            MycoonController panelMycoon = Instantiate(mycoonPrefab, Vector3.zero, Quaternion.identity, targetCanvas.transform);

            RectTransform rectTransform = panelMycoon.GetComponent<RectTransform>();
            SetPanelReactTransformDefault(rectTransform);

            OnInfoOpened?.Invoke();

            List<MycoonController.ContentData> datas = new List<MycoonController.ContentData>();
            MycoonController.ContentData data1 = new MycoonController.ContentData();
            data1.title = _title;
            data1.explanation = _message;
            data1.role = CharacterIdentity.CharacterRole.NARATOR;
            data1.actionType = _actionType;

            datas.Add(data1);

            panelMycoon.SetupPanelInfo(datas, () =>
            {
                _onCompleted?.Invoke();
                OnInfoClosed?.Invoke();
            });
        }

        public void SetPanelReactTransformDefault(RectTransform rectTransform)
        {
            // Menyeting anchor Min dan Max ke posisi default (0, 0) dan (1, 1)
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);

            // Menyeting pivot ke default (0.5, 0.5)
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Set posisi lokalnya (seperti di gambar, pada posisi default 0, 0)
            rectTransform.localPosition = Vector3.zero;

            // Pastikan skala tetap di (1, 1, 1) untuk mempertahankan ukuran asli
            rectTransform.localScale = Vector3.one;
        }
    }
}