using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.SceneController.taufiq
{
    public class StageMapController : SceneControllerAbstract
    {
        [Header("Component References")]
        public List<Checkpoint> _progressCheckpoint;

        [Serializable]
        public class Checkpoint
        {
            public GameObject goItem;
            public Sprite enableSprite;
            public Sprite disableSprite;
        }

        protected override void Init()
        {
            for (int i = 0; i < _progressCheckpoint.Count; i++)
            {
                if (i < (int)gameManager.currentStage)
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