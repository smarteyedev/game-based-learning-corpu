using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
namespace KreliStudio
{
    [CustomEditor(typeof(UIScreen), true)]
    [CanEditMultipleObjects]
    public class UIScreenEditor : Editor
    {


        SerializedProperty m_Layer;
        SerializedProperty m_Archive;
        SerializedProperty m_StartSelectable;
        SerializedProperty m_isCustomPosition;
        SerializedProperty m_unityEvent_OnScreenOpen;
        SerializedProperty m_unityEvent_OnScreenClose;


        protected virtual void OnEnable()
        {
            m_Layer = serializedObject.FindProperty("layer");
            m_Archive = serializedObject.FindProperty("archive");
            m_StartSelectable = serializedObject.FindProperty("startSelectable");
            m_isCustomPosition = serializedObject.FindProperty("isCustomPosition");
            m_unityEvent_OnScreenOpen = serializedObject.FindProperty("onScreenOpen");
            m_unityEvent_OnScreenClose = serializedObject.FindProperty("onScreenClose");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UIEditor.DrawImage("UIScreenLabel");

            // UIScreen Properties
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Screen Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("helpBox");
            GUILayout.Label("Properties", UIStyle.LabelBoldMini);
            m_Layer.intValue = EditorGUILayout.Popup(new GUIContent("UIScreen Layer", "UI Screens with different layers may overlap."), m_Layer.intValue, new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            EditorGUILayout.PropertyField(m_StartSelectable, new GUIContent("Select on start", "Selected UI Element will be highlighted on UI Screen start."));
            EditorGUILayout.PropertyField(m_Archive, new GUIContent("Add To History", "If it is added to the History, you can go back to it by Escape or [Previous UIScreen]"));
            EditorGUILayout.PropertyField(m_isCustomPosition, new GUIContent("Custom Position", "When displaying UI Screen it always set in center of the canvas. This allows you to freely set position many UIScreen in the editor."));
            GUILayout.EndVertical();
            GUILayout.EndVertical();

            // UIScreen Events
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Screen Events", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_unityEvent_OnScreenOpen);
            EditorGUILayout.PropertyField(m_unityEvent_OnScreenClose);
            GUILayout.EndVertical();


            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

    }
}
