using System;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Smarteye.SceneController.taufiq
{

    using Smarteye.AnimationHelper;

    public class ComputerController : SceneControllerAbstract
    {
        [Header("Generate IVCA Panel")]
        [SerializeField] private GameObject introIVCAPanel;
        [SerializeField] private GameObject generateIVCAPanel;
        [SerializeField] private GameObject rowInputIVCA;
        [SerializeField] private GameObject rowOutputIVCA;

        [Space(10)]
        [SerializeField] private TMP_Dropdown dropDown;
        [SerializeField] private Button generateBtn;
        [SerializeField] private List<OptionData> optionDatas;
        [SerializeField] private Sprite dropDownBg;

        private int m_TargetCompany = 0;

        [Serializable]
        public struct OptionData
        {
            public string namaPerusahaan;
            public int tokenIVCA;
            public Sprite backgroundSprite;
        }

        [Header("Result IVCA Panel")]
        [SerializeField] private TextMeshProUGUI outputTitle;
        [SerializeField] private TextMeshProUGUI outputSummary;
        [Space(10)]
        [SerializeField] private IVCADataMap outputIVCAData;

        [Space(10)]
        [SerializeField] private GameObject resultIVCAPanel;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI resultText;

        #region #region Carousel-Function
        // [SerializeField] private List<CarouselData> carouselDatas;

        // [Serializable]
        // public class CarouselData
        // {
        //     public string title;
        //     [TextArea(8, 8)]
        //     public string description;
        // }
        // [SerializeField] private TextMeshProUGUI titleText;
        // [SerializeField] private TextMeshProUGUI descriptionText;
        // [SerializeField] private Button nextButton;
        // [SerializeField] private Button previousButton;
        // private int m_currentIndex = 0;
        #endregion

        protected override void Init()
        {
            m_TargetCompany = 0;

            generateBtn.onClick.AddListener(OnClickSubmit);
            dropDown.onValueChanged.AddListener(OnDropDownValueChange);
            //? previousButton.onClick.AddListener(PreviousItem);
            //? nextButton.onClick.AddListener(NextItem);

            if (isForDebugging) return;

            optionDatas.Clear();

            var tCompanies = gameManager.handlerScenarioData.GetCompanyList();

            foreach (var item in tCompanies)
            {
                OptionData newOpt = new OptionData();
                newOpt.namaPerusahaan = item.company_name;
                newOpt.tokenIVCA = item.id_company;
                newOpt.backgroundSprite = dropDownBg;

                optionDatas.Add(newOpt);
            }

            introIVCAPanel.SetActive(true);
            AnimationHelper.FadeInCanvasGroup(introIVCAPanel.GetComponent<CanvasGroup>(), 1.5f);
        }

        public void OpenPanelRowInput()
        {
            generateIVCAPanel.SetActive(true);

            foreach (var item in optionDatas)
            {
                TMP_Dropdown.OptionData newData = new TMP_Dropdown.OptionData();
                newData.text = item.namaPerusahaan;
                newData.image = item.backgroundSprite;

                dropDown.options.Add(newData);
            }
        }

        public void OnClickSubmit()
        {
            gameManager.handlerScenarioData.GetScenarioById(m_TargetCompany);
            gameManager.handlerScenarioData.GetCompanySummary(m_TargetCompany, ShowIVCA);
            // ShowIVCA();
        }

        private void ShowIVCA(string _companyName, string _summary, string _longResume)
        {
            rowInputIVCA.SetActive(false);
            rowOutputIVCA.SetActive(true);

            /* outputTitle.text = outputIVCAData.companyName;
            outputSummary.text = outputIVCAData.summary; */

            outputTitle.text = _companyName;
            outputSummary.text = _summary;

            gameManager.currentGameStage = (GameStage)gameManager.currentGameStage + 1;
            gameManager.playerData.SetPlayerGameStageProgress(gameManager.currentGameStage);
            // gameManager.playerData.SaveIVCAResult(outputIVCAData.companyName, outputIVCAData.IVCAResult);
            gameManager.playerData.SaveIVCAResult(_companyName, _longResume);
        }

        public void OnClickOpenIVCAResult(bool _isActive)
        {
            if (_isActive)
            {
                generateIVCAPanel.SetActive(false);
                resultIVCAPanel.SetActive(true);

                var temp_ivca = gameManager.playerData.GetIVCAData();

                resultTitleText.text = temp_ivca[0];
                resultText.text = temp_ivca[1];

                //? m_currentIndex = 0;
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

        public void OnDropDownValueChange(int _val)
        {
            // Debug.Log($"{_val}");
            m_TargetCompany = _val;

            if (dropDown.value == 0)
            {
                generateBtn.gameObject.SetActive(false);
            }
            else
            {
                generateBtn.gameObject.SetActive(true);
            }
        }

        #region Carousel-Function
        // public void UpdateCarousel()
        // {
        //     if (carouselDatas.Count == 0) return;

        //     titleText.text = carouselDatas[m_currentIndex].title;
        //     descriptionText.text = carouselDatas[m_currentIndex].description;

        //     previousButton.interactable = m_currentIndex > 0;
        //     nextButton.interactable = m_currentIndex < carouselDatas.Count - 1;
        // }

        // void NextItem()
        // {
        //     if (m_currentIndex < carouselDatas.Count - 1)
        //     {
        //         m_currentIndex++;
        //         UpdateCarousel();
        //     }
        // }

        // void PreviousItem()
        // {
        //     if (m_currentIndex > 0)
        //     {
        //         m_currentIndex--;
        //         UpdateCarousel();
        //     }
        // }

        // public void AddCarouselData(string title, string description)
        // {
        //     carouselDatas.Add(new CarouselData { title = title, description = description });
        //     UpdateCarousel();
        // }
        #endregion
    }

    [Serializable]
    public class IVCADataMap
    {
        public string companyName;
        public string summary;
        public string IVCAResult;
    }
}