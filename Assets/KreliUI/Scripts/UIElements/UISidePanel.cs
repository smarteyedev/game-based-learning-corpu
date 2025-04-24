using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KreliStudio
{
    [RequireComponent(typeof(RectTransform))]
    public class UISidePanel : MonoBehaviour
    {
        public RectTransform panel;
        public Image background;
        public RectTransform button;
        public RectTransform swipe;

        public int side;
        public bool isPanelFade;
        public int oldSide; // helper var for auto set button
        public bool isPercent; // pixel or percent
        public float widthMove;
        public float speed=1f;
        public bool isBackgroundFade;
        public bool closeOnBackground;
        public float backgroundFade;

        public bool isButton; // Open by button or swipe
        public bool isSticky; // button stick to panel
        public bool isElastic; // swipebar elastic or clamped drag 
        public Vector2 buttonAnchorsPosition;
        public Vector2 buttonOffsetPosition;

        public bool isFullScreenSwipe;

        public UnityEvent onOpen = new UnityEvent();
        public UnityEvent onClose = new UnityEvent();

        private bool isOpen = false;
        private bool isMoving=false;


        private void Start()
        {
            Init();
        }
        private void Update()
        {
            Fade();


        }


        public virtual void ButtonAction()
        {
            if (isOpen)
                Close();
            else
                Open();
        }
        
        public virtual void Open()
        {
            if (!isOpen)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UISidePanel", "Open", "Open side panel.");
                isOpen = true;
                switch (side)
                {
                    case 0: // Left
                        StartCoroutine(MoveTo(new Vector2(panel.rect.width, 0f), 1f / speed));
                        break;
                    case 1: // Top
                        StartCoroutine(MoveTo(new Vector2(0f, -panel.rect.height), 1f / speed));
                        break;
                    case 2: // Right
                        StartCoroutine(MoveTo(new Vector2(-panel.rect.width, 0f), 1f / speed));
                        break;
                    case 3: // Bottom
                        StartCoroutine(MoveTo(new Vector2(0f, panel.rect.height), 1f / speed));
                        break;
                }
            }
        }

        public virtual void Close()
        {
            if (isOpen)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UISidePanel", "Close", "Close side panel.");
                isOpen = false;
                switch (side)
                {
                    case 0: // Left
                        StartCoroutine(MoveTo(Vector2.zero, 1f / speed));
                        break;
                    case 1: // Top
                        StartCoroutine(MoveTo(Vector2.zero, 1f / speed));
                        break;
                    case 2: // Right
                        StartCoroutine(MoveTo(Vector2.zero, 1f / speed));
                        break;
                    case 3: // Bottom
                        StartCoroutine(MoveTo(Vector2.zero, 1f / speed));
                        break;
                }
            }
        }

        protected virtual void OnOpen()
        {
            if (closeOnBackground)
                background.raycastTarget = true;
            if (onOpen != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UISidePanel", "On Open", "Event invoke.");
                    onOpen.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onOpen in " + name + ". Error: " + exception.Message);
                }
            }
        }
        protected virtual void OnClose()
        {
            if (closeOnBackground)
                background.raycastTarget = false;
            if (onClose != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UISidePanel", "On Close", "Event invoke.");
                    onClose.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onClose in " + name + ". Error: " + exception.Message);
                }
            }
        }
        public virtual void OnEndDragSwipeBar()
        {
            switch (side)
            {
                case 0: // Left
                    if (panel.anchoredPosition.x > panel.rect.width/2)
                    {
                        isOpen = false;
                        Open();
                    }else
                    {
                        isOpen = true;
                        Close();
                    }
                    break;
                case 1: // Top
                    if (panel.anchoredPosition.y < -panel.rect.height/2)
                    {
                        isOpen = false;
                        Open();
                    }
                    else
                    {
                        isOpen = true;
                        Close();
                    }
                    break;
                case 2: // Right
                    if (panel.anchoredPosition.x < -panel.rect.width/2)
                    {
                        isOpen = false;
                        Open();
                    }
                    else
                    {
                        isOpen = true;
                        Close();
                    }
                    break;
                case 3: // Bottom
                    if (panel.anchoredPosition.y > panel.rect.height/2)
                    {
                        isOpen = false;
                        Open();
                    }
                    else
                    {
                        isOpen = true;
                        Close();
                    }
                    break;
            }
        }
        public void Setup()
        {
            // panel setup
            if (panel)
            {
                // calculate width panel
                float newWidth = 0;
                float newHeight = 0;
                if (isPercent)
                {
                    newWidth = widthMove / 100.0f;
                    newHeight = newWidth;
                }
                else
                {
                    newWidth = widthMove / GetComponent<RectTransform>().rect.width;
                    newHeight = widthMove / GetComponent<RectTransform>().rect.height;
                }
                switch (side)
                {
                    case 0: // Left
                        panel.anchorMin = new Vector2(-newWidth, 0f);
                        panel.anchorMax = new Vector2(0f, 1f);
                        break;
                    case 1: // Top
                        panel.anchorMin = new Vector2(0f, 1f);
                        panel.anchorMax = new Vector2(1f , 1f + newHeight);
                        break;
                    case 2: // Right
                            panel.anchorMin = new Vector2(1f, 0f);
                            panel.anchorMax = new Vector2(1f + newWidth, 1f);
                        break;
                    case 3: // Bottom
                        panel.anchorMin = new Vector2(0f, -newHeight);
                        panel.anchorMax = new Vector2(1f, 0f);
                        break;
                }
            }

            // button setup
            if (isButton)
            {
                if (button) {
                    button.gameObject.SetActive(true);
                    button.anchorMin = buttonAnchorsPosition;
                    button.anchorMax = buttonAnchorsPosition;
                    button.anchoredPosition = buttonOffsetPosition;
                }

                if (swipe)
                    swipe.gameObject.SetActive(false);
            }
            else
            {
                if (swipe)
                {
                    swipe.gameObject.SetActive(true);
                    if (isFullScreenSwipe)
                    {
                        swipe.anchorMin = Vector2.zero;
                        swipe.anchorMax = Vector2.one;
                    }
                    else
                    {
                        switch (side)
                        {
                            case 0: // Left
                                swipe.anchorMin = Vector2.zero;
                                swipe.anchorMax = new Vector2(0.2f,1f);
                                break;
                            case 1: // Top
                                swipe.anchorMin = new Vector2(0f, 0.8f);
                                swipe.anchorMax = Vector2.one;
                                break;
                            case 2: // Right
                                swipe.anchorMin = new Vector2(0.8f,0f);
                                swipe.anchorMax = Vector2.one;
                                break;
                            case 3: // Bottom
                                swipe.anchorMin = Vector2.zero;
                                swipe.anchorMax = new Vector2(1f, 0.2f);
                                break;
                        }
                    }
                }

                if (button)
                    button.gameObject.SetActive(false);
            }

            // background
            background.raycastTarget = false;
            if (isBackgroundFade)
            {
                Color tempColor = background.color;
                tempColor.a = backgroundFade;
                background.color = tempColor;
            }
            else
            {
                Color tempColor = background.color;
                tempColor.a = 0;
                background.color = tempColor;
            }
        }


        private IEnumerator MoveTo(Vector3 toPosition, float duration)
        {
            //Make sure there is only one instance of this function running
            if (isMoving)
                yield break; ///exit if this is still running

            isMoving = true;
            float counter = 0;
            //set alpha color for background

            while (Vector2.Distance(panel.anchoredPosition, toPosition) > 5f)
            {
                counter += Time.deltaTime;
                // move panel
                panel.anchoredPosition = Vector3.Lerp(panel.anchoredPosition, toPosition, counter / duration);

                if (isButton && isSticky && button )
                {
                    button.anchoredPosition = Vector3.Lerp(button.anchoredPosition, (toPosition + (Vector3)buttonOffsetPosition), counter / duration);                    
                }
                if (!isButton && swipe)
                {
                    swipe.anchoredPosition = Vector3.Lerp(swipe.anchoredPosition, toPosition , counter / duration);
                }
                yield return null;
            }

            panel.anchoredPosition = toPosition;
            if (isButton && isSticky && button)
            {
                button.anchoredPosition = (toPosition + (Vector3)buttonOffsetPosition);
            }
            if (!isButton && swipe)
            {
                swipe.anchoredPosition = toPosition;
            }
            // callbacks
            if (isOpen)
                OnOpen();
            else
                OnClose();

            isMoving = false;
        }
        private void Init()
        {
            if (closeOnBackground)
            {
                //background.GetComponent<UIButton>().onClick.AddListener(Close);
                background.GetComponent<Button>().onClick.AddListener(Close);
            }

            if (button)
            {
                //button.GetComponent<UIButton>().actionsOnClick.isEvent = true;
                //button.GetComponent<UIButton>().actionsOnClick.unityEvent.AddListener(ButtonAction);
                button.GetComponent<Button>().onClick.AddListener(ButtonAction);
            }
        }
        private void Fade()
        {
            // fade background
            if (isBackgroundFade)
            {
                float alpha = 0;
                if (side == 0 || side == 2)// if is horizontal then use width
                    alpha = backgroundFade * (Vector2.Distance(Vector2.zero, panel.anchoredPosition) / panel.rect.width);
                else// if is vertical use then height
                    alpha = backgroundFade * (Vector2.Distance(Vector2.zero, panel.anchoredPosition) / panel.rect.height);

                Color tempColor = background.color;
                tempColor.a = alpha;
                background.color = tempColor;
            }
            if (isPanelFade)
            {
                float alpha = 0;
                if (side == 0 || side == 2)// if is horizontal then use width
                    alpha = 1f * (Vector2.Distance(Vector2.zero, panel.anchoredPosition) / panel.rect.width);
                else// if is vertical use then height
                    alpha = 1f * (Vector2.Distance(Vector2.zero, panel.anchoredPosition) / panel.rect.height);

                panel.GetComponent<CanvasGroup>().alpha = alpha;
            }

        }
    }
}
