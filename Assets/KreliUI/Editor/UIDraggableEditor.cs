using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace KreliStudio
{
    [CustomEditor(typeof(UIDraggable), true)]
    [CanEditMultipleObjects]
    public class UIDraggableEditor : SelectableEditor
    {

        SerializedProperty m_replaceable;
        SerializedProperty m_onDragged;
        SerializedProperty m_onDropped;
        SerializedProperty m_onChanged;


        // helper variable
        bool _openSelectable;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_replaceable = serializedObject.FindProperty("replaceable");
            m_onDragged = serializedObject.FindProperty("onDragged");
            m_onDropped = serializedObject.FindProperty("onDropped");
            m_onChanged = serializedObject.FindProperty("onChanged");
        }


        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UIDraggableLabel");
            serializedObject.Update();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Draggable Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            _openSelectable = EditorGUILayout.Foldout(_openSelectable, new GUIContent("Selectable"), true, UIStyle.FoldoutBoldMini);
            if (_openSelectable)
            {
                base.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();

            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Properties", UIStyle.LabelBoldMini);
                EditorGUILayout.PropertyField(m_replaceable, new GUIContent("Replaceable","Can be replaced with another UI Draggable elements."));
            GUILayout.EndVertical();
            GUILayout.EndVertical();


            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Draggable Events", UIStyle.LabelBold);

            EditorGUILayout.PropertyField(m_onDragged, new GUIContent("On Dragged"));
            EditorGUILayout.PropertyField(m_onDropped, new GUIContent("On Dropped"));
            EditorGUILayout.PropertyField(m_onChanged, new GUIContent("On Changed"));

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }




    }
}
