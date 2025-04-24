using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace KreliStudio
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UIScreen : MonoBehaviour
    {

        #region Variables
        public int layer = 0; 
        //public int sortingOrder=0;
        public bool archive = true;
        public Selectable startSelectable;
        public bool isCustomPosition=false;
        public UnityEvent onScreenOpen = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator _animator;
        private bool _isOpen = false;
        #endregion

        #region Main Methods
        public virtual void Init()
        {
            _animator = GetComponent<Animator>();

            if (startSelectable)
            {
                EventSystem.current.SetSelectedGameObject(startSelectable.gameObject);
            }
        }
        public virtual void OpenScreen(object _object = null)
        {
            UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIScreen", "OpenScreen", "Open UI Screen.");
            if (isCustomPosition) {
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }       
            if (onScreenOpen != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIScreen", "On Screen Open", "Event invoke.");
                    onScreenOpen.Invoke();
                }
                        catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event OnScreenOpen in " + name + ". Error: " + exception.Message);
                }
            }
            if (!_isOpen)
                HandleAnimator("Open");

            _isOpen = true;
        }
        public virtual void CloseScreen()
        {
            UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIScreen", "CloseScreen", "Close UI Screen.");
            if (onScreenClose != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIScreen", "On Screen Close", "Event invoke.");
                    onScreenClose.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event OnScreenClose in " +name +". Error: " + exception.Message);
                }
            }
            if (_isOpen)
                HandleAnimator("Close");

            _isOpen = false;
        }
        public virtual void CloseScreenImmediately()
        {
            UIDebug.PrintDebug(UIDebug.DebugType.SCREEN, transform, "UIScreen", "CloseScreenImmediately", "Close UI Screen immediatyly.");
            if (onScreenClose != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIScreen", "On Screen Close", "Event invoke.");
                    onScreenClose.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event OnScreenClose in " + name + ". Error: " + exception.Message);
                }
            }
            if (_isOpen)
                HandleAnimator("CloseImmediately");

            _isOpen = false;

        }

        #endregion

        #region Helper Methods
        private void HandleAnimator(string _trigger)
        {
            if (_animator)
            {
                _animator.SetTrigger(_trigger);
            }
        }
        #endregion
    }
}
