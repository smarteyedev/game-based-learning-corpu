using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KreliStudio
{
    [CustomEditor(typeof(UIManager), true)]
    public class UIManagerEditor : Editor
    {

        SerializedProperty m_startScreen;

        SerializedProperty m_openTab;
        SerializedProperty m_screens;
        SerializedProperty m_currentScreens;
        SerializedProperty m_historyStack; 
        SerializedProperty m_currentLayer;
        SerializedProperty m_debugUIElements;
        SerializedProperty m_debugEvents;
        SerializedProperty m_debugUIScreen;
        SerializedProperty m_debugUIScene;
        SerializedProperty m_onSwitchedScreen;


        bool _openUIScreen = false;
        bool _openLayers = false;
        bool _openHistory = false;

        protected virtual void OnEnable()
        {
            m_startScreen = serializedObject.FindProperty("startScreen");
            m_openTab = serializedObject.FindProperty("openTab");
            m_screens = serializedObject.FindProperty("_screens");
            m_currentScreens = serializedObject.FindProperty("_currentScreens");
            m_historyStack = serializedObject.FindProperty("_historyStack");
            m_currentLayer = serializedObject.FindProperty("_currentLayer");
            m_debugUIElements = serializedObject.FindProperty("debugUIElements");
            m_debugEvents = serializedObject.FindProperty("debugEvents");
            m_debugUIScreen = serializedObject.FindProperty("debugUIScreen");
            m_debugUIScene = serializedObject.FindProperty("debugUIScene");
            m_onSwitchedScreen = serializedObject.FindProperty("onSwitchedScreen");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UIEditor.DrawImage("UIManagerLabel");

            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Manager Properties", UIStyle.LabelBold);
            m_openTab.intValue = GUILayout.Toolbar(m_openTab.intValue, new string[] { "Debugger", "UI Screens" });
            switch (m_openTab.intValue)
            {
                case 0:
                    GUILayout.BeginVertical("HelpBox");
                    GUILayout.Label("Debugger", UIStyle.LabelBoldMini);
                    EditorGUILayout.PropertyField(m_debugUIElements, new GUIContent("Debug UI Elements","Show debug logs in console from all UI elements."));
                    EditorGUILayout.PropertyField(m_debugEvents, new GUIContent("Debug Events", "Show debug logs in console from events."));
                    EditorGUILayout.PropertyField(m_debugUIScreen, new GUIContent("Debug UI Screen", "Show debug logs in console from all UI Screen."));
                    EditorGUILayout.PropertyField(m_debugUIScene, new GUIContent("Debug UI Scene", "Show debug logs in console from all UI Scene."));
                    GUILayout.EndVertical();
                    break;
                case 1:
                    /*GUILayout.BeginVertical("HelpBox");
                    GUILayout.Label("UI Screens", UIStyle.LabelBoldMini);
                    EditorGUILayout.PropertyField(m_startScreen, new GUIContent("Start UI Screen"));

                    GUILayout.EndVertical();*/
                    GUILayout.BeginVertical("HelpBox");
                    GUI.enabled = true;
                    GUILayout.Label("Preview (In Game Mode)", UIStyle.LabelBoldMini);
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(m_currentLayer, new GUIContent("Current Layer", "The layer on which the currently active UI Screen is."));
                    GUI.enabled = true;
                    EditorGUI.indentLevel++;
                    _openUIScreen = EditorGUILayout.Foldout(_openUIScreen, new GUIContent("UI Screens", "All UI Screens from UI Manager."), true);
                    GUI.enabled = false;
                    if (_openUIScreen)
                    {
                        int size = m_screens.FindPropertyRelative("Array.size").intValue;
                        for (int i = 0; i < size; i++)
                        {
                            EditorGUILayout.PropertyField(m_screens.GetArrayElementAtIndex(i), new GUIContent("UI Screen"));
                        }
                    }
                    GUI.enabled = true;
                    _openLayers = EditorGUILayout.Foldout(_openLayers, new GUIContent("Layers","All layers with open UI Screen on them."), true);
                    GUI.enabled = false;
                    if (_openLayers)
                    {
                        int size = m_currentScreens.FindPropertyRelative("Array.size").intValue;
                        for (int i = 0; i < size; i++)
                        {
                            EditorGUILayout.PropertyField(m_currentScreens.GetArrayElementAtIndex(i), new GUIContent("Layer " + i));
                        }
                    }
                    GUI.enabled = true;
                    _openHistory = EditorGUILayout.Foldout(_openHistory, new GUIContent("History", "History Stack with UIScreen (use escape button or [Previous UIScreen] to use history stack)."), true);
                    GUI.enabled = false;
                    if (_openHistory)
                    {
                        int size = m_historyStack.FindPropertyRelative("Array.size").intValue;
                        for (int i = 0; i < size; i++)
                        {
                            EditorGUILayout.PropertyField(m_historyStack.GetArrayElementAtIndex(i), new GUIContent("History " + i));
                        }
                    }
                    EditorGUI.indentLevel--;
                    GUILayout.EndVertical();
                    GUI.enabled = true;
                    break;
            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Manager Events", UIStyle.LabelBold);
            EditorGUILayout.PropertyField(m_onSwitchedScreen);
            GUILayout.EndVertical();

            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

    }

}

