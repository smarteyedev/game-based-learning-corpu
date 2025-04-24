using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace KreliStudio
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        
        public UnityEvent onSwitchedScreen = new UnityEvent();
        [SerializeField] private Component[] _screens = new Component[0];
        [SerializeField] private UIScreen[] _currentScreens = new UIScreen[11];
        public UIScreen[] CurrentScreens { get { return _currentScreens; } }
        [SerializeField] private int _currentLayer = 0;
        public int CurrentLayer { get { return _currentLayer; } }
        [SerializeField] private List<UIScreen> _historyStack = new List<UIScreen>();
        public List<UIScreen> HistoryStack { get { return _historyStack; } }
        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }
        public bool debugUIElements = false;
        public bool debugEvents = false;
        public bool debugUIScreen = false;
        public bool debugUIScene = false;
        [SerializeField] private int openTab;
        #endregion

        #region Main Methods
        void Start()
        {
            // init can be call from GameManager script or simillar
            Init();
        }

        public void Init()
        {
            Singleton();
            _screens = GetComponentsInChildren<UIScreen>(true);

            foreach (var _screen in _screens)
            {
                _screen.gameObject.SetActive(true);
                _screen.GetComponent<UIScreen>().Init();
            }

            
            
        }
        public void GoToScreen(UIScreen _nextScreen)
        {
            GoToScreen(_nextScreen, null);
        }
        public void GoToScreen(UIScreen _nextScreen, object _object)
        {
            if (_nextScreen != null)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIManager", "GoToScreen", "Go to UI Screen " + _nextScreen.name);
                int nextLayer = _nextScreen.layer;
                // add current screen to history
                // if current screen exist
                if (_currentScreens[_currentLayer])
                {
                    // and if we dont open the same screen again
                    if (_currentScreens[_currentLayer] != _nextScreen)
                        // and if this screen have permission to add to history
                        if (_currentScreens[_currentLayer].archive)
                            // then add it to history array
                            _historyStack.Add(_currentScreens[_currentLayer]);
                }

                // close all screens from current layer and smaller to next layer without this one want to open
                // if actual open smaller or equal layer than was opened
                if (_currentLayer >= nextLayer)
                {
                    int i = _currentLayer;
                    do
                    {
                        // if screen exist in current array is equal it is open
                        if (_currentScreens[i])
                        {
                            // if screen is not this want to open
                            if (_currentScreens[i] != _nextScreen)
                            {
                                // close it
                                _currentScreens[i].CloseScreen();
                                // and clear place in current array
                                _currentScreens[i] = null;
                            }
                        }
                        i--;
                    } while (i >= nextLayer);
                }

                // open next screen
                // if we dont open the same screen again
                //if (_currentScreen[nextLayer] != _nextScreen)
                //{
                    // set opening screen to current array on his layer
                    _currentScreens[nextLayer] = _nextScreen;
                    // just in case active gameObject
                    _currentScreens[nextLayer].gameObject.SetActive(true);
                    // set object on "Top" in hierarchy
                    _currentScreens[nextLayer].transform.SetAsLastSibling();
                    // open screen with parameter _object
                    _currentScreens[nextLayer].OpenScreen(_object);
                //}
                // set new current layer
                _currentLayer = nextLayer;

                // if are any events then invoke them
                if (onSwitchedScreen != null)
                {
                    try
                    {
                        UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIManager", "On Switched Screen", "Event invoke.");
                        onSwitchedScreen.Invoke();
                    }
                    catch (System.Exception exception)
                    {
                        Debug.LogError("Couldn't invoke event OnSwitchedScreen in " + name + ". Error: " + exception.Message);
                    }
                }
            }
        }
        public void GoToPreviuosScreen()
        {
            // if history is not empty then
            if (_historyStack.Count > 0)
            {
                // set last index
                int lastIndex = _historyStack.Count - 1;
                UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIManager", "GoToPreviuosScreen", "Go to previous UI Screen " + _historyStack[lastIndex].name + ",");
                // call metod GoToScreen with last closed screen
                GoToScreen(_historyStack[lastIndex]);
                // remove all index from history array from "lastIndex" to end of array
                // Called GoToScreen add new screens to history then we have to remove from lastIndex to end of array
                _historyStack.RemoveRange(lastIndex, _historyStack.Count - lastIndex);
            }else
                UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIManager", "GoToPreviuosScreen", "Can not go to previous UI Screen because history stack is empty.");
        }
        public void CloseAllUIScreenImmediately()
        {
            foreach(UIScreen s in _currentScreens)
            {
                if (s)
                    s.CloseScreenImmediately();
            }

        }
        public UIScreen GetUIScreenByName(string name)
        {
            UIScreen s = Array.Find(_screens, searchedObject => searchedObject.name == name) as UIScreen;
            if (s ==null)
            {
                Debug.LogError("UIScreen " + name +" not found.");
            }

            return s;
        }
        public UIScreen GetCurrentScreen()
        {
            return _currentScreens[_currentLayer];
        }
        public void PlaySound(AudioClip audioClip)
        {
            AudioSource source = null;
            foreach (AudioSource s in GetComponents<AudioSource>())
            {
                if (s.clip == audioClip)
                {
                    source = s;
                }
            }

            if (source)
            {
                source.Play();
            }
            else
            {
                source = gameObject.AddComponent<AudioSource>();
                source.clip = audioClip;
                source.Play();
            }

        }
        public void Vibrate(int duration)
        {
            Vibration.Vibrate(duration * 100);
        }
        
        #endregion

        #region Helper Methods
        private void CreateSource()
        {
        }
        private void Singleton()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
                Destroy(gameObject);
        }
        #endregion
    }
}
