using System;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Smarteye.SceneController.taufiq
{
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Smarteye.AnimationHelper;
    using Smarteye.VisualNovel;

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
            public int companyID;
            public Sprite backgroundSprite;
        }

        [Header("Result IVCA Panel")]
        [SerializeField] private TextMeshProUGUI outputTitle;
        [SerializeField] private TextMeshProUGUI outputSummary;

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
                newOpt.companyID = item.id_company;
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
            gameManager.handlerScenarioData.GetScenarioById(m_TargetCompany, OnSuccessGetScenario, OnProtocolErrGetScenario);
        }

        private void ShowIVCA(string _companyName, string _summary, string _longResume)
        {
            rowInputIVCA.SetActive(false);
            rowOutputIVCA.SetActive(true);

            outputTitle.text = _companyName;
            outputSummary.text = _summary;

            gameManager.currentGameStage = (GameStage)gameManager.currentGameStage + 1;
            gameManager.playerData.SetPlayerGameStageProgress(gameManager.currentGameStage);
            gameManager.playerData.SaveIVCAResult(_companyName, _longResume);

            gameManager.StorePlayerDataToDatabase();
        }

        public string TruncateTo255Characters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input.Length > 255 ? input.Substring(0, 255) : input;
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
                        gameManager.playerData.SetPlayerScenario(fixedJson);

                        gameManager.handlerScenarioData.GetCompanySummary(m_TargetCompany, OnSuccessGetSummary, OnProtocolErrGetSummary);
                        Debug.Log($"Berhasil parsing : {fixedJson}");
                    }
                    else
                    {
                        Debug.LogWarning("Scenario data kosong atau tidak terformat dengan benar.");
                    }
                }
            }
            catch (JsonSerializationException jsonEx)
            {
                /* gameManager.LoadScene(1);
                gameManager.currentGameStage = GameStage.IVCA;
                gameManager.playerData.SetPlayerGameStageProgress(gameManager.currentGameStage);
                gameManager.StorePlayerDataToDatabase(); */

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
            /* { "\"KLIEN\"", "\"CLIENT\"" },
            { "\"TIM TEKNIS\"", "\"ASISTEN\"" },
            { "\"TEMAN\"", "\"BOS\"" },
            { "\"IT STAFF\"", "\"BOS\"" },  */

            // Mapping SceneProgress
            { "\"FAILEDRESULT\"", "\"FAILRESULT\"" },
            { "\"UNEXPECTEDRESULT\"", "\"FAILRESULT\"" },

            // Mapping stage
            /* { "\"INITIAL_MEETING\"", "\"RAPPORT\"" },
            { "\"FOLLOW_UP_MEETING_PREPARATION\"", "\"PROBING\"" },
            { "\"FOLLOW-UP\"", "\"PROBING\"" },
            { "\"WAITING\"", "\"PROBING\"" } */
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

        #region CallBack-GetCompany
        public void OnSuccessGetSummary(JObject result)
        {
            try
            {
                if (result != null)
                {
                    string compName = optionDatas.First((x) => x.companyID == m_TargetCompany).namaPerusahaan;
                    ShowIVCA($"{compName}", result["short_description"].ToString(), result["long_description"].ToString());
                    Debug.Log($"Sort : {result["short_description"]} | Long : {result["long_description"]}");
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

        public void OnProtocolErrGetSummary(JObject result)
        {
            Debug.LogError($"Protocol Error: {GetFormattedError(result)}");
        }

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
        #endregion

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
}