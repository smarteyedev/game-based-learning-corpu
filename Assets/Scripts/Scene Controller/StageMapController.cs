using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        private int m_stageNumber = 0;

        [Header("Component References")]
        [SerializeField] private GameObject btnStageParent;
        [SerializeField] private List<ButtonStageHandler> _itemStageBtns;

        [Header("Scoring System")]
        [SerializeField] private GameObject panelScoring;
        [SerializeField] private TextMeshProUGUI textScore;
        [SerializeField] private Image[] badges;

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


        #region Score-Title

        private void OpenScoringPanel()
        {
            OnScoringPanelView(true);

            textScore.text = $"{gameManager.playerData.GetTotalScore()} / 100";
        }

        public void OnScoringPanelView(bool _isActive)
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

        #endregion 
    }
}