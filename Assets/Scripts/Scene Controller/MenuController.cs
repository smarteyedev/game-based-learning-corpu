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
            mycoonHandler.ShowMycoonInfo(() =>
            {
                ChangeSceneTo(2);
                gameManager.currentStage = (Stage)1;
            });

            OnOpeningComplate?.Invoke();
        }
    }
}