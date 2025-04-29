using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Smarteye.MycoonController.taufiq
{
    public class MycoonHandler : MonoBehaviour
    {
        [Header("Config")]
        public List<MycoonController.ContentData> contentItem;

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

        public void ShowMycoonInfo(Action _onCompleted = null)
        {
            // Menginstansiasi objek dan membuatnya menjadi anak dari canvas
            MycoonController panelMycoon = Instantiate(mycoonPrefab, Vector3.zero, Quaternion.identity, targetCanvas.transform);

            RectTransform rectTransform = panelMycoon.GetComponent<RectTransform>();
            SetPanelReactTransformDefault(rectTransform);

            OnInfoOpened?.Invoke();

            panelMycoon.SetupPanelInfo(contentItem, () =>
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