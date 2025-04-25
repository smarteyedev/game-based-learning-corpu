using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using UnityEngine;

namespace Smarteye.SceneController.taufiq
{
    public class MenuController : SceneControllerAbstract
    {
        protected override void Init()
        {

        }

        public void OnTutorialComplate()
        {
            ChangeScene(1);
            gameManager.currentStage = (Stage)2;
        }
    }
}