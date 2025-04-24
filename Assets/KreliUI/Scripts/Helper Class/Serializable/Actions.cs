using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KreliStudio {
    [Serializable]
    public class Actions{

        public GameObject parent;
        //customEditor
        public bool isOpen=false;
        public bool isEventOpen = false;
        public bool isUIScreenOpen = false;
        public bool isSceneLoaderOpen = false;

        //Bezier between Button and UIScreen
        public Vector3 startPoint = new Vector3(-0.0f, 0.0f, 0.0f);
        public Vector3 endPoint = new Vector3(-2.0f, 2.0f, 0.0f);
        public Vector3 startTangent = Vector3.zero;
        public Vector3 endTangent = Vector3.zero;


        // event section
        public bool isEvent = false;
        public UnityEvent unityEvent = new UnityEvent();

        // UIScreen section
        public bool isUIScreen = false;
        public int uiScreenIndex;
        public UIScreen uiScreen;


        // SceneLoader section
        public bool isSceneLoader = false;
        public int buildIndex;

        // sound & vibration
       /* public bool isSound=false;
        public Sound sound;
        public bool isVibration=false;
        public int vibrate=100;*/

        /*public void AddParent(GameObject _parent)
        {
            parent = _parent;
            sound.AddParent(parent);
        }*/
        
        public void Action()
        {
            // events
            if (isEvent)
            {
                InvokeEvent();
            }
            // UIScreen
            if (isUIScreen)
            {
                GoToUIScreen();
            }
            // Scene Loader
            if (isSceneLoader)
            {
                LoadScene();
            }
            /*
            if (isSound && sound.clip)
            {
                sound.Play();
            }
            if (isVibration)
            {
                Vibration.Vibrate(vibrate);
            }*/

        }

        public bool IsEnable()
        {
            return ((isEvent || isUIScreen) || isSceneLoader);
        }


        private void InvokeEvent()
        {
            if (unityEvent != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, null, "Action", "InvokeEvent", "Event invoke.");
                    unityEvent.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event Action. Error: " + exception.Message);
                }
            }
        }

        private void GoToUIScreen()
        {
            if (uiScreen != null)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.EVENT, null, "Action", "GoToUIScreen", "Go to screen " + uiScreen.name + ".");
                UIManager.Instance.GoToScreen(uiScreen);
            }
            else
            {
                UIDebug.PrintDebug(UIDebug.DebugType.EVENT, null, "Action", "GoToUIScreen", "Go to previous screen.");
                UIManager.Instance.GoToPreviuosScreen();
            }

        }

        private void LoadScene()
        {
            if (UISceneManager.Instance)
            {
                if (buildIndex == -100)
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, null, "Action", "LoadScene", "Go to previous scene.");
                    UISceneManager.Instance.LoadPreviousScene();
                }
                else
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, null, "Action", "LoadScene", "Go to scene (buildindex " + buildIndex + ").");
                    UISceneManager.Instance.LoadScene(buildIndex);
                }
            }
            else
                Debug.LogWarning("Can not load scene because UI Scene Manager instance not found.");
        }



    }
    
}