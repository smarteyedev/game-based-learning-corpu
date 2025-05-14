using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Smarteye.Manager.taufiq;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        private int m_stageNumber = 0;

        [Header("Component References")]
        [SerializeField] private GameObject btnStageParent;
        [SerializeField] private List<ButtonStageHandler> _itemStageBtns;

        [Header("Scoring System")]
        [SerializeField] private BadgeSetting[] badges;

        [Serializable]
        public class BadgeSetting
        {
            public string badgeName;
            public int minScore;
            public int maxScore;
            public Sprite badgeSprite;
        }

        [SerializeField] private TitleSetting[] playerTitles;
        [Serializable]
        public class TitleSetting
        {
            public string titleName;
            public Image targetImage;
            public Sprite activeSprite;
            public Sprite nonactiveSprite;
        }
        [Space(5f)]

        [SerializeField] private GameObject panelScoring;
        public CanvasGroup objectA;
        public RectTransform objectB;
        public RectTransform objectC;
        [Space(2f)]

        [SerializeField] private TextMeshProUGUI textScore;
        private int m_displayedScore = 0;
        [SerializeField] private Image[] imgBadges;
        [SerializeField] private Button btnCloseScorePanel;
        private List<String> BadgeCollected = new List<String>();
        private Sequence sequence;


        protected override void StartOnDebugging()
        {
            base.StartOnDebugging();

            m_stageNumber = 4;
        }

        protected override void Init()
        {
            if (!isForDebugging)
            {
                m_stageNumber = (int)gameManager.currentGameStage;

                Debug.Log($"Opening Stage-Map Scene | current stage-progress number: {m_stageNumber}");
            }

            for (int i = 0; i < _itemStageBtns.Count; i++)
            {
                int indexTarget = i;
                _itemStageBtns[i].onClickCustom.AddListener(() => OpenPopup(indexTarget));

                if (i < m_stageNumber)
                {
                    _itemStageBtns[i].SetActiveButton(true);
                }
                else
                {
                    _itemStageBtns[i].SetActiveButton(false);
                }

                if (i == m_stageNumber - 1)
                {
                    // Debug.Log($"{_itemStageBtns[i].gameObject.name}");
                    _itemStageBtns[i].SetupBtnChangeScene(true, ChangeSceneTo);
                }
                else
                {
                    _itemStageBtns[i].SetupBtnChangeScene(false);
                }
            }

            if (m_stageNumber == 7)
            {
                Debug.Log($"Current stage number: {m_stageNumber} | Game is finish and open popup score title");
                OpenScoringPanel();
            }
        }

        public void OpenPopup(int _btnIndex)
        {
            ResetPopup();
            _itemStageBtns[_btnIndex].OpenPopupDetail(true);
        }

        public void ResetPopup()
        {
            var hasPopupActive = _itemStageBtns.FindAll((x) => x.isPopupOpen == true).ToList();

            if (hasPopupActive.Count > 0)
            {
                foreach (var item in _itemStageBtns)
                {
                    item.OpenPopupDetail(false);
                }
            }
        }


        #region Scoring-Panel
        private void OpenScoringPanel()
        {
            AnimatePanelScoring();
            btnCloseScorePanel.onClick.AddListener(() => { OnScoringPanelView(false); });
        }

        private void OnScoringPanelView(bool _isActive)
        {
            if (_isActive)
            {
                panelScoring.SetActive(true);
                btnStageParent.SetActive(false);
            }
            else
            {
                panelScoring.SetActive(false);
                btnStageParent.SetActive(true);
            }
        }

        private void AnimatePanelScoring()
        {
            OnScoringPanelView(true);
            int targetScore = gameManager.playerData.GetTotalScore();

            sequence = DOTween.Sequence();

            sequence.Append(objectA.DOFade(1f, 1f)) //? 1. Fade A dalam 1 detik
                .Append(DOTween.To(() => m_displayedScore, x =>
                {
                    m_displayedScore = x;
                    textScore.text = $"{m_displayedScore} / 100"; //? 2. Update text tiap frame
                }, targetScore, 2f)
                .SetEase(Ease.InOutSine).OnPlay(() =>
                {
                    // Debug.Log($"Start change score value...");
                    ShowAllBadgesSequentially(); //? 3. Fade 5 stage badge
                }))
                /* .AppendCallback(() =>
                {
                    Debug.Log($"fungsi lain");
                })
                .AppendInterval(3f) */ // Contoh Delay 3 detik
                .Append(objectB.DOAnchorPosX(-475f, 1f)) //? 4. Geser B ke kiri dalam 1 detik
                .AppendCallback(() =>
                {
                    ShowPlayerTitle(); //? 5. SetActive player title
                    objectC.gameObject.SetActive(true);

                    //? 6. Mulai rotasi C terus-menerus
                    objectC.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Restart);
                })
                .AppendCallback(() =>
                {
                    //? 7. Tampilkan button close
                    btnCloseScorePanel.gameObject.SetActive(true);
                    btnCloseScorePanel.GetComponent<Image>().DOFade(1f, 3f);

                    // Optional: Cleanup Sequence
                    sequence.Kill();
                    sequence = null;
                });
        }

        private void ShowAllBadgesSequentially()
        {
            if (imgBadges == null || imgBadges.Length == 0)
            {
                Debug.LogWarning("imgBadges kosong, tidak ada yang dianimasikan.");
                return;
            }

            Sequence badgeSequence = DOTween.Sequence();

            for (int i = 0; i < imgBadges.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        imgBadges[i].sprite = GetBadgeByScore(gameManager.playerData.GetScoreByGameStage(GameStage.PROSPECTINGANDPROFILING));
                        break;
                    case 1:
                        imgBadges[i].sprite = GetBadgeByScore(gameManager.playerData.GetScoreByGameStage(GameStage.RAPPORT));
                        break;
                    case 2:
                        imgBadges[i].sprite = GetBadgeByScore(gameManager.playerData.GetScoreByGameStage(GameStage.PROBING));
                        break;
                    case 3:
                        imgBadges[i].sprite = GetBadgeByScore(gameManager.playerData.GetScoreByGameStage(GameStage.SOLUTION));
                        break;
                    case 4:
                        imgBadges[i].sprite = GetBadgeByScore(gameManager.playerData.GetScoreByGameStage(GameStage.OBJECTIONANDCLOSING));
                        break;
                }

                imgBadges[i].color = new Color(imgBadges[i].color.r, imgBadges[i].color.g, imgBadges[i].color.b, 0f);
                badgeSequence.Append(imgBadges[i].DOFade(1f, 0.4f));
            }

            badgeSequence.OnComplete(() =>
            {
                // Debug.Log("Semua badge selesai di-fade.");
            });
        }

        private void ShowPlayerTitle()
        {
            ResetAllTitles();

            int goldCount = CountBadge("Gold");
            int silverCount = CountBadge("Silver");
            int bronzeCount = CountBadge("Bronze");

            if (goldCount == 5)
            {
                SetActiveSprites(0, playerTitles.Length - 1); // Consultative Master
            }
            else if (goldCount == 4)
            {
                SetActiveSprites(1, 3); // Strategic Challenger
            }
            else if (goldCount > 0)
            {
                SetActiveSprites(2, 3); // Engaged Collaborator
            }
            else if (silverCount >= 3)
            {
                SetActiveSprites(2, 3); // Engaged Collaborator
            }
            else if (bronzeCount < 3)
            {
                SetActiveSprites(3, 3); // Learning Advocate
            }
            else
            {
                SetActiveSprites(3, 3); // Learning Advocate
            }
        }

        private int CountBadge(string badgeName)
        {
            return BadgeCollected.Count(badge => badge == badgeName);
        }

        private void ResetAllTitles()
        {
            foreach (var title in playerTitles)
            {
                title.targetImage.sprite = title.nonactiveSprite;
            }
        }

        private void SetActiveSprites(int startIndex, int endIndex)
        {
            // Pastikan index valid
            startIndex = Mathf.Clamp(startIndex, 0, playerTitles.Length - 1);
            endIndex = Mathf.Clamp(endIndex, 0, playerTitles.Length - 1);

            for (int i = startIndex; i <= endIndex; i++)
            {
                playerTitles[i].targetImage.sprite = playerTitles[i].activeSprite;
            }
        }

        private Sprite GetBadgeByScore(int _score)
        {
            Sprite targetBadge = null;

            foreach (var badge in badges)
            {
                if (_score >= badge.minScore && _score <= badge.maxScore)
                {
                    targetBadge = badge.badgeSprite;
                    BadgeCollected.Add(badge.badgeName);
                    return targetBadge;
                }
            }

            return targetBadge = null;
        }
        #endregion 
    }
}