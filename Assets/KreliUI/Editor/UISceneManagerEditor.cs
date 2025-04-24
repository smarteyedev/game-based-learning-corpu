using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KreliStudio
{
    [CustomEditor(typeof(UISceneManager), true)]
    public class UISceneManagerEditor : Editor
    {
        UISceneManager targetScript;

        SerializedProperty m_startScene;
        SerializedProperty m_previousScene;
        SerializedProperty m_currnetScene;
        //SerializedProperty m_startLoadingScreen;
        ReorderableList m_loadingScreens;
        ReorderableList m_sceneProperties;

        SerializedProperty m_onSceneLoaded;
        SerializedProperty m_onSceneUnloaded;

        //helper var
        SerializedProperty m_openTab;
        string[] sceneNames;
        string[] loadingScreenNames;
        Vector2 scrollPosScene;
        Vector2 scrollPosLoading;
        bool _openScreneReview = false;

        protected virtual void OnEnable()
        {
            targetScript = target as UISceneManager;
            if (UISceneManager.Instance == null)
            {
                targetScript.Singleton();
            }
            m_openTab = serializedObject.FindProperty("openTab");
            m_startScene = serializedObject.FindProperty("startScene");
            m_previousScene = serializedObject.FindProperty("previousScene");
            m_currnetScene = serializedObject.FindProperty("currentScene");
            m_onSceneLoaded = serializedObject.FindProperty("onSceneLoaded");
            m_onSceneUnloaded = serializedObject.FindProperty("onSceneUnloaded");
            //m_startLoadingScreen = serializedObject.FindProperty("startLoadingScreen");
            m_loadingScreens = new ReorderableList(serializedObject, serializedObject.FindProperty("loadingScreens"), false, true, true, true);
            m_sceneProperties = new ReorderableList(serializedObject, serializedObject.FindProperty("sceneProperties"), true, true, true, true);
            //m_sceneProperties.elementHeight = EditorGUIUtility.singleLineHeight * 7.5f;
            m_sceneProperties.drawHeaderCallback = (Rect rect) => {
                //EditorGUI.LabelField(rect, "Scenes List");
            };
            m_sceneProperties.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                SceneArrayGUI(rect, index, isActive, isFocused);
            };
            m_sceneProperties.onSelectCallback = (ReorderableList l) => {
                var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("scene").objectReferenceValue as SceneAsset;
                if (prefab)
                    EditorGUIUtility.PingObject(prefab);
            };
            m_sceneProperties.onRemoveCallback = (ReorderableList l) => {
                if (l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("scene").objectReferenceValue != null)
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete element " + l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("scene").objectReferenceValue.name + " from Scenes list?", "Yes", "No"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete element " + l.index + " from Scenes list?", "Yes", "No"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }

                }
            };
            m_sceneProperties.elementHeightCallback = delegate (int index) {
                var element = m_sceneProperties.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("onSceneLoaded");
                var elementHeight = 110 + EditorGUI.GetPropertyHeight(element);
                return elementHeight;
            };

            //m_loadingScreens.elementHeight = EditorGUIUtility.singleLineHeight * 5.5f;
            m_loadingScreens.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Loading Screens List");
            };
            m_loadingScreens.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                LoadingScreensArrayGUI(rect, index, isActive, isFocused);
            };
            m_loadingScreens.onRemoveCallback = (ReorderableList l) => {
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete element " + (l.index+1) + " from Loading Screens list?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }                
            };
            m_loadingScreens.elementHeightCallback = delegate (int index) {
                var element = m_loadingScreens.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("onLoading");
                var elementHeight = 90 + EditorGUI.GetPropertyHeight(element);
                return elementHeight;
            };



        }


        public override void OnInspectorGUI()
        {
            UIEditor.DrawImage("UISceneManagerLabel");
            serializedObject.Update();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Scene Manager Properties", UIStyle.LabelBold);
            m_openTab.intValue = GUILayout.Toolbar(m_openTab.intValue, new string[] { "Scenes", "Loading Screens", "Events"});
            switch (m_openTab.intValue)
            {
                case 0:
                    // get scene list 

                    //m_startScene.intValue = EditorGUILayout.Popup("Start Scene", m_startScene.intValue, new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });

                    GUILayout.BeginVertical("HelpBox");
                    List<SceneProperties> sceneProperties = UISceneManager.Instance.sceneProperties;
                    string[] scenesNames = ConvertScenesToString(sceneProperties);
                    int idScene = -1;
                    idScene = sceneProperties.FindIndex(obj => obj.buildIndex == m_startScene.intValue);
                    idScene = (idScene < 0) ? 0 : idScene;
                    idScene = EditorGUILayout.Popup(new GUIContent("Start Scene", "Start scene opens when the application is launched together with the UIManager scene."), idScene, scenesNames);
                    if (sceneProperties.Count > 0)
                    {
                        m_startScene.intValue = sceneProperties[idScene].buildIndex;
                    }



                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("HelpBox");
                    EditorGUI.indentLevel++;
                    _openScreneReview = EditorGUILayout.Foldout(_openScreneReview, new GUIContent("Scene Review"), true, UIStyle.FoldoutBoldMini);
                    if (_openScreneReview)
                    {
                        GUILayout.Label("Current Scene", UIStyle.LabelBoldMini);
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(m_currnetScene.FindPropertyRelative("scene"), new GUIContent("Scene"));
                        if (m_currnetScene.FindPropertyRelative("buildIndex").intValue >= 0)
                        {
                            EditorGUILayout.PropertyField(m_currnetScene.FindPropertyRelative("buildIndex"), new GUIContent("Build Index"));
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Build Index", "Not added to build settings", "TextField");
                        }
                        EditorGUILayout.PropertyField(m_currnetScene.FindPropertyRelative("loadSceneMode"), new GUIContent("Load Scene Mode"));
                        GUI.enabled = true;
                        GUILayout.Label("Previous Scene", UIStyle.LabelBoldMini);
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(m_previousScene.FindPropertyRelative("scene"), new GUIContent("Scene"));
                        if (m_previousScene.FindPropertyRelative("buildIndex").intValue >= 0)
                        {
                            EditorGUILayout.PropertyField(m_previousScene.FindPropertyRelative("buildIndex"), new GUIContent("Build Index"));
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Build Index", "Not added to build settings", "TextField");
                        }
                        EditorGUILayout.PropertyField(m_previousScene.FindPropertyRelative("loadSceneMode"), new GUIContent("Load Scene Mode"));
                    }
                    EditorGUI.indentLevel--;
                    GUILayout.EndVertical();
                    GUI.enabled = true;
                    GUILayout.BeginVertical("HelpBox");
                    GUILayout.Label("Scene List", UIStyle.LabelBoldMini);
                    scrollPosScene = EditorGUILayout.BeginScrollView(scrollPosScene, false,true);
                    m_sceneProperties.DoLayoutList();
                    EditorGUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;
                case 1:
                    /*GUILayout.BeginVertical("HelpBox");
                    m_startLoadingScreen.intValue = EditorGUILayout.Popup(new GUIContent("Start Loading Screen"),m_startLoadingScreen.intValue, GetLoadingScreens());
                    GUILayout.EndVertical();*/
                    GUILayout.BeginVertical("HelpBox");
                    scrollPosLoading = EditorGUILayout.BeginScrollView(scrollPosLoading, false, true);
                    m_loadingScreens.DoLayoutList();
                    EditorGUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;
                case 2:
                    GUILayout.BeginVertical("HelpBox");
                    GUILayout.Label("UI Scene Manager Events", UIStyle.LabelBoldMini);
                    EditorGUILayout.PropertyField(m_onSceneLoaded, new GUIContent("On Scene Loaded"));
                    EditorGUILayout.PropertyField(m_onSceneUnloaded, new GUIContent("On Scene Unloaded"));
                    GUILayout.EndVertical();
                    break;
            }
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }


        private void SceneArrayGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            sceneNames = GetScenesFromBuildSettings();
            loadingScreenNames = GetLoadingScreens();
            var element = m_sceneProperties.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            string foldoutName;
            int buildIndex = -1;
            UnityEngine.Object obj = element.FindPropertyRelative("scene").objectReferenceValue;
            if (obj)
            {
                buildIndex = Array.FindIndex(sceneNames, scene => scene == obj.name);
                foldoutName = obj.name;
                if (buildIndex >= 0)
                    foldoutName += " (" + buildIndex + ")";
            }
            else
            {
                foldoutName = "No Scene";
            }
            EditorGUI.LabelField(rect, foldoutName, UIStyle.LabelBoldMini);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("scene"), new GUIContent("Scene","Drop here scene from project folder (Remember to add scene to Build Settings too)."));
            GUI.enabled = false;
            if (buildIndex >= 0)
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("buildIndex"),new GUIContent("Build Index", "Build index taked from Build Settings."));
                element.FindPropertyRelative("buildIndex").intValue = buildIndex;
            }
            else
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width, EditorGUIUtility.singleLineHeight),
                    new GUIContent("Build Index","You have to add this scene to Build Settings"), new GUIContent("Not added to build settings", "You have to add this scene to Build Settings."),"TextField");
                element.FindPropertyRelative("buildIndex").intValue = buildIndex;
            }
            GUI.enabled = true;
            element.FindPropertyRelative("isLoadingScreen").boolValue = EditorGUI.Toggle(new Rect(rect.x - 15, rect.y + EditorGUIUtility.singleLineHeight * 3, 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("isLoadingScreen").boolValue);
            if (element.FindPropertyRelative("isLoadingScreen").boolValue)
                GUI.enabled = true;
            else
                GUI.enabled = false;
            element.FindPropertyRelative("loadingScreenIndex").intValue = EditorGUI.Popup(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight),
                "Loading Screen",
                element.FindPropertyRelative("loadingScreenIndex").intValue,
                loadingScreenNames);
            GUI.enabled = true;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("loadSceneMode"),new GUIContent("Load Scene Mode", "Single - new scene replace old scene, Additive - add new scene to hierarchy."));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 5, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("closeAllUIScreenImmediately"),new GUIContent("Close UI", "On load new scene close all opened UI Screen Immidiately."));
            
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 6, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("onSceneLoaded"));
            EditorGUI.indentLevel--;
        }
        private void LoadingScreensArrayGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = m_loadingScreens.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            string foldoutName="";

            UnityEngine.Object obj = element.FindPropertyRelative("canvasGroup").objectReferenceValue;
            if (obj)
            {
                foldoutName = obj.name + " (" + index + ")";
            }
            else
            {
                foldoutName = "No Loading Screen";
            }
            EditorGUI.LabelField(rect, foldoutName, UIStyle.LabelBoldMini);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("canvasGroup"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("fadeInDuration"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("fadeOutDuration"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, rect.width, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("onLoading"), new GUIContent("On Loading"));
        }
            

        private string[] GetScenesFromBuildSettings()
        {
            List<string> temp = new List<string>();
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.Length - 6);
                    temp.Add(name);
                }
            }
            return temp.ToArray();
        }
        private string[] GetLoadingScreens()
        {
            List<string> temp = new List<string>();
            foreach (LoadingScreenProperties S in targetScript.loadingScreens)
            {
                if (S.canvasGroup)
                {
                    string name = S.canvasGroup.name;
                    temp.Add(name +" (" + temp.Count +")");
                }
            }
            return temp.ToArray();
        }
        private string[] ConvertScenesToString(List<SceneProperties> sceneList)
        {

            List<string> temp = new List<string>();
            foreach (SceneProperties S in sceneList)
            {
                if (S.scene)
                {
                    string name = S.scene.name;
                    temp.Add(name + " (" + S.buildIndex + ")");
                }
                else
                    temp.Add("No Scene" + " (-1)");

            }
            return temp.ToArray();

        }

         




    }

}
