using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace KreliStudio
{
    [RequireComponent(typeof(RectTransform))]
    public class UIRadialSlider : Selectable, IDragHandler, IBeginDragHandler
    {

        [Serializable]
        public class SliderEvent : UnityEvent<float> { }


        public Image fillArea;
        public Text fillText;
        public RectTransform handle;
        public float minValue = 0f;
        public float maxValue = 1f;
        [SerializeField] private float value = 0f;
        public float Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value =  value;
                OnChangedValue();
            }
        }
        public bool wholeNumbers = false;
        public int textFormat = 0;
        [SerializeField] private int fillOrigin = 0;
        public int FillOrigin
        {
            get
            {
                return fillOrigin;
            }

            set
            {
                fillOrigin = value;
                fillOrigin = Mathf.Clamp(fillOrigin,0,3);
                fillArea.fillOrigin = FillOrigin;
                OnChangedValue();
            }
        }
        public float handleOffset = 0;
        public bool rotateHandle = false;
        [SerializeField] private bool clockwise = false;
        public bool Clockwise
        {
            get
            {
                return clockwise;
            }

            set
            {
                clockwise = value;
                fillArea.fillClockwise = Clockwise;
                OnChangedValue();
            }
        }

        

        public SliderEvent onValueChanged = new SliderEvent();



        private Vector2 offsetDrag; // offset from center of slider to begin Drag
        //private float fillOriginOffset=0;
        public void SetValue()
        {
            Value++;
        }
        

        public void OnChangedValue()
        {
            RedrawSlider();
            RedrawText();
            RedrawHandle();

            if (onValueChanged != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIRadialSlider", "On Changed Value", "Event invoke.");
                    onValueChanged.Invoke(Value);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onValueChanged in " + name + ". Error: " + exception.Message);
                }
            }
        }




        public void Setup()
        {
            fillArea.fillOrigin = FillOrigin;
            fillArea.fillClockwise = Clockwise;
            OnChangedValue();
        }



        public void OnBeginDrag(PointerEventData eventData)
        {
            if (interactable)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIRadialSlider", "OnBeginDrag", "Slider handle begin drag.");
                offsetDrag = handle.anchoredPosition - eventData.position;
            }
            else
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIRadialSlider", "OnBeginDrag", "Can not begin drag radial slider because is not interactable.");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (interactable)
            {
                // align handle to slider circle
                HandleToProgress(eventData.position + offsetDrag);
            }
        }

        private void RedrawSlider()
        {
            fillArea.fillAmount = (value - minValue) / (maxValue - minValue);


        }
        private void RedrawText()
        {
            if (fillText)
            {
                fillText.gameObject.SetActive(true);
                string text = "";
                switch (textFormat)
                {
                    case 0:
                        fillText.gameObject.SetActive(false);
                        return;
                    case 1:
                        if (wholeNumbers)
                            text = value.ToString("0");
                        else
                            text = value.ToString("0.0");
                        break;
                    case 2:
                        if (wholeNumbers)
                            text = value.ToString("0") + "/" + maxValue.ToString("0");
                        else
                            text = value.ToString("0.0") + "/" + maxValue.ToString("0.0");
                        break;
                    case 3:
                        if (wholeNumbers)
                            text = ((value - minValue) / (maxValue - minValue) * 100f).ToString("0") + "%";
                        else
                            text = ((value - minValue) / (maxValue - minValue) * 100f).ToString("0.0") + "%";
                        break;
                }
                fillText.text = text;
            }
        }

        private void RedrawHandle()
        {
            PlaceOnCircle((value - minValue) / (maxValue - minValue));
        }

        private void HandleToProgress(Vector2 position)
        {
            float startAngle = Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg;
            float originOffset = 90f * FillOrigin;
            float angle = startAngle;
            int clockwiseOffset = Clockwise ? 1 : -1;
            angle +=  180f + originOffset;
            angle *= clockwiseOffset;
            float progress =  angle / 360f;
            progress = progress - Mathf.Floor(progress);
            Value = (progress) * (maxValue - minValue) + minValue; ;
        }
        private void PlaceOnCircle(float progress)
        {
            Vector2 position = Vector2.zero;
            float radius = fillArea.GetComponent<RectTransform>().rect.width / 2f + handleOffset;
            float originOffset = 0.5f - 0.25f * FillOrigin;
            int clockwiseOffset = Clockwise ? 1 : -1;
            progress += originOffset * clockwiseOffset;
            progress *= clockwiseOffset;

            float angle = (progress - 180f) * 360f;

            position.x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            position.y = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            if (handle)
            {
                handle.anchoredPosition = position;
                if (rotateHandle)
                    handle.rotation = Quaternion.Euler(0, 0, -angle);
            }
        }
        
    }
}
