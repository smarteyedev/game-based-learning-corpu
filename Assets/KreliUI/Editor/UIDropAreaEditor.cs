using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace KreliStudio
{
    [CustomEditor(typeof(UIDropArea), true)]
    [CanEditMultipleObjects]
    public class UIDropAreaEditor : SelectableEditor
    {


        SerializedProperty m_onChanged;


        // helper variable
        bool _openSelectable;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_onChanged = serializedObject.FindProperty("onChanged");
        }


        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UIDropAreaLabel");
            serializedObject.Update();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Drop Area Properties", UIStyle.LabelBold);
            GUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;
            _openSelectable = EditorGUILayout.Foldout(_openSelectable, new GUIContent("Selectable"), true, UIStyle.FoldoutBoldMini);
            if (_openSelectable)
            {
                base.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            GUILayout.EndVertical();


            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Drop Area Events", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_onChanged, new GUIContent("On Changed"));
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }




    }
}
