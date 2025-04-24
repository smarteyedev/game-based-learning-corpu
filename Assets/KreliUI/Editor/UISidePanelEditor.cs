using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace KreliStudio
{
    [CustomEditor(typeof(UISidePanel), true)]
    public class UISidePanelEditor : Editor
    {
        UISidePanel targetScript;

        SerializedProperty m_panel;
        SerializedProperty m_background;
        SerializedProperty m_button;
        SerializedProperty m_swipe;
        SerializedProperty m_side;
        SerializedProperty m_isPanelFade;
        SerializedProperty m_oldSide;
        SerializedProperty m_isPercent;
        SerializedProperty m_widthMove;
        SerializedProperty m_speed;
        SerializedProperty m_isBackgroundFade;
        SerializedProperty m_closeOnBackground;
        SerializedProperty m_backgroundFade;
        SerializedProperty m_isButton;
        SerializedProperty m_isSticky;
        SerializedProperty m_isElastic;
        SerializedProperty m_buttonAnchorsPosition;
        SerializedProperty m_buttonOffsetPosition;
        SerializedProperty m_isFullScreenSwipe;
        SerializedProperty m_onOpen;
        SerializedProperty m_onClose;

        // helper variable
        bool _openReferences;
        protected virtual void OnEnable()
        {
            targetScript = (UISidePanel)target;

            m_panel = serializedObject.FindProperty("panel");
            m_background = serializedObject.FindProperty("background");
            m_button = serializedObject.FindProperty("button");
            m_swipe = serializedObject.FindProperty("swipe");
            m_side = serializedObject.FindProperty("side");
            m_isPanelFade = serializedObject.FindProperty("isPanelFade");
            m_oldSide = serializedObject.FindProperty("oldSide");
            m_isPercent = serializedObject.FindProperty("isPercent");
            m_widthMove = serializedObject.FindProperty("widthMove");
            m_speed = serializedObject.FindProperty("speed");
            m_isBackgroundFade = serializedObject.FindProperty("isBackgroundFade");
            m_closeOnBackground = serializedObject.FindProperty("closeOnBackground");
            m_backgroundFade = serializedObject.FindProperty("backgroundFade");
            m_isButton = serializedObject.FindProperty("isButton");
            m_isSticky = serializedObject.FindProperty("isSticky");
            m_isElastic = serializedObject.FindProperty("isElastic");
            m_buttonAnchorsPosition = serializedObject.FindProperty("buttonAnchorsPosition");
            m_buttonOffsetPosition = serializedObject.FindProperty("buttonOffsetPosition");
            m_isFullScreenSwipe = serializedObject.FindProperty("isFullScreenSwipe");
            m_onOpen = serializedObject.FindProperty("onOpen");
            m_onClose = serializedObject.FindProperty("onClose");
        }


        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UISidePanelLabel");
            serializedObject.Update();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Side Panel Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            _openReferences = EditorGUILayout.Foldout(_openReferences, new GUIContent("References", "Place here references to objects."),true, UIStyle.FoldoutBoldMini);
            if (_openReferences)
            {
                EditorGUILayout.PropertyField(m_panel, new GUIContent("Panel"));
                EditorGUILayout.PropertyField(m_background, new GUIContent("Background"));
                if (m_isButton.boolValue)
                {
                    EditorGUILayout.PropertyField(m_button, new GUIContent("Button"));
                }
                else
                {
                    EditorGUILayout.PropertyField(m_swipe, new GUIContent("Swipe"));
                }
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Content Panel", UIStyle.LabelBoldMini);
            int isPanelFadeInt = (m_isPanelFade.boolValue) ? 1 : 0;
            isPanelFadeInt = EditorGUILayout.Popup(new GUIContent("Panel","Always visible or fade animation when open and close."), isPanelFadeInt, new string[] { "Normal", "Fade" });
            m_isPanelFade.boolValue = (isPanelFadeInt != 0) ? true : false;

            m_oldSide.intValue = m_side.intValue;
            m_side.intValue = EditorGUILayout.Popup("Side", m_side.intValue, new string[] { "Left", "Top", "Right", "Bottom" });
            if (m_side.intValue != m_oldSide.intValue && m_isButton.boolValue && m_button.GetValue<RectTransform>())
            {
                switch (m_side.intValue)
                {
                    case 0: // Left
                        m_buttonAnchorsPosition.vector2Value = new Vector2(0f, 0.5f);
                        m_buttonOffsetPosition.vector2Value = new Vector2(m_button.GetValue<RectTransform>().rect.width / 2f, 0f);
                        break;
                    case 1: // Top
                        m_buttonAnchorsPosition.vector2Value = new Vector2(0.5f, 1f);
                        m_buttonOffsetPosition.vector2Value = new Vector2(0f, -m_button.GetValue<RectTransform>().rect.height / 2f);
                        break;
                    case 2: // Right
                        m_buttonAnchorsPosition.vector2Value = new Vector2(1f, 0.5f);
                        m_buttonOffsetPosition.vector2Value = new Vector2(-m_button.GetValue<RectTransform>().rect.width / 2f, 0f);
                        break;
                    case 3: // Bottom
                        m_buttonAnchorsPosition.vector2Value = new Vector2(0.5f, 0f);
                        m_buttonOffsetPosition.vector2Value = new Vector2(0f, m_button.GetValue<RectTransform>().rect.height / 2f);
                        break;
                }
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_widthMove, new GUIContent("Width"));
            int isPercentInt = (m_isPercent.boolValue) ? 1 : 0;
            isPercentInt = EditorGUILayout.Popup(isPercentInt, new string[] { "Pixel", "Percent" });
            m_isPercent.boolValue = (isPercentInt != 0) ? true : false;
            if (m_isPercent.boolValue)
            {
                m_widthMove.floatValue = Mathf.Clamp(m_widthMove.floatValue, 0f, 100f);
            }else
            if (m_widthMove.floatValue < 0)
            {
                m_widthMove.floatValue = 0;
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(m_speed, new GUIContent("Movement Speed","Speed of panel open and close animations."));
            m_speed.floatValue = Mathf.Clamp(m_speed.floatValue, 0.1f, 10f);    
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Background", UIStyle.LabelBoldMini);
            int isBackgroundFadeInt = (m_isBackgroundFade.boolValue) ? 1 : 0;
            isBackgroundFadeInt = EditorGUILayout.Popup(new GUIContent("Background","Set background to invisible or smooth fade."),isBackgroundFadeInt, new string[] { "None", "Fade" });
            m_isBackgroundFade.boolValue = (isBackgroundFadeInt != 0) ? true : false;
            EditorGUILayout.PropertyField(m_closeOnBackground, new GUIContent("Close On Click","Close side panel when clicked background (works with None and Fade background too)."));
            if (m_isBackgroundFade.boolValue)
            {
                EditorGUILayout.PropertyField(m_backgroundFade, new GUIContent("Fade Alpha"));
                m_backgroundFade.floatValue = Mathf.Clamp01(m_backgroundFade.floatValue);
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Controls", UIStyle.LabelBoldMini);
            int isButtonInt = (m_isButton.boolValue) ? 1 : 0;
            isButtonInt = EditorGUILayout.Popup(new GUIContent("Control type","Button or swipe to control open and close side panel."),isButtonInt, new string[] { "Swipe", "Button" });
            m_isButton.boolValue = (isButtonInt != 0) ? true : false;
            if (m_isButton.boolValue)
            {
                int isStickyInt = (m_isSticky.boolValue) ? 1 : 0;
                isStickyInt = EditorGUILayout.Popup(new GUIContent("Sticky Button", "Attach button to panel and move button with panel."), isStickyInt, new string[] { "None", "Stick To Panel" });
                m_isSticky.boolValue = (isStickyInt != 0) ? true : false;
                EditorGUILayout.PropertyField(m_buttonAnchorsPosition, new GUIContent("Anchors Position","Set anchors position of button."));
                EditorGUILayout.PropertyField(m_buttonOffsetPosition, new GUIContent("Offset Position","Set position offset from anchors to button."));
            }
            else
            {
                int isElasticInt = (m_isElastic.boolValue) ? 1 : 0;
                isElasticInt = EditorGUILayout.Popup(new GUIContent("Drag Type","Simillar like Scroll Rect can be elastic or clamped."), isElasticInt, new string[] { "Clamped", "Elastic" });
                m_isElastic.boolValue = (isElasticInt != 0) ? true : false;

                int isFullScreenSwipeInt = (m_isFullScreenSwipe.boolValue) ? 1 : 0;
                isFullScreenSwipeInt = EditorGUILayout.Popup(new GUIContent("Drag Area","Set dragable area to full screen or small area close to side panel."), isFullScreenSwipeInt, new string[] { "Side", "Full Screen" });
                m_isFullScreenSwipe.boolValue = (isFullScreenSwipeInt != 0) ? true : false;
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();


            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Side Panel Events", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_onOpen, new GUIContent("On Open"));
            EditorGUILayout.PropertyField(m_onClose, new GUIContent("On Close"));

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
                return;
            else
                targetScript.Setup();
        }

    }
}