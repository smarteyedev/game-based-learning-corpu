using System;
using System.Linq;
using UnityEngine;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        private int m_stageNumber = 0;

        [Header("Component References")]
        public ButtonStageHandler[] _itemStageBtns;

        protected override void StartOnDebugging()
        {
            base.StartOnDebugging();

            m_stageNumber = 4;
        }

        protected override void Init()
        {
            if (!isForDebugging)
            {
                m_stageNumber = (int)gameManager.currentStage;
            }

            for (int i = 0; i < _itemStageBtns.Length; i++)
            {
                if (i < m_stageNumber)
                {
                    _itemStageBtns[i].SetActiveButton(true);
                    GameObject go = _itemStageBtns[i].gameObject;
                    _itemStageBtns[i].onClickCustom.AddListener(() => CheckPopupActivation(go));
                }
                else
                {
                    _itemStageBtns[i].SetActiveButton(false);
                    GameObject go = _itemStageBtns[i].gameObject;
                    _itemStageBtns[i].onClickCustom.AddListener(() => CheckPopupActivation(go));
                }

                if (i == m_stageNumber - 1)
                {
                    _itemStageBtns[i].SetupBtnChangeScene(true, ChangeScene);
                }
                else
                {
                    _itemStageBtns[i].SetupBtnChangeScene(false);
                }
            }
        }

        public void CheckPopupActivation(GameObject go)
        {
            for (int i = 0; i < _itemStageBtns.Length; i++)
            {
                if (go == _itemStageBtns[i].gameObject)
                {
                    _itemStageBtns[i].OpenPopupDetail(true);
                }
                else
                {
                    _itemStageBtns[i].OpenPopupDetail(false);
                }
            }
        }

        public void ResetPopup()
        {
            bool hasPopupActive = _itemStageBtns.Any((x) => x.isPopupOpen);
            if (hasPopupActive)
            {
                foreach (var item in _itemStageBtns)
                {
                    if (item.isPopupOpen) item.OpenPopupDetail(false);
                }
            }
        }
    }
}