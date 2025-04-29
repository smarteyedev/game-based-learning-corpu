using System;
using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Smarteye.SceneController.taufiq
{
    public class ComputerController : SceneControllerAbstract
    {
        [Header("Generate IVCA Panel")]
        public GameObject generateIVCAPanel;
        public GameObject rowInputIVCA;
        public GameObject rowOutputIVCA;
        public InputField companyInput;
        public Button generateBtn;

        [Header("Result IVCA Panel")]
        public GameObject resultIVCAPanel;
        public List<CarouselData> carouselDatas;

        [Serializable]
        public class CarouselData
        {
            public string title;
            [TextArea(8, 8)]
            public string description;
        }
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public Button nextButton;
        public Button previousButton;

        private int m_currentIndex = 0;

        protected override void Init()
        {
            if (gameManager.currentStage == Stage.Profiling)
            {
                mycoonHandler.ShowMycoonInfo(Stage.Profiling);
            }

            generateBtn.onClick.AddListener(OnClickSubmit);
            previousButton.onClick.AddListener(PreviousItem);
            nextButton.onClick.AddListener(NextItem);
        }

        public void OnClickSubmit()
        {
            rowInputIVCA.SetActive(false);
            rowOutputIVCA.SetActive(true);
        }

        public void OnClickOpenIVCA(bool _isActive)
        {
            if (_isActive)
            {
                generateIVCAPanel.SetActive(false);
                resultIVCAPanel.SetActive(true);
                m_currentIndex = 0;

                UpdateCarousel();
            }
            else
            {
                resultIVCAPanel.SetActive(false);
                generateIVCAPanel.SetActive(true);
            }
        }

        public void OnClickCloseComputer()
        {
            ChangeSceneTo(2);
            gameManager.currentStage = (Stage)2;
        }

        public void UpdateCarousel()
        {
            if (carouselDatas.Count == 0) return;

            titleText.text = carouselDatas[m_currentIndex].title;
            descriptionText.text = carouselDatas[m_currentIndex].description;

            previousButton.interactable = m_currentIndex > 0;
            nextButton.interactable = m_currentIndex < carouselDatas.Count - 1;
        }

        void NextItem()
        {
            if (m_currentIndex < carouselDatas.Count - 1)
            {
                m_currentIndex++;
                UpdateCarousel();
            }
        }

        void PreviousItem()
        {
            if (m_currentIndex > 0)
            {
                m_currentIndex--;
                UpdateCarousel();
            }
        }

        public void AddCarouselData(string title, string description)
        {
            carouselDatas.Add(new CarouselData { title = title, description = description });
            UpdateCarousel();
        }
    }
}