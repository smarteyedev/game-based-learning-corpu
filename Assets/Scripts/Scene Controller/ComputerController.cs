using System;
using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.SceneController.taufiq
{
    public class ComputerController : SceneControllerAbstract
    {
        [Header("Generate IVCA Panel")]
        [SerializeField] private GameObject generateIVCAPanel;
        [SerializeField] private GameObject rowInputIVCA;
        [SerializeField] private GameObject rowOutputIVCA;
        [SerializeField] private InputField companyInput;
        [SerializeField] private Button generateBtn;

        [Header("Result IVCA Panel")]
        [SerializeField] private GameObject resultIVCAPanel;

        #region Old-System
        [SerializeField] private List<CarouselData> carouselDatas;

        [Serializable]
        public class CarouselData
        {
            public string title;
            [TextArea(8, 8)]
            public string description;
        }
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        private int m_currentIndex = 0;

        #endregion

        protected override void Init()
        {
            generateBtn.onClick.AddListener(OnClickSubmit);
            //? previousButton.onClick.AddListener(PreviousItem);
            //? nextButton.onClick.AddListener(NextItem);

            if (isForDebugging) return;

            if (gameManager.currentGameStage == GameStage.IVCA)
            {
                mycoonHandler.ShowMycoonInfo(GameStage.IVCA);
            }
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

                //? UpdateCarousel();
            }
            else
            {
                resultIVCAPanel.SetActive(false);
                generateIVCAPanel.SetActive(true);
            }
        }

        public void OnClickNextStage()
        {
            ChangeSceneTo(2);
            gameManager.currentGameStage = (GameStage)2;
        }

        #region Carousel-Function
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
        #endregion
    }
}