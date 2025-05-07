using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using UnityEngine;
using UnityEngine.Events;

namespace Smarteye.SceneController.taufiq
{
    public class MenuController : SceneControllerAbstract
    {
        [Header("Unity Event")]
        public UnityEvent OnOpeningComplate;
        protected override void Init()
        {

        }

        public void HasFinishedOpening()
        {
            mycoonHandler.ShowMycoonInfo(GameStage.None, () =>
            {
                ChangeSceneTo(2);
                gameManager.currentGameStage = (GameStage)1;
            });

            OnOpeningComplate?.Invoke();
        }
    }
}