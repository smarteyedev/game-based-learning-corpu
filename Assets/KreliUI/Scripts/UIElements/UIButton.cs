using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace KreliStudio
{

    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class UIButton : Button
    {
        
        public Actions actionsOnClick;
        public Actions actionsOnDoubleClick;
        public Actions actionsOnLongClick;
        public Actions actionsOnPointerEnter;
        public Actions actionsOnPointerExit;
        public Actions actionsOnPointerDown;
        public Actions actionsOnPointerUp;



        // properties for custom editor script
        public bool editorMenuSelectableProperties = true;
        //Click Type Checker (One Click, Double Click, Long Click)
        private int _clickCount = 0;
        private bool isPressed = false;
        private bool isCheckerRunning = false;
        private bool isLongClick = false;


        public override void OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
            {
                base.OnPointerClick(eventData);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                base.OnPointerDown(eventData);
                actionsOnPointerDown.Action();

                isPressed = true;
                StartCoroutine("LongClickChecker");
            }
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (interactable)
            {
                base.OnPointerUp(eventData);
                actionsOnPointerUp.Action();
                isPressed = false;
                if (!isLongClick)
                {
                    StopCoroutine("LongClickChecker");
                    if (actionsOnDoubleClick.IsEnable())
                    {
                        _clickCount = eventData.clickCount;
                        if (!isCheckerRunning)
                            StartCoroutine("OneAndDoubleClickChecker");
                    }
                    else
                    {
                        //Debug.Log("One Click");
                        UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIButton", "OnClick", "Button was clicked once.");
                        actionsOnClick.Action();
                    }
                }
                else
                {
                    isLongClick = false;
                }
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            actionsOnPointerEnter.Action();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            actionsOnPointerExit.Action();
        }

        IEnumerator OneAndDoubleClickChecker()
        {
            isCheckerRunning = true;
            yield return new WaitForSeconds(0.25f);

            if (_clickCount >= 2)
            {
                //Debug.Log("Double Click");
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIButton", "OnDoubleClick", "Button was clicked twice.");
                actionsOnDoubleClick.Action();
                
            }
            else
            if (_clickCount == 1)
            {
                //Debug.Log("One Click 2");
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIButton", "OnClick", "Button was clicked once.");
                actionsOnClick.Action();                
            }
            isCheckerRunning = false;
        }

        IEnumerator LongClickChecker()
        {
            yield return new WaitForSeconds(0.5f);
            if (isPressed)
            {
                if (actionsOnLongClick.IsEnable())
                {
                    //Debug.Log("Long Click");
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIButton", "OnLongClick", "Button was long pressed.");
                    isLongClick = true;
                    actionsOnLongClick.Action();
                }
            }
        }
    }
}