using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace KreliStudio {

    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(Mask))]
    [RequireComponent(typeof(Image))]
    public class UICarouselPanel : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [Serializable]
        public class SnapRectEvent : UnityEvent<int> { }

        //References
        public RectTransform content;
        public UIButton previousButton;
        public UIButton nextButton;
        public RectTransform iconBar;

        // Properties
        public bool isHorizontal = true;
        public bool isButtons = true;
        public bool isIconBar = true;
        public int startingPage = 0;
        public float spaceOffset = 1f;
        public bool isOneSwipeMove = false;
        public bool isColor;
        public Sprite iconOn;
        public Sprite iconOff;
        public Color colorOn;
        public Color colorOff;


        //Effect
        public bool isEffect = false;
        public float effectRange = 100f;
        public float effectScale = 1f;

        //Events
        public SnapRectEvent onSelected = new SnapRectEvent();


        private bool isInitialized = false;
        private float fastSwipeThresholdTime = 0.3f;
        private int fastSwipeThresholdDistance = 100;
        private float decelerationRate = 10f;
        private float _fastSwipeThresholdMaxLimit;
        private ScrollRect _scrollRectComponent;
        private RectTransform _scrollRectRect;
        // number of pages in container
        private int _pageCount;
        private int _currentPage;
        // whether lerping is in progress and target lerp position
        private bool _lerp;
        private Vector2 _lerpTo;
        // target position of every page
        private List<Vector2> _pagePositions = new List<Vector2>();
        // in draggging, when dragging started and where it started
        private bool _dragging;
        private float _timeStamp;
        private Vector2 _startPosition;
        private bool _canSnap;
        private List<Transform> _items = new List<Transform>();
        private List<Image> _icons = new List<Image>();



        void Start()
        {
            Init();
        }

        //------------------------------------------------------------------------
        public void Init()
        {
            _scrollRectComponent = GetComponent<ScrollRect>();
            _scrollRectRect = GetComponent<RectTransform>();
            _scrollRectComponent.content = content;
            _pageCount = content.childCount;

            // is it horizontal or vertical scrollrect
            _scrollRectComponent.horizontal = isHorizontal;
            _scrollRectComponent.vertical = !isHorizontal;

            _lerp = false;
            _canSnap = false;


            // init
            if (_pageCount == 0)
            {
                Debug.LogWarning("Not found any objects to show in Content Panel");
                return;
            }
            SetPagePositions();
            InitPageSelection();
            SetPage(startingPage);
            isInitialized = true;
            if (!isIconBar && iconBar != null)
                iconBar.gameObject.SetActive(false);

            // prev and next buttons
            if (isButtons)
            {
                if (nextButton)
                    nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });
                if (previousButton)
                    previousButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
            }
            else
            {
                if (nextButton)
                    nextButton.gameObject.SetActive(false);
                if (previousButton)
                    previousButton.gameObject.SetActive(false);
            }

            // on start something is selected so invoke event
            if (onSelected != null)
            {
                try
                {
                    onSelected.Invoke(startingPage);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onSelected. Error: " + exception.Message);
                }
            }
            

        }

        //------------------------------------------------------------------------
        void Update() {
            if (isInitialized)
            {
                // if moving to target position
                if (_lerp)
                {
                    // prevent overshooting with values greater than 1
                    float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
                    content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, _lerpTo, decelerate);
                    // time to stop lerping?
                    if (Vector2.SqrMagnitude(content.anchoredPosition - _lerpTo) < 0.25f)
                    {
                        // snap to target and stop lerping
                        content.anchoredPosition = _lerpTo;
                        _lerp = false;
                        // clear also any scrollrect move that may interfere with our lerping
                        _scrollRectComponent.velocity = Vector2.zero;
                    }
                }
                if (_canSnap)
                {
                    if (_scrollRectComponent.velocity.x < 250.0f && _scrollRectComponent.velocity.x > -250.0f)
                    {
                        LerpToPage(GetNearestPage());
                    }
                }
                if (isEffect)
                {
                    for (int i = 0; i < _items.Count; i++)
                    {
                        // distance from item to center 
                        float scale = Vector3.Distance(_items[i].position, transform.position);
                        // clamp and reverse distance between effectWidth
                        scale = effectRange - Mathf.Clamp(scale, 0.0f, effectRange);
                        // scale effect
                        scale = 1 + (scale / effectRange) * effectScale;
                        _items[i].localScale = new Vector3(scale, scale, scale);
                    }
                }


            }
        }

        //------------------------------------------------------------------------
        private void SetPagePositions() {

            float width = 0;
            float height = 0;
            float offsetX = 0;
            float offsetY = 0;
            float containerWidth = 0;
            float containerHeight = 0;

            if (isHorizontal) {
                // screen width in pixels of scrollrect window
                width = _scrollRectRect.rect.width / (2 / spaceOffset);
                // center position of all pages
                offsetX = width / 2;
                // total width
                containerWidth = width * _pageCount;
                // limit fast swipe length - beyond this length it is fast swipe no more
                _fastSwipeThresholdMaxLimit = width;
            } else {
                height = _scrollRectRect.rect.height / (2 / spaceOffset);
                offsetY = height / 2;
                containerHeight = height * _pageCount;
                _fastSwipeThresholdMaxLimit = height;
            }

            // set width of container
            Vector2 newSize = new Vector2(containerWidth, containerHeight);
            content.sizeDelta = newSize;
            Vector2 newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
            content.anchoredPosition = newPosition;

            // delete any previous settings
            _pagePositions.Clear();

            // iterate through all container childern and set their positions
            for (int i = 0; i < _pageCount; i++) {
                RectTransform child = content.GetChild(i).GetComponent<RectTransform>();
                Vector2 childPosition;
                if (isHorizontal) {
                    childPosition = new Vector2(i * width - containerWidth / 2 + offsetX, 0f);
                } else {
                    childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
                }
                child.anchoredPosition = childPosition;
                _pagePositions.Add(-childPosition);
            }
        }

        //------------------------------------------------------------------------
        private void SetPage(int aPageIndex)
        {

            aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
            content.anchoredPosition = _pagePositions[aPageIndex];
            _currentPage = aPageIndex;
            SetIcon();
        }

        //------------------------------------------------------------------------
        public void LerpToPage(int aPageIndex)
        {
            aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
            _lerpTo = _pagePositions[aPageIndex];
            _lerp = true;
            _currentPage = aPageIndex;
            SetIcon();

            if (onSelected != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UICarouselPanel", "On Selected", "Event invoke.");
                    onSelected.Invoke(aPageIndex);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onSelected. Error: " + exception.Message);
                }
            }
            //GameManager.Instance.guiManager.panelMainMenu.GetComponent<MainMenuGUI>().OnSelected(_currentPage);
        }

        //------------------------------------------------------------------------
        private void InitPageSelection() {
            // cache all Image components into list
            for (int i = 0; i < _scrollRectComponent.content.childCount; i++) {
                _items.Add(_scrollRectComponent.content.GetChild(i));
                if (isIconBar)
                {
                    GameObject obj = new GameObject("Icon_" + i);
                    obj.transform.SetParent(iconBar);
                    obj.AddComponent<Image>().sprite = iconOn;
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, 15f);
                    _icons.Add(obj.GetComponent<Image>());
                }
            }

        }

        private void SetIcon()
        {
            if (isIconBar && iconBar)
            {
                for (int i = 0; i < iconBar.childCount; i++)
                {
                    if (_currentPage == i)
                    {
                        if (isColor)
                            _icons[i].color = colorOn;
                        else
                            _icons[i].sprite = iconOn;
                    }
                    else
                    {
                        if (isColor)
                            _icons[i].color = colorOff;
                        else
                            _icons[i].sprite = iconOff;
                    }
                }
            }
        }

            //------------------------------------------------------------------------
        public void NextScreen()
        {
            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "NextScreen", "Go to page " + (_currentPage + 1) + ".");
            LerpToPage(_currentPage + 1);
        }

        //------------------------------------------------------------------------
        public void PreviousScreen()
        {
            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "PreviousScreen", "Go to page " + (_currentPage - 1) + ".");
            LerpToPage(_currentPage - 1);
        }

        //------------------------------------------------------------------------
        private int GetNearestPage() {
            // based on distance from current position, find nearest page
            Vector2 currentPosition = content.anchoredPosition;
            _canSnap = false;
            float distance = float.MaxValue;
            int nearestPage = _currentPage;

            for (int i = 0; i < _pagePositions.Count; i++) {
                float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
                if (testDist < distance) {
                    distance = testDist;
                    nearestPage = i;
                }
            }

            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "GetNearestPage", "Go to page " + nearestPage +".");
            return nearestPage;
        }

        //------------------------------------------------------------------------
        public void OnBeginDrag(PointerEventData aEventData)
        {
            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "OnBeginDrag", "Begin drag carousel panel.");
            // if currently lerping, then stop it as user is draging
            _lerp = false;
            // not dragging yet
            _dragging = false;
        }

        //------------------------------------------------------------------------
        public void OnEndDrag(PointerEventData aEventData) {
            if (isOneSwipeMove)
            {
                // how much was container's content dragged
                float difference;
                if (isHorizontal) {
                    difference = _startPosition.x - content.anchoredPosition.x;
                } else {
                    difference = -(_startPosition.y - content.anchoredPosition.y);
                }

                // test for fast swipe - swipe that moves only +/-1 item
                if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
                    Mathf.Abs(difference) > fastSwipeThresholdDistance &&
                    Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit)
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "OnEndDrag", "Fast swipe - move by once.");
                    if (difference > 0) {
                        NextScreen();
                    } else {
                        PreviousScreen();
                    }
                } else {
                    // if not fast time, look to which page we got to
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UICarouselPanel", "OnEndDrag", "Fast swipe - scroll carousel freely");
                    LerpToPage(GetNearestPage());

                }
            }
            _canSnap = true;
            _dragging = false;
        }

        //------------------------------------------------------------------------
        public void OnDrag(PointerEventData aEventData) {
            if (!_dragging) {
                // dragging started
                _dragging = true;
                // save time - unscaled so pausing with Time.scale should not affect it
                _timeStamp = Time.unscaledTime;
                // save current position of cointainer
                _startPosition = content.anchoredPosition;
            }
        }
    }
}
