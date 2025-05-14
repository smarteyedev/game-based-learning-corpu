using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Smarteye.SceneController.taufiq;
using Smarteye.Manager.taufiq;
using System;
using Smarteye.VisualNovel.Character;
using DG.Tweening;

namespace Smarteye.VisualNovel.taufiq
{
    #region   Old-System-Visual-Novel
    public class StoryBlock
    {
        public string naration;
        public string option1Text;
        public string option2Text;
        public StoryBlock Option1Block;
        public StoryBlock Option2Block;

        public StoryBlock(string _naration, string _option1Text = "", string _option2Text = "", StoryBlock _option1Block = null, StoryBlock _option2Block = null)
        {
            this.naration = _naration;
            this.option1Text = _option1Text;
            this.option2Text = _option2Text;
            this.Option1Block = _option1Block;
            this.Option2Block = _option2Block;
        }
    }
    #endregion

    public class VisualNovelController : SceneControllerAbstract
    {
        #region   Old-System-Visual-Novel
        /* public TextMeshProUGUI narationText;
        public Button option1Btn;
        public Button option2Btn;

        private StoryBlock currentBlock;

        static StoryBlock block9 = new StoryBlock("kamu terus-terusan terjebak disituasi saat ini dan tetap susah untuk mendapatkan gaji yang sesuai dengan keinginanmu. kamu tidak berhasil!");
        static StoryBlock block8 = new StoryBlock("Selamat kamu telah diterima di perusahaan impianmu sebagai game programmer dengan gaji sesuai yang kamu inginkan");
        static StoryBlock block7 = new StoryBlock("Kamu saat ini sebagai anak magang yang digaji underrated. sekalipun kamu mengajukan kenaikkan gaji, pengajuanmu ditolak karena posisimu sebagai anak magang. selanjutnya apa yang akan kamu lakukan?", "Mencari perkerjan lain", "Terus berusaha untuk mengembangkan kemampuan diri dan memperbanyak portofolio", block9, block8);
        static StoryBlock block6 = new StoryBlock("kamu telah belajar dengan sungguh-sunggu, namun panitia bootcamp belum sama sekali menawarkan perkejaan untukmu sebagai game programmer", "Mendaftar magang", "Mencari perkerjaan lain", block7, block9);
        static StoryBlock block5 = new StoryBlock("saat ini kamu memiliki dasar pemrograman game, namun kamu masih merasa skill mu masih kurang baik untuk mengerjakan proyek besar, sedangkan tahun depan kamu sudah harus mendaftar kerja", "Mengambil pelatihan untuk seleksi BUMN selama 2 tahun", "Mendaftar magang dengan gajih 2.7 juta", block8, block7);
        static StoryBlock block4 = new StoryBlock("kamu tidak diterima di jurusan teknologi informatika, namun kamu mendapatkan penawaran di jurusan teknologi rekayasa multimedia", "Mengambil bootcamp", "Mengambil tawaran di TRM", block6, block5);
        static StoryBlock block3 = new StoryBlock("Kamu mencari sumber informasi lain seperti tanya ke kakak kelas atau mencari konten youtube yang membahas cara untuk menjadi game programmer. setelah kamu mendaftar ke perusahaan sayangnya kamu ditolak", "Mengikuti bootcamp", "Mencari perkerjaan lain", block6, block9);
        static StoryBlock block2 = new StoryBlock("kamu mendapatkan arahan dari orang tua mu untuk terus melanjutkan ke perkuliahan dan mengambil jurusan teknologi informatika", "Mencari informasi lain", "Mendaftar perkuliahan ke TEL-U", block3, block4);
        static StoryBlock block1 = new StoryBlock("Saat ini aku baru lulus dari SMA, apa cara yang harus aku lakukan ya untuk menjadi seorang game developer yang gajinya sesuai denganku?", "Bertanya ke orang tua", "Cari informasi sendiri ah..", block2, block3);
 */
        #endregion

        [Header("Visual Novel Sytem")]
        [Space(5f)]
        [SerializeField] private VisualNovelView currentView = VisualNovelView.NONE;
        public enum VisualNovelView
        {
            NONE = 0, MYCOON = 1, INTRO = 2, DIALOG = 3, DECISION = 4, RESULT = 5
        }

        [Space(5f)]
        [SerializeField] private List<SceneScenarioDataRoot> temp_BlockScenarioData;

        private SceneScenarioDataRoot m_currentBlockScenario;
        private int m_dialogIndex = 0;

        private State m_VNState = State.COMPLETED;
        public enum State
        {
            PLAYING, SPEEDED_UP, COMPLETED
        }

        private float m_speedFactor = 1f;
        private Coroutine m_myCoroutine = null;
        private Sequence m_sequence;

        [Header("Scoring System")]
        [Space(2f)]
        [SerializeField] private int passingGrage;
        private int m_currentVNScore = 0;

        [Header("Component References for Visual Novel")]
        [Space(5f)]
        [SerializeField] private CharacterData characterData;

        [Space(5f)]
        //? Main UI
        [SerializeField] private CanvasGroup backgroundBlur;
        [SerializeField] private GameObject plankStage;
        [SerializeField] private TextMeshProUGUI textPlankStage;

        //? Introduction Story References
        [Space(5f)]
        [SerializeField] private GameObject panelIntroduction;
        [SerializeField] private TextMeshProUGUI textIntrodutionStory;

        //? dialog references
        [Space(5f)]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private GameObject nextInstruction;
        private bool m_isCanNextVisualNovel = false;
        [SerializeField] private Image imgCharacterDialog;

        //? Decision references
        [Space(10f)]
        [SerializeField] private GameObject decisionPanel;
        [SerializeField] private TextMeshProUGUI textQuestion;
        [SerializeField] private Image imgCharacterDecision;
        [SerializeField] private MultipleButtonInteractive[] buttonOptions;

        [Space(10f)]
        [SerializeField] private GameObject panelFail;
        [SerializeField] private GameObject panelSuccess;

        //? Journal Controller references
        [Space(10f)]
        [SerializeField] private JournalController journalController;
        [SerializeField] private GameObject btnOpenJurnal;

        //? Timer references
        [Space(10f)]
        [SerializeField] private TimeManager timeManager;

        //? Scoring references
        [Space(10f)]
        [SerializeField] private GameObject panelScore;
        [SerializeField] private TextMeshProUGUI textScore;

        protected override void Init()
        {
            //? DisplayBlock(block1); -> this is old-system-visual-novel
            currentView = VisualNovelView.NONE;
            MainUIActive(false);
            m_currentVNScore = 0;

            if (gameManager.currentGameStage == GameStage.PROSPECTINGANDPROFILING)
            {
                temp_BlockScenarioData = gameManager.scenarioLoader.GetScenesByStages(SceneScenarioDataRoot.Stage.PROSPECTINGANDPROFILING);
            }
            else if (gameManager.currentGameStage == GameStage.OBJECTIONANDCLOSING)
            {
                temp_BlockScenarioData = gameManager.scenarioLoader.GetScenesByStages(SceneScenarioDataRoot.Stage.OBJECTIONANDCLOSING, SceneScenarioDataRoot.Stage.EPILOG);
            }
            else if (gameManager.currentGameStage == GameStage.PROBING)
            {
                temp_BlockScenarioData = gameManager.scenarioLoader.GetScenesByStages(SceneScenarioDataRoot.Stage.PROBING);
            }
            else if (gameManager.currentGameStage == GameStage.RAPPORT)
            {
                temp_BlockScenarioData = gameManager.scenarioLoader.GetScenesByStages(SceneScenarioDataRoot.Stage.RAPPORT);
            }
            else if (gameManager.currentGameStage == GameStage.SOLUTION)
            {
                temp_BlockScenarioData = gameManager.scenarioLoader.GetScenesByStages(SceneScenarioDataRoot.Stage.SOLUTION);
            }

            m_currentBlockScenario = temp_BlockScenarioData[0];
            m_isCanNextVisualNovel = false;
            nextInstruction.SetActive(false);

            if (isForDebugging)
            {
                // currentView = VisualNovelView.MYCOONMATERIALS;
                ChangeVisualNovelView(VisualNovelView.NONE);
                return;
            }
            else
            {
                ChangeVisualNovelView(VisualNovelView.NONE);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && m_VNState == State.PLAYING)
            {
                SpeedupRunningText();
                // Debug.Log("Mouse kiri ditekan");
            }
            else if (Input.GetMouseButtonDown(0) && m_VNState == State.COMPLETED && m_isCanNextVisualNovel)
            {
                OnClickNext();
                m_isCanNextVisualNovel = false;
            }

            else if (Input.GetMouseButtonDown(0) && currentView == VisualNovelView.INTRO)
            {
                OnClickCloseIntroStory();
            }
        }

        public void SetVisualNovelView(int _viewIndex) => currentView = (VisualNovelView)_viewIndex;

        public void ChangeVisualNovelView(VisualNovelView _newView)
        {
            currentView = _newView;

            switch (currentView)
            {
                case VisualNovelView.NONE:
                    if (string.IsNullOrEmpty(m_currentBlockScenario.agentAIHint))
                    {
                        ChangeVisualNovelView(VisualNovelView.INTRO);
                    }
                    else
                    {
                        ChangeVisualNovelView(VisualNovelView.MYCOON);
                    }

                    if (!isForDebugging)
                    {
                        /* save to journal for new notes */
                        journalController.AddJurnalNote(gameManager.currentGameStage, m_currentBlockScenario.agentAIHint);
                    }

                    break;
                case VisualNovelView.MYCOON:
                    MainUIActive(false);
                    decisionPanel.SetActive(false);

                    CharacterIdentity.Action.ActionType npcAction = m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.FAILRESULT ?
                                                                    CharacterIdentity.Action.ActionType.CONFUSED : CharacterIdentity.Action.ActionType.HAPPY;

                    mycoonHandler.ShowMycoonInfo("", m_currentBlockScenario.agentAIHint, npcAction, () =>
                    {
                        if (string.IsNullOrEmpty(m_currentBlockScenario.introductionStory))
                            ChangeVisualNovelView(VisualNovelView.DIALOG);
                        else
                        {
                            ChangeVisualNovelView(VisualNovelView.INTRO);
                        }
                    });
                    break;
                case VisualNovelView.INTRO:
                    ShowIntroPanel();
                    break;
                case VisualNovelView.DIALOG:
                    ShowDialogPanel();
                    break;
                case VisualNovelView.DECISION:
                    ShowDecisionPanel();
                    break;
                case VisualNovelView.RESULT:
                    ShowResultPanel();
                    break;
                default:
                    Debug.Log($"it has not played");
                    break;
            }
        }

        private void MainUIActive(bool _isActive)
        {
            backgroundBlur.alpha = _isActive ? 1 : 0;
            plankStage.gameObject.SetActive(_isActive);

            if (_isActive == true)
            {
                textPlankStage.text = GenerateStageName(m_currentBlockScenario.stage);

                if (m_currentBlockScenario == temp_BlockScenarioData[0])
                {
                    timeManager.RestartTimer(300f);
                }
            }

        }

        private String GenerateStageName(SceneScenarioDataRoot.Stage _stage)
        {
            string result = _stage switch
            {
                SceneScenarioDataRoot.Stage.PROLOG => "PROLOG",
                SceneScenarioDataRoot.Stage.PROSPECTINGANDPROFILING => "PROSPECTING & PROFILING",
                SceneScenarioDataRoot.Stage.RAPPORT => "RAPPORT",
                SceneScenarioDataRoot.Stage.PROBING => "PROBING",
                SceneScenarioDataRoot.Stage.SOLUTION => "SOLUTION",
                SceneScenarioDataRoot.Stage.OBJECTIONANDCLOSING => "OBJECTION & CLOSING",
                _ => "none"
            };

            return result;
        }

        #region Panel Introduction Story

        private void ShowIntroPanel()
        {
            panelIntroduction.SetActive(true);
            textIntrodutionStory.text = $"{m_currentBlockScenario.introductionStory}";
        }

        private void OnClickCloseIntroStory()
        {
            if (m_currentBlockScenario.dialogueData.Count == 0)
            {
                if (m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.SUCCESSRESULT
                    || m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.FAILRESULT)
                {
                    ChangeVisualNovelView(VisualNovelView.RESULT);
                }
                else if (m_currentBlockScenario.decisionData != null)
                {
                    ChangeVisualNovelView(VisualNovelView.DECISION);
                }

                panelIntroduction.SetActive(false);
                textIntrodutionStory.text = "";
            }
            else
            {
                panelIntroduction.SetActive(false);
                textIntrodutionStory.text = "";

                ChangeVisualNovelView(VisualNovelView.DIALOG);
            }
        }

        #endregion

        #region Dialog-Visual-Novel-Function

        private void ShowDialogPanel()
        {
            dialogPanel.SetActive(true);
            decisionPanel.SetActive(false);
            MainUIActive(true);
            m_isCanNextVisualNovel = false;

            UpdateDialog(m_currentBlockScenario);
        }

        private void UpdateDialog(SceneScenarioDataRoot _blockScenario)
        {
            if (m_VNState == State.PLAYING && m_myCoroutine != null) return;

            nextInstruction.gameObject.SetActive(false);
            List<SceneScenarioDataRoot.DialogueRoot> narations = _blockScenario.dialogueData;

            string _speakerName = narations[m_dialogIndex].speakerName == SceneScenarioDataRoot.SpeakerRoot.PLAYER ? $"{gameManager.playerData.GetPlayerName()}" : $"{narations[m_dialogIndex].speakerName}";
            speakerNameText.text = $"- {_speakerName} -";
            m_myCoroutine = StartCoroutine(RunningText(narations[m_dialogIndex].narationText, dialogText));

            UpdateCharacterSprite(narations[m_dialogIndex].speakerName.ToString());
        }

        private void OnClickNext()
        {
            if (m_VNState != State.COMPLETED) return;

            if (m_dialogIndex < m_currentBlockScenario.dialogueData.Count - 1)
            {
                m_dialogIndex++;
                UpdateDialog(m_currentBlockScenario);
            }
            else
            {
                m_dialogIndex = 0;

                if (m_currentBlockScenario.decisionData != null
                    && m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.DIALOGUE
                    && m_currentBlockScenario.stage != SceneScenarioDataRoot.Stage.EPILOG)
                {
                    ChangeVisualNovelView(VisualNovelView.DECISION);
                }
                else if (m_currentBlockScenario.decisionData != null
                        && m_currentBlockScenario.stage == SceneScenarioDataRoot.Stage.EPILOG)
                {
                    Debug.LogWarning($"Panell akhirr game... player sukes menyelesaikan game");
                }
                else
                {
                    ChangeVisualNovelView(VisualNovelView.RESULT);
                }
            }
        }

        private IEnumerator RunningText(string text, TextMeshProUGUI target)
        {
            m_VNState = State.PLAYING;
            target.text = "";
            int wordIndex = 0;

            while (m_VNState != State.COMPLETED)
            {
                target.text += text[wordIndex];
                yield return new WaitForSeconds(m_speedFactor * 0.05f);

                if (++wordIndex == text.Length)
                {
                    if (!isPlayNormalSpeed())
                    {
                        m_speedFactor = 1f;
                    }

                    m_VNState = State.COMPLETED;
                    nextInstruction.SetActive(true);
                    m_isCanNextVisualNovel = true;
                    m_myCoroutine = null;
                    break;
                }
            }
        }

        private void SpeedupRunningText()
        {
            if (!isPlayNormalSpeed()) return;

            m_VNState = State.SPEEDED_UP;
            m_speedFactor = 0f;
        }

        private bool isPlayNormalSpeed() => m_speedFactor == 1f && m_VNState == State.PLAYING;

        #endregion

        #region Decision-Making-Function

        private void ShowDecisionPanel()
        {
            decisionPanel.SetActive(true);
            dialogPanel.SetActive(false);
            btnOpenJurnal.SetActive(true);

            UpdateCharacterSprite("NARATOR");

            SetQuestionAndOption();
        }

        private void SetQuestionAndOption()
        {
            SceneScenarioDataRoot.DecisionRoot dec = m_currentBlockScenario.decisionData;

            textQuestion.text = dec.question;

            for (int b = 0; b < buttonOptions.Length; b++)
            {
                if (b < dec.optionDatas.Count)
                {
                    buttonOptions[b].gameObject.SetActive(true);
                    buttonOptions[b].SetOptionText(dec.optionDatas[b].optionText);
                    string nextIdentity = dec.optionDatas[b].nextSceneIdentity;
                    MultipleButtonInteractive btnSelected = buttonOptions[b];
                    int optionScore = dec.optionDatas[b].score;
                    buttonOptions[b].OnMouseDown.AddListener(() =>
                    {
                        AnimationSelectButtonOption(btnSelected, () => OnClickChangeBlock(nextIdentity));
                        m_currentVNScore += optionScore;
                        btnOpenJurnal.SetActive(false);

                        Debug.Log($"Score was added about {optionScore} | current Score is {m_currentVNScore}");
                    });
                }
                else
                {
                    buttonOptions[b].gameObject.SetActive(false);
                }
            }
        }

        private void OnClickChangeBlock(string _targetBlockIndex)
        {
            /* currentBlockScenario = new BlockScenarioDataMap(
                temp_BlockScenarioData[_targetBlockIndex].sceneId,
                temp_BlockScenarioData[_targetBlockIndex].preNarationDatas,
                temp_BlockScenarioData[_targetBlockIndex].decisionData
            ); */

            try
            {
                SceneScenarioDataRoot nextBlock = temp_BlockScenarioData.First((x) => x.sceneIdentity == _targetBlockIndex);
                m_currentBlockScenario = nextBlock;

                ChangeVisualNovelView(VisualNovelView.NONE);

                foreach (var btn in buttonOptions)
                {
                    btn.OnMouseDown.RemoveAllListeners();
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.LogError($"An error occurred while querying data: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"An unexpected error occurred: {ex.Message}");
            }
        }
        #endregion

        #region Result-Panel

        public void ShowResultPanel()
        {
            dialogPanel.SetActive(false);
            MainUIActive(false);
            decisionPanel.SetActive(false);

            if (m_currentVNScore < passingGrage)
            {
                panelFail.SetActive(true);

                return;
            }
            else if (m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.FAILRESULT)
            {
                panelFail.SetActive(true);

                return;
            }

            if (m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.SUCCESSRESULT)
            {
                panelSuccess.SetActive(true);

                journalController.SaveCurrentJurnalNote();
                gameManager.playerData.UpdatePlayerScore(gameManager.currentGameStage, m_currentVNScore);
            }
            else
            {
                Debug.Log($"faild show panel result, because it's dialog blok...");
            }
        }

        public void OnclickResetScene()
        {
            ChangeSceneTo(4);
        }

        public void OnclickBackToStageMap()
        {
            if (m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.SUCCESSRESULT)
            {
                int currentStgIndex = (int)gameManager.currentGameStage;
                // Debug.Log($"currentStage number: {currentStgIndex}");
                gameManager.currentGameStage = (GameStage)currentStgIndex + 1;
                gameManager.playerData.SetPlayerGameStageProgress(gameManager.currentGameStage);
                ChangeSceneTo(2);
            }
            else if (m_currentBlockScenario.sceneProgress == SceneScenarioDataRoot.SceneProgress.FAILRESULT)
            {
                ChangeSceneTo(2);
            }
        }

        #endregion

        #region UI-Animation
        private void UpdateCharacterSprite(string _speakerName)
        {
            if (currentView is VisualNovelView.DIALOG)
            {
                Sprite img = characterData.GetCharacter(GetCurrentSpeaker(_speakerName), CharacterIdentity.Action.ActionType.TALKING);
                if (img != null && GetCurrentSpeaker(_speakerName) != CharacterIdentity.CharacterRole.PLAYER)
                {
                    imgCharacterDialog.sprite = img;
                }
            }

            if (currentView is VisualNovelView.DECISION)
            {
                Sprite img = characterData.GetCharacter(GetCurrentSpeaker(_speakerName), CharacterIdentity.Action.ActionType.CONFUSED);
                imgCharacterDecision.sprite = img;
            }
        }

        private CharacterIdentity.CharacterRole GetCurrentSpeaker(string _speakerName)
        {
            CharacterIdentity.CharacterRole result = _speakerName switch
            {
                "PLAYER" => CharacterIdentity.CharacterRole.PLAYER,
                "NARATOR" => CharacterIdentity.CharacterRole.NARATOR,
                "CLIENT" => CharacterIdentity.CharacterRole.CLIENT,
                "ASISTEN" => CharacterIdentity.CharacterRole.ASISTEN,
                "BOS" => CharacterIdentity.CharacterRole.BOS,
                "SECURITY" => CharacterIdentity.CharacterRole.SECURITY,
                _ => CharacterIdentity.CharacterRole.NONE
            };

            return result;
        }

        private void AnimationSelectButtonOption(MultipleButtonInteractive _btnSelected, Action _onClickAction)
        {
            float _blingDuration = 0.15f;

            if (m_sequence != null)
            {
                Debug.Log("There is an existing canvas group animation, killing it.");
                m_sequence.Kill();
            }
            m_sequence = DOTween.Sequence();

            for (int i = 0; i < buttonOptions.Length; i++)
            {
                if (buttonOptions[i] == _btnSelected)
                {
                    CanvasGroup _cg = buttonOptions[i].GetComponent<CanvasGroup>();
                    for (int j = 0; j < 3; j++) // Melakukan fade in dan out 3 kali
                    {
                        m_sequence.Append(_cg.DOFade(0f, _blingDuration).SetEase(Ease.InOutSine))
                                    .Append(_cg.DOFade(1f, _blingDuration).SetEase(Ease.InOutSine));
                    }

                    m_sequence.AppendInterval(1.5f)
                                .OnKill(() =>
                                {
                                    _onClickAction?.Invoke();
                                });
                }
                else
                {
                    buttonOptions[i].SetAlpha(0);
                }
            }
        }

        #endregion

        #region Scoring System

        public void OpenPanelScore()
        {
            panelScore.SetActive(true);
            textScore.text = m_currentVNScore.ToString();
        }
        #endregion

        #region   Old-System-Visual-Novel
        /* private void DisplayBlock(StoryBlock block)
        {
            narationText.text = block.naration;
            option1Btn.GetComponentInChildren<TextMeshProUGUI>().text = block.option1Text;
            option2Btn.GetComponentInChildren<TextMeshProUGUI>().text = block.option2Text;

            currentBlock = block;
        }

        public void Button1Clicking()
        {
            DisplayBlock(currentBlock.Option1Block);

            CheckingProgress();
        }

        public void Button2Clicking()
        {
            DisplayBlock(currentBlock.Option2Block);

            CheckingProgress();
        }

        private void CheckingProgress()
        {
            if (currentBlock == block9)
            {
                option1Btn.gameObject.SetActive(false);
                option2Btn.gameObject.SetActive(false);
            }
            else if (currentBlock == block8)
            {
                option1Btn.gameObject.SetActive(false);
                option2Btn.gameObject.SetActive(false);
            }
        } */
        #endregion
    }
}