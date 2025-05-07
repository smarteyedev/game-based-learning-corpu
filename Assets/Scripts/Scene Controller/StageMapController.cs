using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        private int m_stageNumber = 0;

        [Header("Component References")]
        public List<ButtonStageHandler> _itemStageBtns;

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

                // Debug.Log($"current stage: {m_stageNumber}");
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
        }

        public void OpenPopup(int _btnIndex)
        {
            // for (int i = 0; i < _itemStageBtns.Count; i++)
            // {
            //     if (go == _itemStageBtns[i].gameObject)
            //     {
            //         _itemStageBtns[i].OpenPopupDetail(true);
            //     }
            //     else
            //     {
            //         _itemStageBtns[i].OpenPopupDetail(false);
            //     }
            // }

            ResetPopup();
            _itemStageBtns[_btnIndex].OpenPopupDetail(true);

            // Debug.Log($"try to open button {_itemStageBtns[_btnIndex]}");
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
    }
}