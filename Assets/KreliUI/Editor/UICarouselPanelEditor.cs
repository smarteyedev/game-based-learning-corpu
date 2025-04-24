using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


namespace KreliStudio
{
    [CustomEditor(typeof(UICarouselPanel), true)]
    public class UICarouselPanelEditor : Editor
    {

        //References
        SerializedProperty m_content;
        SerializedProperty m_previousButton;
        SerializedProperty m_nextButton;
        SerializedProperty m_iconBar;

        // Properties
        SerializedProperty m_isHorizontal;
        SerializedProperty m_isButtons;
        SerializedProperty m_isIconBar;
        SerializedProperty m_startingPage;
        SerializedProperty m_spaceOffset;
        SerializedProperty m_isOneSwipeMove;
        SerializedProperty m_isColor;
        SerializedProperty m_iconOn;
        SerializedProperty m_iconOff;
        SerializedProperty m_colorOn;
        SerializedProperty m_colorOff;


        //Effect
        SerializedProperty m_isEffect;
        SerializedProperty m_effectRange;
        SerializedProperty m_effectScale;

        //Events
        SerializedProperty m_onSelected;


        //helper car
        bool m_openReferences;

        protected virtual void OnEnable()
        {
            m_content = serializedObject.FindProperty("content");
            m_previousButton = serializedObject.FindProperty("previousButton");
            m_nextButton = serializedObject.FindProperty("nextButton");
            m_iconBar = serializedObject.FindProperty("iconBar");
            m_isHorizontal = serializedObject.FindProperty("isHorizontal");
            m_isButtons = serializedObject.FindProperty("isButtons");
            m_isIconBar = serializedObject.FindProperty("isIconBar");
            m_startingPage = serializedObject.FindProperty("startingPage");
            m_spaceOffset = serializedObject.FindProperty("spaceOffset");
            m_isOneSwipeMove = serializedObject.FindProperty("isOneSwipeMove");
            m_isEffect = serializedObject.FindProperty("isEffect");
            m_effectRange = serializedObject.FindProperty("effectRange");
            m_effectScale = serializedObject.FindProperty("effectScale");
            m_onSelected = serializedObject.FindProperty("onSelected");
            m_isColor = serializedObject.FindProperty("isColor");
            m_iconOn = serializedObject.FindProperty("iconOn");
            m_iconOff = serializedObject.FindProperty("iconOff");
            m_colorOn = serializedObject.FindProperty("colorOn");
            m_colorOff = serializedObject.FindProperty("colorOff");
        }

        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UICarouselPanelLabel");
            serializedObject.Update();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Carousel Panel Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            m_openReferences = EditorGUILayout.Foldout(m_openReferences, new GUIContent("References", "Place here references to objects."), true, UIStyle.FoldoutBoldMini);
            if (m_openReferences)
            {
                EditorGUILayout.PropertyField(m_content, new GUIContent("Content"));
                EditorGUILayout.PropertyField(m_iconBar, new GUIContent("Icon Bar"));
                if (m_isButtons.boolValue)
                {
                    EditorGUILayout.PropertyField(m_previousButton, new GUIContent("Previous Button"));
                    EditorGUILayout.PropertyField(m_nextButton, new GUIContent("Next Button"));
                }
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Content Panel", UIStyle.LabelBold);                       
            int isHorizontalInt = (m_isHorizontal.boolValue) ? 1 : 0;
            isHorizontalInt = EditorGUILayout.Popup(new GUIContent("Orientation", "Place carousel panel in a specific orientation."), isHorizontalInt, new string[] { "Vertical", "Horizontal" });
            m_isHorizontal.boolValue = (isHorizontalInt != 0) ? true : false;
            EditorGUILayout.PropertyField(m_spaceOffset, new GUIContent("Space Offset", "Space distance between carousels items."));
            if (m_spaceOffset.floatValue <= 0.01f)
            {
                m_spaceOffset.floatValue = 0.01f;
            }
            int isEffestInt = (m_isEffect.boolValue) ? 1 : 0;
            isEffestInt = EditorGUILayout.Popup(new GUIContent("Scale Effect", "Scale objects in the center of the carousel."), isEffestInt, new string[] { "Disable", "Enable" });
            m_isEffect.boolValue = (isEffestInt != 0) ? true : false;
            if (m_isEffect.boolValue)
            {
                EditorGUILayout.PropertyField(m_effectRange, new GUIContent("Effect Range", "Start scaling in the area."));
                EditorGUILayout.PropertyField(m_effectScale, new GUIContent("Effect Scale"));

            }
            int isIconBarInt = (m_isIconBar.boolValue) ? 1 : 0;
            isIconBarInt = EditorGUILayout.Popup(new GUIContent("Icon Bar", "Show the bar with elements icon."), isIconBarInt, new string[] { "Disable", "Enable" });
            m_isIconBar.boolValue = (isIconBarInt != 0) ? true : false;
            if (m_isIconBar.boolValue)
            {
                int isColorInt = (m_isColor.boolValue) ? 1 : 0;
                isColorInt = EditorGUILayout.Popup(new GUIContent("Animation Type"), isColorInt, new string[] { "Sprite", "Color" });
                m_isColor.boolValue = (isColorInt != 0) ? true : false;

                if (m_isColor.boolValue)
                {
                    EditorGUILayout.PropertyField(m_iconOn, new GUIContent("Sprite"));
                    EditorGUILayout.PropertyField(m_colorOff, new GUIContent("Deselected"));
                    EditorGUILayout.PropertyField(m_colorOn, new GUIContent("Selected"));
                }
                else
                {
                    EditorGUILayout.PropertyField(m_iconOff, new GUIContent("Deselected"));
                    EditorGUILayout.PropertyField(m_iconOn, new GUIContent("Selected"));
                }

            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Controls", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_startingPage, new GUIContent("Start Element", "Select this element at start."));
            int isButtonInt = (m_isButtons.boolValue) ? 1 : 0;
            isButtonInt = EditorGUILayout.Popup(new GUIContent("Button","Show buttons to control carousel (previous and next)."), isButtonInt, new string[] { "Disable", "Enable" });
            m_isButtons.boolValue = (isButtonInt != 0) ? true : false;

            int isFastSwipeInt = (m_isOneSwipeMove.boolValue) ? 1 : 0;
            isFastSwipeInt = EditorGUILayout.Popup(new GUIContent("Fast Swipe", "Fast swipe and jumps by one element or scroll with proportional swipe force."), isFastSwipeInt, new string[] { "Scrolling", "Move By One" });
            m_isOneSwipeMove.boolValue = (isFastSwipeInt != 0) ? true : false;
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Carousel Panel Events", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_onSelected, new GUIContent("On Selected"));

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}