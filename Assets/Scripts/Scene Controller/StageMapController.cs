using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        private int m_stageNumber = 0;

        [Header("Component References")]
        public List<Checkpoint> _progressCheckpoint;

        [Serializable]
        public class Checkpoint
        {
            public GameObject goItem;
            public Sprite enableSprite;
            public Sprite disableSprite;
        }

        protected override void StartOnDebugging()
        {
            base.StartOnDebugging();

            m_stageNumber = 1;
        }

        protected override void Init()
        {
            if (!isForDebugging)
            {
                m_stageNumber = (int)gameManager.currentStage;
            }

            for (int i = 0; i < _progressCheckpoint.Count; i++)
            {
                if (i < m_stageNumber)
                {
                    _progressCheckpoint[i].goItem.GetComponent<Image>().sprite = _progressCheckpoint[i].enableSprite;
                }
                else
                {
                    _progressCheckpoint[i].goItem.GetComponent<Image>().sprite = _progressCheckpoint[i].disableSprite;
                }
            }
        }
    }
}