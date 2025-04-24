using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using System.Linq;

namespace KreliStudio
{
    [CustomEditor(typeof(UIButton), true)]
    [CanEditMultipleObjects]
    [InitializeOnLoadAttribute]
    public class UIButtonEditor : SelectableEditor
    {

        SerializedProperty m_sortingOrder;
        // onClick() Actions
        ActionsSerialized p_OnClickActions_SerializedObject;
        // onDoubleClick() Actions
        ActionsSerialized p_OnDoubleClick_SerializedObject;
        // onLongClick() Actions
        ActionsSerialized p_OnLongClick_SerializedObject;
        // onPointerEnter() Actions
        ActionsSerialized p_OnPointerEnter_SerializedObject;
        // onPointerExit() Actions
        ActionsSerialized p_OnPointerExit_SerializedObject;
        // onPointerDown() Actions
        ActionsSerialized p_OnPointerDown_SerializedObject;
        // onPointerUp() Actions
        ActionsSerialized p_OnPointerUp_SerializedObject;

        // show/hide panels
        SerializedProperty p_SelectablePropertiesFieldsOpen;
        // show/hide animation
        AnimBool m_SelectablePropertiesFieldsOpen;
        AnimBool m_OnClick_FieldsOpen;
        AnimBool m_OnDoubleClick_FieldsOpen;
        AnimBool m_OnLongClick_FieldsOpen;
        AnimBool m_OnPointEnter_FieldsOpen;
        AnimBool m_OnPointExit_FieldsOpen;
        AnimBool m_OnPointDown_FieldsOpen;
        AnimBool m_OnPointUp_FieldsOpen;
        //show/hide actions
        bool m_isEventOpen;
        bool m_isUIScreenOpen;
        bool m_isSceneLoaderOpen;
        

        float bezier=0f;
        // colors enable/disable
        Color colorBacgroundEnable = new Color(0.55f,1.0f,0.55f);
        Color colorBacgroundDisable = new Color(1.0f, 0.55f, 0.55f);


        //helper var
        bool _openSelectable;

        

        protected override void OnEnable()
        {
            base.OnEnable();
            p_SelectablePropertiesFieldsOpen = serializedObject.FindProperty("editorMenuSelectableProperties");
            //m_sortingOrder = serializedObject.FindProperty("sortingOrder");
            p_OnClickActions_SerializedObject = SetActionsSerializedProperty("actionsOnClick");
            p_OnDoubleClick_SerializedObject = SetActionsSerializedProperty("actionsOnDoubleClick");
            p_OnLongClick_SerializedObject = SetActionsSerializedProperty("actionsOnLongClick");
            p_OnPointerEnter_SerializedObject = SetActionsSerializedProperty("actionsOnPointerEnter");
            p_OnPointerExit_SerializedObject = SetActionsSerializedProperty("actionsOnPointerExit");
            p_OnPointerDown_SerializedObject = SetActionsSerializedProperty("actionsOnPointerDown");
            p_OnPointerUp_SerializedObject = SetActionsSerializedProperty("actionsOnPointerUp");


            // set data from script to custom editor
            m_SelectablePropertiesFieldsOpen = new AnimBool(p_SelectablePropertiesFieldsOpen.boolValue);

            m_OnClick_FieldsOpen = new AnimBool(p_OnClickActions_SerializedObject.isOpen.boolValue);
            m_OnDoubleClick_FieldsOpen = new AnimBool(p_OnDoubleClick_SerializedObject.isOpen.boolValue);
            m_OnLongClick_FieldsOpen = new AnimBool(p_OnLongClick_SerializedObject.isOpen.boolValue);
            m_OnPointEnter_FieldsOpen = new AnimBool(p_OnPointerEnter_SerializedObject.isOpen.boolValue);
            m_OnPointExit_FieldsOpen = new AnimBool(p_OnPointerExit_SerializedObject.isOpen.boolValue);
            m_OnPointDown_FieldsOpen = new AnimBool(p_OnPointerDown_SerializedObject.isOpen.boolValue);
            m_OnPointUp_FieldsOpen = new AnimBool(p_OnPointerUp_SerializedObject.isOpen.boolValue);

            // repaint elements
            m_SelectablePropertiesFieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));

            m_OnClick_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnDoubleClick_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnLongClick_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnPointEnter_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnPointExit_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnPointDown_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
            m_OnPointUp_FieldsOpen.valueChanged.AddListener(new UnityAction(base.Repaint));
        }



        protected override void OnDisable()
        {
            base.OnDisable();
            bezier = 0;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUI.changed)
            {
                Debug.Log("Button changed");
            }
                UIEditor.DrawImage("UIButtonLabel");

            GUILayout.BeginVertical("Box");
            GUILayout.Label("UI Button Properties", UIStyle.LabelBold);
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
            /////////////////////////////////
            GUILayout.BeginHorizontal();
            GUILayout.Label("UI Button Actions", UIStyle.LabelBold);
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), "selectionRect")) {
                SetAllOpen(false);
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"), "selectionRect"))
            {
                SetAllOpen(true);
            }

            EditorGUILayout.EndHorizontal();
            ///////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnClick_FieldsOpen.target = GUILayout.Toggle(m_OnClick_FieldsOpen.target, "Actions On Click()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnClickActions_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnClickActions_SerializedObject.isOpen.boolValue = m_OnClick_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnClick_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnClickActions_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnClick_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            ///////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnDoubleClick_FieldsOpen.target = GUILayout.Toggle(m_OnDoubleClick_FieldsOpen.target, "Actions On Double Click()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnDoubleClick_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnDoubleClick_SerializedObject.isOpen.boolValue = m_OnDoubleClick_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnDoubleClick_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnDoubleClick_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnDoubleClick_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            ///////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnLongClick_FieldsOpen.target = GUILayout.Toggle(m_OnLongClick_FieldsOpen.target, "Actions On Long Click()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnLongClick_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnLongClick_SerializedObject.isOpen.boolValue = m_OnLongClick_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnLongClick_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnLongClick_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnLongClick_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            /////////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnPointEnter_FieldsOpen.target = GUILayout.Toggle(m_OnPointEnter_FieldsOpen.target, "Actions On Pointer Enter()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnPointerEnter_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnPointerEnter_SerializedObject.isOpen.boolValue = m_OnPointEnter_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnPointEnter_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnPointerEnter_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnPointEnter_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            /////////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnPointExit_FieldsOpen.target = GUILayout.Toggle(m_OnPointExit_FieldsOpen.target, "Actions On Pointer Exit()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnPointerExit_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnPointerExit_SerializedObject.isOpen.boolValue = m_OnPointExit_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnPointExit_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnPointerExit_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnPointExit_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            ///////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnPointDown_FieldsOpen.target = GUILayout.Toggle(m_OnPointDown_FieldsOpen.target, "Actions On Pointer Down()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnPointerDown_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnPointerDown_SerializedObject.isOpen.boolValue = m_OnPointDown_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnPointDown_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnPointerDown_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnPointDown_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            ///////////////////////////////////
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical("HelpBox");
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            m_OnPointUp_FieldsOpen.target = GUILayout.Toggle(m_OnPointUp_FieldsOpen.target, "Actions On Pointer Up()", UIStyle.ButtonMini);
            DrawActionMiniIcon(p_OnPointerUp_SerializedObject);
            EditorGUILayout.EndHorizontal();
            p_OnPointerUp_SerializedObject.isOpen.boolValue = m_OnPointUp_FieldsOpen.target;
            using (var group = new EditorGUILayout.FadeGroupScope(m_OnPointUp_FieldsOpen.faded))
            {
                if (group.visible)
                {
                    DrawActionSetup(p_OnPointerUp_SerializedObject);
                }
            }
            GUILayout.EndVertical();
            if (m_OnPointUp_FieldsOpen.target)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            /////////////////////////////////



            GUILayout.EndVertical();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
        

        
        private void OnSceneGUI()
        {

            // Debug.Log("counter: " + counter);
            if (p_OnClickActions_SerializedObject.isUIScreen.boolValue)
                OnSceneViewGUI();
        }
        private void DrawActionMiniIcon(ActionsSerialized thisAction)
        {
            if (thisAction.isEvent.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            GUILayout.Toggle(thisAction.isEvent.boolValue, new GUIContent(" E ", "Unity Events"), UIStyle.Toggle);
            if (thisAction.isUIScreen.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            GUILayout.Toggle(thisAction.isUIScreen.boolValue, new GUIContent(" U ", "UIScreen"), UIStyle.Toggle);
            if (thisAction.isSceneLoader.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            GUILayout.Toggle(thisAction.isSceneLoader.boolValue, new GUIContent(" S ", "Scene"), UIStyle.Toggle);


            GUI.backgroundColor = Color.white;
        }
        private void DrawActionSetup(ActionsSerialized thisAction)
        {
            m_isEventOpen = thisAction.isEventOpen.boolValue;
            m_isUIScreenOpen = thisAction.isUIScreenOpen.boolValue;
            m_isSceneLoaderOpen = thisAction.isSceneLoaderOpen.boolValue;

            EditorGUI.indentLevel++;
            ///////////////////////////////////
            EditorGUILayout.BeginVertical("ObjectFieldThumb");
            EditorGUILayout.BeginHorizontal("ObjectPickerToolbar");
            m_isEventOpen = EditorGUILayout.Foldout(m_isEventOpen,new GUIContent("Invoke Event", "Call event after a specified action."), true, "Foldout");
            if (thisAction.isEvent.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            thisAction.isEvent.boolValue = GUILayout.Toggle(thisAction.isEvent.boolValue, thisAction.isEvent.boolValue ? "  ENABLE  " : " DISABLE ", UIStyle.Toggle);
            if (thisAction.isEvent.boolValue)
                GUI.backgroundColor = colorBacgroundEnable;
            else
                GUI.backgroundColor = colorBacgroundDisable;
            EditorGUILayout.EndHorizontal();
            thisAction.isEventOpen.boolValue = m_isEventOpen;
            if (m_isEventOpen)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(thisAction.unityEvent, new GUIContent("On Action"));
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            ///////////////////////////////////


            EditorGUILayout.BeginVertical("ObjectFieldThumb");
            EditorGUILayout.BeginHorizontal("ObjectPickerToolbar");
            m_isUIScreenOpen = EditorGUILayout.Foldout(m_isUIScreenOpen, new GUIContent("Open UI Screen", "Open UIScreen after a specified action."), true, "Foldout");
            if (thisAction.isUIScreen.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            thisAction.isUIScreen.boolValue = GUILayout.Toggle(thisAction.isUIScreen.boolValue, thisAction.isUIScreen.boolValue ? "  ENABLE  " : " DISABLE ", UIStyle.Toggle);
            if (thisAction.isUIScreen.boolValue)
                GUI.backgroundColor = colorBacgroundEnable;
            else
                GUI.backgroundColor = colorBacgroundDisable;
            EditorGUILayout.EndHorizontal();
            thisAction.isUIScreenOpen.boolValue = m_isUIScreenOpen;
            if (m_isUIScreenOpen)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();


                UIScreen[] uiScreen = UIEditor.GetUIScreens();
                string[] uiScreenNames = UIEditor.GetUIScreensNames(uiScreen);
                int idScreen = Array.FindIndex(uiScreen, obj => obj == thisAction.uiScreen.GetValue<UIScreen>());
                idScreen = (idScreen < 0) ? 0 : idScreen;
                idScreen = EditorGUILayout.Popup(new GUIContent("UIScreen","Choose UIScreen to open."), idScreen, uiScreenNames);

                if (uiScreen.Length > 0)
                {
                    thisAction.uiScreen.SetValue(uiScreen[idScreen]);
                }

                if (GUILayout.Button(new GUIContent("Ping", "Show connected UIScreen to this UIButton.")) && thisAction.uiScreen.GetValue<UIScreen>() != null)
                {
                    EditorGUIUtility.PingObject(thisAction.uiScreen.GetValue<UIScreen>().GetInstanceID());
                    bezier = 1f;
                }
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            ///////////////////////////////////


            EditorGUILayout.BeginVertical("ObjectFieldThumb");
            EditorGUILayout.BeginHorizontal("ObjectPickerToolbar");
            m_isSceneLoaderOpen = EditorGUILayout.Foldout(m_isSceneLoaderOpen, new GUIContent("Load Scene", "Open Scene after a specified action."), true, "Foldout");
            if (thisAction.isSceneLoader.boolValue)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;
            thisAction.isSceneLoader.boolValue = GUILayout.Toggle(thisAction.isSceneLoader.boolValue, thisAction.isSceneLoader.boolValue ? "  ENABLE  " : " DISABLE ", UIStyle.Toggle);
            if (thisAction.isSceneLoader.boolValue)
                GUI.backgroundColor = colorBacgroundEnable;
            else
                GUI.backgroundColor = colorBacgroundDisable;
            EditorGUILayout.EndHorizontal();
            thisAction.isSceneLoaderOpen.boolValue = m_isSceneLoaderOpen;
            if (m_isSceneLoaderOpen)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                if (UISceneManager.Instance)
                {
                    // get scene list
                    List<SceneProperties> sceneProperties = GetAllScenes(UISceneManager.Instance.sceneProperties); 
                    string[] scenesNames = ConvertScenesToString(sceneProperties);
                    int idScene = -1;
                    idScene = sceneProperties.FindIndex(obj => obj.buildIndex == thisAction.buildIndex.intValue);
                    idScene = (idScene < 0) ? 0 : idScene;
                    idScene = EditorGUILayout.Popup(new GUIContent("Scene", "Choose Scene to open."), idScene, scenesNames);
                    if (sceneProperties.Count > 0)
                    {
                        thisAction.buildIndex.intValue = sceneProperties[idScene].buildIndex;
                    }

                    EditorGUILayout.Space();
                    if (GUILayout.Button(new GUIContent("Open Scene Manager", "Show UI Scene Manager in inspector.")))
                    {
                        EditorGUIUtility.PingObject(UISceneManager.Instance.gameObject);
                        Selection.activeGameObject = UISceneManager.Instance.gameObject;
                    }
                }
                else
                {
                    GUILayout.Label("Create UI Scene Manager first");
                    GUILayout.Label("Find it in \"Create/KreliStudio/UI SceneManager\".");
                    EditorGUILayout.Space();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            GUI.backgroundColor = Color.white;
            /*
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Sound & Vibration",UIStyle.LabelBoldMini);
            EditorGUILayout.PropertyField(thisAction.isSound, new GUIContent("Play Sound"));
            if (thisAction.isSound.boolValue)
            {
                EditorGUILayout.PropertyField(thisAction.sound.FindPropertyRelative("clip"), new GUIContent("Clip"));
                EditorGUILayout.Slider(thisAction.sound.FindPropertyRelative("volume"), 0f, 1f, new GUIContent("Volume"));
                EditorGUILayout.Slider(thisAction.sound.FindPropertyRelative("pitch"), 0.1f, 3f, new GUIContent("Pitch"));
            }
            EditorGUILayout.PropertyField(thisAction.isVibration, new GUIContent("Vibration"));
            if (thisAction.isVibration.boolValue)
            {
                EditorGUILayout.IntSlider(thisAction.vibrate, 10, 1000, new GUIContent("Duration"));
            }

            EditorGUILayout.EndVertical();*/
            EditorGUILayout.EndVertical();
            ///////////////////////////////////
            EditorGUI.indentLevel--;
        }
   

        private ActionsSerialized SetActionsSerializedProperty(string inScriptActionsName)
        {
            ActionsSerialized actions = new ActionsSerialized
            {
                isEvent = serializedObject.FindProperty(inScriptActionsName + ".isEvent"),
                unityEvent = serializedObject.FindProperty(inScriptActionsName + ".unityEvent"),
                isUIScreen = serializedObject.FindProperty(inScriptActionsName + ".isUIScreen"),
                uiScreen = serializedObject.FindProperty(inScriptActionsName + ".uiScreen"),
                uiScreenIndex = serializedObject.FindProperty(inScriptActionsName + ".uiScreenIndex"),
                isSceneLoader = serializedObject.FindProperty(inScriptActionsName + ".isSceneLoader"),
                buildIndex = serializedObject.FindProperty(inScriptActionsName + ".buildIndex"),
                isOpen = serializedObject.FindProperty(inScriptActionsName + ".isOpen"),
                isEventOpen = serializedObject.FindProperty(inScriptActionsName + ".isEventOpen"),
                isUIScreenOpen = serializedObject.FindProperty(inScriptActionsName + ".isUIScreenOpen"),
                isSceneLoaderOpen = serializedObject.FindProperty(inScriptActionsName + ".isSceneLoaderOpen"),
                isSound = serializedObject.FindProperty(inScriptActionsName + ".isSound"),
                sound = serializedObject.FindProperty(inScriptActionsName + ".sound"),
                isVibration = serializedObject.FindProperty(inScriptActionsName + ".isVibration"),
                vibrate = serializedObject.FindProperty(inScriptActionsName + ".vibrate")

            };

            return actions;
        }

        private List<SceneProperties>GetAllScenes(List<SceneProperties> sceneList)
        {
            List<SceneProperties> temp = new List<SceneProperties>(sceneList);
            temp.Insert(0, new SceneProperties(-100)); // previous scene
            return temp;
        }
        private string[] ConvertScenesToString(List<SceneProperties> sceneList)
        {

            List<string> temp = new List<string>();
            foreach (SceneProperties S in sceneList)
            {
                if (S.buildIndex == -100)
                {
                    temp.Add("[Previous Scene]");
                }
                else
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
        private void SetAllOpen(bool isOpen)
        {
            m_OnClick_FieldsOpen.target = isOpen;
            m_OnDoubleClick_FieldsOpen.target = isOpen;
            m_OnLongClick_FieldsOpen.target = isOpen;
            m_OnPointEnter_FieldsOpen.target = isOpen;
            m_OnPointExit_FieldsOpen.target = isOpen;
            m_OnPointDown_FieldsOpen.target = isOpen;
            m_OnPointUp_FieldsOpen.target = isOpen;

            SetOpen(p_OnClickActions_SerializedObject, isOpen);
            // onDoubleClick() Actions
            SetOpen(p_OnDoubleClick_SerializedObject, isOpen);
            // onLongClick() Actions
            SetOpen(p_OnLongClick_SerializedObject, isOpen);
            // onPointerEnter() Actions
            SetOpen(p_OnPointerEnter_SerializedObject, isOpen);
            // onPointerExit() Actions
            SetOpen(p_OnPointerExit_SerializedObject, isOpen);
            // onPointerDown() Actions
            SetOpen(p_OnPointerDown_SerializedObject, isOpen);
            // onPointerUp() Actions
            SetOpen(p_OnPointerUp_SerializedObject, isOpen);
        }
        private void SetOpen(ActionsSerialized action, bool isOpen)
        {
            action.isOpen.boolValue = isOpen;
            action.isEventOpen.boolValue = isOpen;
            action.isUIScreenOpen.boolValue = isOpen;
            action.isSceneLoaderOpen.boolValue = isOpen;
        }
        private void OnSceneViewGUI()
        {
            if (p_OnClickActions_SerializedObject.uiScreen.GetValue<UIScreen>() != null) {

                UIButton targetButton = target as UIButton;
                UIScreen targetScreen = p_OnClickActions_SerializedObject.uiScreen.GetValue<UIScreen>();

                Vector3 startPoint = targetButton.transform.position;
                Vector3 endPoint = targetScreen.transform.position;

                Vector3 startTangent = new Vector3(endPoint.x, startPoint.y, 0);
                Vector3 endTangent = new Vector3(startPoint.x, endPoint.y, 0);

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.yellow, null, 4f* bezier);
                Handles.color = Color.yellow;
                Handles.CircleHandleCap(0, startPoint, Quaternion.identity, 1f * bezier, EventType.Repaint);
                Handles.RectangleHandleCap(0, endPoint, Quaternion.identity, 1f * bezier,EventType.Repaint);

            }
        }
}

    struct ActionsSerialized
    {
        public SerializedProperty isEvent;
        public SerializedProperty unityEvent;
        public SerializedProperty isUIScreen;
        public SerializedProperty uiScreen;
        public SerializedProperty uiScreenIndex;
        public SerializedProperty isSceneLoader;
        public SerializedProperty buildIndex;
        public SerializedProperty isSound;
        public SerializedProperty sound;
        public SerializedProperty isVibration;
        public SerializedProperty vibrate;

        public SerializedProperty isOpen;
        public SerializedProperty isEventOpen;
        public SerializedProperty isUIScreenOpen;
        public SerializedProperty isSceneLoaderOpen;
        
    }


}