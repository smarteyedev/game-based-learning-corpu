using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace KreliStudio
{
    public class UIEditor : Editor
    {

        private const string kUILayerName = "UI";
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDefaultAnimator = "KreliUI/Animation/BaseScreenController.controller";

        private static Vector2 _uiButtonSize = new Vector2(160, 30);
        private static Vector2 _uiElementSize = new Vector2(100f, 100f);

        private static Color _uiElementColor = new Color(1f, 1f, 1f, 1f);
        private static Color _uiSpecialColor = new Color(0f, 1f, 0.56f, 1f);
        private static Color _uiPanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color _textColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        






        // Create UI Manager
        [ MenuItem("GameObject/KreliStudio/UI/UI Manager", false, 10)]
        public static UIManager CreateManager()
        {
            GameObject uiElement;
            // if is on scene then select it
            if (FindObjectOfType<UIManager>() != null)
            {
                uiElement = FindObjectOfType<UIManager>().gameObject;
                Selection.SetActiveObjectWithContext(uiElement, null);
            }// else create new
            else
            {
                uiElement = new GameObject("UIManager");
                uiElement.AddComponent<UIManager>();
                uiElement.AddComponent<UISceneManager>();

                GameObject canvas = new GameObject("MasterCanvas");
                canvas.transform.SetParent(uiElement.transform);
                canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.AddComponent<CanvasScaler>();
                canvas.AddComponent<GraphicRaycaster>();
                canvas.layer = LayerMask.NameToLayer(kUILayerName);

                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.transform.SetParent(uiElement.transform);
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                GameObject screenGroup = new GameObject("UIScreensGroup");
                screenGroup.transform.SetParent(canvas.transform);
                SetFullScreen(screenGroup.AddComponent<RectTransform>());
                screenGroup.layer = LayerMask.NameToLayer(kUILayerName);

                GameObject loadingScreen = new GameObject("LoadingScreensGroup");
                loadingScreen.transform.SetParent(canvas.transform);
                SetFullScreen(loadingScreen.AddComponent<RectTransform>());
                loadingScreen.layer = LayerMask.NameToLayer(kUILayerName);

                Selection.SetActiveObjectWithContext(uiElement, null);
            }
            return uiElement.GetComponent<UIManager>();
        }
        /*// Create UI Scene Manager
        [MenuItem("GameObject/KreliStudio/UI Scene Manager", false, 11)]
        public static UISceneManager CreateSceneManager()
        {
            GameObject uiElement = new GameObject("UISceneManager");
            uiElement.AddComponent<UISceneManager>();
            GameObject canvas = new GameObject("Loading Screens Canvas");
            canvas.transform.SetParent(uiElement.transform);
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            canvas.layer = LayerMask.NameToLayer(kUILayerName);


            return uiElement.GetComponent<UISceneManager>();
        }*/
        // Create UI Screen
        [MenuItem("GameObject/KreliStudio/UI/UI Screen", false,51)]
        public static UIScreen CreateScreen()
        {        
            GameObject uiElement = new GameObject("UIScreen");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UIScreen>();
            uiElement.GetComponent<Animator>().runtimeAnimatorController = LoadBaseAnimatorAvatar();
            SetFullScreen(uiElement.GetComponent<RectTransform>());

            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(uiElement.transform);
            SetFullScreen(panel.AddComponent<RectTransform>());
            SetImage(panel.AddComponent<Image>(), kBackgroundSpriteResourcePath);
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);
            panel.layer = LayerMask.NameToLayer(kUILayerName);
            

            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UIScreen>();
        }

        // Create UI Button
        [MenuItem("GameObject/KreliStudio/UI/UI Button", false, 52)]
        public static UIButton CreateButton()
        {
            GameObject uiElement = new GameObject("UIButton");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UIButton>();
            SetSize(uiElement.GetComponent<RectTransform>(),_uiButtonSize);
            SetImage(uiElement.GetComponent<Image>(), kStandardSpritePath);
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject text = new GameObject("Text");
            text.transform.SetParent(uiElement.transform);
            text.AddComponent<Text>();
            text.GetComponent<Text>().text = "UI Button";
            text.GetComponent<Text>().color = _textColor;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            SetFullScreen(text.GetComponent<RectTransform>());
            text.layer = LayerMask.NameToLayer(kUILayerName);


            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UIButton>();
        }

        // Create UI Dragable
        [MenuItem("GameObject/KreliStudio/UI/UI Dragable", false, 53)]
        public static UIDraggable CreateDragable()
        {            
            GameObject uiElement = new GameObject("UIDragable");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UIDraggable>();
            SetImage(uiElement.AddComponent<Image>(), kKnobPath);
            SetSize(uiElement.GetComponent<RectTransform>(), _uiElementSize);
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);

            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UIDraggable>();
        }
        // Create UI Drop Area
        [MenuItem("GameObject/KreliStudio/UI/UI Drop Area", false, 54)]
        public static UIDropArea CreateDropArea()
        {
            GameObject uiElement = new GameObject("UIDropArea");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UIDropArea>();
            SetImage(uiElement.AddComponent<Image>(), kBackgroundSpriteResourcePath);
            SetSize(uiElement.GetComponent<RectTransform>(), _uiElementSize);
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);

            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UIDropArea>();
        }
        // Create Radial Slider
        [MenuItem("GameObject/KreliStudio/UI/UI Radial Slider", false, 55)]
        public static UIRadialSlider CreateRadialSlider()
        {
            GameObject uiElement = new GameObject("UIRadialSlider");
            uiElement.transform.SetParent(GetCanvasParent());
            SetSize(uiElement.AddComponent<RectTransform>(), _uiElementSize);
            uiElement.AddComponent<UIRadialSlider>();
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiElement.transform);
            background.AddComponent<Image>();
            SetFullScreen(background.GetComponent<RectTransform>());
            SetImage(background.GetComponent<Image>(), kKnobPath);
            background.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(uiElement.transform);
            fillArea.AddComponent<Image>();
            SetFullScreen(fillArea.GetComponent<RectTransform>());
            SetImage(fillArea.GetComponent<Image>(), kKnobPath);
            fillArea.GetComponent<Image>().color = _uiSpecialColor;
            fillArea.GetComponent<Image>().type = Image.Type.Filled;
            fillArea.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
            fillArea.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject foreground = new GameObject("Foreground");
            foreground.transform.SetParent(uiElement.transform);
            foreground.AddComponent<Image>();
            SetFullScreen(foreground.GetComponent<RectTransform>(), 0.9f);
            SetImage(foreground.GetComponent<Image>(), kKnobPath);
            foreground.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject text = new GameObject("Text");
            text.transform.SetParent(foreground.transform);
            text.AddComponent<Text>();
            text.GetComponent<Text>().text = "UI Radial Slider";
            text.GetComponent<Text>().color = _textColor;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            SetFullScreen(text.GetComponent<RectTransform>());
            text.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject handleArea = new GameObject("Handle Slide Area");
            handleArea.transform.SetParent(uiElement.transform);
            SetFullScreen(handleArea.AddComponent<RectTransform>());
            handleArea.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform);
            handle.AddComponent<Image>();
            SetSize(handle.GetComponent<RectTransform>(), new Vector2(30,30));
            SetImage(handle.GetComponent<Image>(), kKnobPath);
            handle.layer = LayerMask.NameToLayer(kUILayerName);


            uiElement.GetComponent<UIRadialSlider>().handle = handle.GetComponent<RectTransform>();
            uiElement.GetComponent<UIRadialSlider>().targetGraphic = handle.GetComponent<Image>();
            uiElement.GetComponent<UIRadialSlider>().fillText = text.GetComponent<Text>();
            uiElement.GetComponent<UIRadialSlider>().fillArea = fillArea.GetComponent<Image>();
            uiElement.GetComponent<UIRadialSlider>().handleOffset = -5f;
            uiElement.GetComponent<UIRadialSlider>().textFormat = 3;
            uiElement.GetComponent<UIRadialSlider>().Clockwise = true;
            uiElement.GetComponent<UIRadialSlider>().Value = .5f;
            uiElement.GetComponent<UIRadialSlider>().Setup();

            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UIRadialSlider>();
        }
        // Create UI Side Panel
        [MenuItem("GameObject/KreliStudio/UI/UI Side Panel", false, 56)]
        public static UISidePanel CreateSidePanel()
        {
            GameObject uiElement = new GameObject("UISidePanel");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UISidePanel>();
            SetFullScreen(uiElement.GetComponent<RectTransform>());
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiElement.transform);
            background.AddComponent<Image>();
            background.AddComponent<UIButton>();
            background.GetComponent<Image>().color = _uiPanelColor;
            SetFullScreen(background.GetComponent<RectTransform>());
            background.layer = LayerMask.NameToLayer(kUILayerName);

            GameObject content = new GameObject("Content");
            content.transform.SetParent(uiElement.transform);
            content.AddComponent<Image>();
            content.AddComponent<CanvasGroup>();
            SetImage(content.GetComponent<Image>(), kBackgroundSpriteResourcePath);
            SetFullScreen(content.GetComponent<RectTransform>());
            content.layer = LayerMask.NameToLayer(kUILayerName);


            GameObject swipe = new GameObject("Swipe");
            swipe.transform.SetParent(uiElement.transform);
            swipe.AddComponent<Image>();
            swipe.GetComponent<Image>().color = Color.clear;
            swipe.AddComponent<SwipeBar>().sidePanel = uiElement.GetComponent<UISidePanel>();
            SetFullScreen(swipe.GetComponent<RectTransform>());
            swipe.layer = LayerMask.NameToLayer(kUILayerName);


            UIButton uiButton = CreateButton();
            uiButton.transform.SetParent(uiElement.transform);

            uiElement.GetComponent<UISidePanel>().background = background.GetComponent<Image>();
            uiElement.GetComponent<UISidePanel>().panel = content.GetComponent<RectTransform>();
            uiElement.GetComponent<UISidePanel>().swipe = swipe.GetComponent<RectTransform>();
            uiElement.GetComponent<UISidePanel>().button = uiButton.GetComponent<RectTransform>();
            uiElement.GetComponent<UISidePanel>().isPercent = true;
            uiElement.GetComponent<UISidePanel>().widthMove = 40.0f;
            uiElement.GetComponent<UISidePanel>().isBackgroundFade = true;
            uiElement.GetComponent<UISidePanel>().backgroundFade = 0.5f;
            uiElement.GetComponent<UISidePanel>().closeOnBackground = true;
            uiElement.GetComponent<UISidePanel>().isButton = true;
            uiElement.GetComponent<UISidePanel>().isSticky = true;
            uiElement.GetComponent<UISidePanel>().buttonAnchorsPosition = new Vector2(0.0f, 0.5f);
            uiElement.GetComponent<UISidePanel>().buttonOffsetPosition = new Vector2(80.0f, 0.0f);
            uiElement.GetComponent<UISidePanel>().Setup();

            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UISidePanel>();
        }

        // Create UI Side Panel
        [MenuItem("GameObject/KreliStudio/UI/UI Carousel Panel", false, 57)]
        public static UICarouselPanel CreateCarouselPanel()
        {
            GameObject uiElement = new GameObject("UICarouselPanel");
            uiElement.transform.SetParent(GetCanvasParent());
            uiElement.AddComponent<UICarouselPanel>();
            SetFullScreen(uiElement.GetComponent<RectTransform>());
            uiElement.GetComponent<RectTransform>().sizeDelta = new Vector2(-200, -200);
            SetImage(uiElement.GetComponent<Image>(), kBackgroundSpriteResourcePath);
            uiElement.GetComponent<Image>().color = _uiPanelColor;
            uiElement.layer = LayerMask.NameToLayer(kUILayerName);
            
            GameObject content = new GameObject("Content");
            content.transform.SetParent(uiElement.transform);
            content.AddComponent<Image>();
            content.GetComponent<Image>().color = Color.clear;
            SetFullScreen(content.GetComponent<RectTransform>());

            GameObject iconBar = new GameObject("IconBar");
            iconBar.transform.SetParent(uiElement.transform);
            iconBar.AddComponent<Image>();
            iconBar.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            iconBar.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            iconBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 30f);
            iconBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 15f);
            iconBar.AddComponent<HorizontalLayoutGroup>();
            iconBar.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 10, 10, 10);
            iconBar.GetComponent<HorizontalLayoutGroup>().spacing = 5;
            iconBar.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;



            UIButton uiButtonPrevious = CreateButton();
            uiButtonPrevious.transform.SetParent(uiElement.transform);
            uiButtonPrevious.name = "UIButtonPrevious";
            uiButtonPrevious.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
            uiButtonPrevious.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
            uiButtonPrevious.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 60f);
            uiButtonPrevious.GetComponent<RectTransform>().anchoredPosition = new Vector2(30f, 0f);
            uiButtonPrevious.transform.GetComponentInChildren<Text>().text = "Prev";

            UIButton uiButtonNext = CreateButton();
            uiButtonNext.transform.SetParent(uiElement.transform);
            uiButtonNext.name = "UIButtonNext";
            uiButtonNext.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
            uiButtonNext.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
            uiButtonNext.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 60f);
            uiButtonNext.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30f, 0f);
            uiButtonNext.transform.GetComponentInChildren<Text>().text = "Next";

            uiElement.GetComponent<UICarouselPanel>().content = content.GetComponent<RectTransform>();
            uiElement.GetComponent<UICarouselPanel>().iconBar = iconBar.GetComponent<RectTransform>();
            uiElement.GetComponent<UICarouselPanel>().nextButton = uiButtonNext;
            uiElement.GetComponent<UICarouselPanel>().previousButton = uiButtonPrevious;


            Selection.SetActiveObjectWithContext(uiElement, null);
            return uiElement.GetComponent<UICarouselPanel>();
        }




        private static void SetFullScreen(RectTransform rectTransform,float scale=1.0f)
        {
            rectTransform.anchorMin = new Vector2(1 - scale, 1 - scale);
            rectTransform.anchorMax = new Vector2(scale, scale);
            rectTransform.pivot = Vector2.one*0.5f;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
        }

        private static void SetSize(RectTransform rectTransform, Vector2 size)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
        }
        
        private static void SetImage(Image img,string path)
        {
            img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(path);
            img.color = _uiElementColor;
            img.type = Image.Type.Sliced;
        }
        private static Transform GetCanvasParent()
        {
            Transform parentCanvas = null;
            if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponentInParent<Canvas>() == null)
            {
                if (FindObjectOfType<UIManager>() != null)
                {
                    if (FindObjectOfType<UIManager>().GetComponentInChildren<Canvas>() != null)
                        parentCanvas = FindObjectOfType<UIManager>().GetComponentInChildren<Canvas>().transform;
                    else
                        Debug.LogWarning("Can not find Master Canvas in UI Manager.");
                }
                else
                {
                    CreateManager();
                    return GetCanvasParent();
                }
            }
            else
            {
                parentCanvas = Selection.activeTransform;
            }
            return parentCanvas;
        }



        // Custom Editor helper metod
        public static Texture2D LoadPNG(string fileName)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/KreliUI/Images/" + fileName + ".png");
        }
        public static RuntimeAnimatorController LoadBaseAnimatorAvatar()
        {
            return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/KreliUI/Animation/BaseScreenController.controller");
        }
        public static void DrawImage(string fileName)
        {
            Rect logoPosition = GUILayoutUtility.GetRect(128, 512, 32, 64);
            Texture2D logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/KreliUI/Images/" + fileName + ".png");
            GUI.DrawTexture(logoPosition, logo, ScaleMode.ScaleToFit);
        }

        public static string[] GetScenes()
        {
            List<string> temp = new List<string>();
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.Length - 6);
                    temp.Add(name + " [ID " + temp.Count + "]");
                }
            }
            return temp.ToArray();
        }
        public static UIScreen[] GetUIScreens()
        {
            List<UIScreen> temp = new List<UIScreen>();
            temp.Add(null); // previous uiScreen
            foreach (UIScreen s in Resources.FindObjectsOfTypeAll(typeof(UIScreen)))
            {
                temp.Add(s);
            }
            return temp.ToArray();
        }
        public static string[] GetUIScreensNames(UIScreen[] uiScreens)
        {
            List<string> temp = new List<string>();
            foreach (UIScreen s in uiScreens)
            {
                if (s != null)
                    temp.Add(s.name +" [ID " +s.GetInstanceID() +"]");
                else
                    temp.Add("[Prevous UIScreen]");
            }
            return temp.ToArray();
        }
        

    }
}