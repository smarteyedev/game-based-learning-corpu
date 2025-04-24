using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace KreliStudio
{
    [CustomEditor(typeof(UIRadialSlider), true)]
    public class UIRadialSliderEditor : SelectableEditor
    {
        UIRadialSlider targetScript;
                
        SerializedProperty m_fillArea;
        SerializedProperty m_fillText;
        SerializedProperty m_handle;
        SerializedProperty m_minValue;
        SerializedProperty m_maxValue;
        SerializedProperty m_value;
        SerializedProperty m_wholeNumbers;
        SerializedProperty m_textFormat;
        SerializedProperty m_handleOffset;
        SerializedProperty m_fillOrigin;
        SerializedProperty m_rotateHandle;
        SerializedProperty m_clockwise;
        SerializedProperty m_onValueChanged;



        // helper variable
        bool _openSelectable;
        bool _openReferences;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetScript = (UIRadialSlider)target;

            m_fillArea = serializedObject.FindProperty("fillArea");
            m_fillText = serializedObject.FindProperty("fillText");
            m_handle = serializedObject.FindProperty("handle");
            m_minValue = serializedObject.FindProperty("minValue");
            m_maxValue = serializedObject.FindProperty("maxValue");
            m_value = serializedObject.FindProperty("value");
            m_wholeNumbers = serializedObject.FindProperty("wholeNumbers");
            m_textFormat = serializedObject.FindProperty("textFormat");
            m_handleOffset = serializedObject.FindProperty("handleOffset");
            m_fillOrigin = serializedObject.FindProperty("fillOrigin");
            m_rotateHandle = serializedObject.FindProperty("rotateHandle");
            m_clockwise = serializedObject.FindProperty("clockwise");
            m_onValueChanged = serializedObject.FindProperty("onValueChanged");
        }


        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UIRadialSliderLabel");
            serializedObject.Update();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Radial Slider Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            _openSelectable = EditorGUILayout.Foldout(_openSelectable, new GUIContent("Selectable"), true,UIStyle.FoldoutBoldMini);
            if (_openSelectable)
            {
                base.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            _openReferences = EditorGUILayout.Foldout(_openReferences, new GUIContent("References", "Place here references to objects."), true, UIStyle.FoldoutBoldMini);
            if (_openReferences)
            {
                EditorGUILayout.PropertyField(m_fillArea, new GUIContent("Fill Area"));
                EditorGUILayout.PropertyField(m_fillText, new GUIContent("Text Area"));
                EditorGUILayout.PropertyField(m_handle, new GUIContent("Handle"));
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Properties", UIStyle.LabelBoldMini);
            EditorGUILayout.PropertyField(m_wholeNumbers, new GUIContent("Whole Numbers", " Round values to integer."));            
            EditorGUILayout.PropertyField(m_minValue, new GUIContent("Min Value"));
            if (m_maxValue.floatValue < m_minValue.floatValue)
            {
                m_minValue.floatValue = m_maxValue.floatValue;
            }
            EditorGUILayout.PropertyField(m_maxValue, new GUIContent("Max Value"));
            if (m_maxValue.floatValue < m_minValue.floatValue)
            {
                m_maxValue.floatValue = m_minValue.floatValue;
            }
            EditorGUILayout.Slider(m_value, m_minValue.floatValue, m_maxValue.floatValue,new GUIContent("Value"));
            if (m_wholeNumbers.boolValue)
            {
                m_minValue.floatValue = Mathf.Round(m_minValue.floatValue);
                m_maxValue.floatValue = Mathf.Round(m_maxValue.floatValue);
                m_value.floatValue = Mathf.Round(m_value.floatValue);
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Slider Format", UIStyle.LabelBoldMini);
            m_textFormat.intValue = EditorGUILayout.Popup("Text Format", m_textFormat.intValue, new string[] { "none", "value", "value | max value", "percent" });
            m_fillOrigin.intValue = EditorGUILayout.Popup("Fill Origin", m_fillOrigin.intValue, new string[] { "Bottom", "Right", "Top", "Left" });
            
            EditorGUILayout.PropertyField(m_handleOffset, new GUIContent("Handle Offset", "Distance of handle from center."));
            EditorGUILayout.PropertyField(m_rotateHandle, new GUIContent("Handle Rotation", "Rotate handle when move around."));
            EditorGUILayout.PropertyField(m_clockwise, new GUIContent("Clockwise"));

            GUILayout.EndVertical();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Radial Slider Events", UIStyle.LabelBold);

            EditorGUILayout.PropertyField(m_onValueChanged, new GUIContent("On Value Changed"));

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            ///////////////
            if (EditorApplication.isPlaying)
                return;
            else
                targetScript.Setup();
        }
    }
}
