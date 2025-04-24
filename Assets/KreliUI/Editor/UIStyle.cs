
using UnityEngine;

namespace KreliStudio
{
    public static class UIStyle
    {

        public static GUIStyle FoldoutBold
        {
            get
            {
                GUIStyle style = new GUIStyle("Foldout");
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 11;
                return style;
            }
        }
        public static GUIStyle FoldoutBoldMini
        {
            get
            {
                GUIStyle style = new GUIStyle("Foldout");
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 10;
                return style;
            }
        }
        public static GUIStyle LabelBold
        {
            get
            {
                GUIStyle style = new GUIStyle("BoldLabel");
                style.fontSize = 11;
                return style;
            }
        }
        public static GUIStyle LabelBoldMini
        {
            get
            {
                GUIStyle style = new GUIStyle("BoldLabel");
                style.fontSize = 10;
                return style;
            }
        }
        public static GUIStyle Toggle
        {
            get
            {
                GUIStyle style = new GUIStyle("Tooltip");
                style.normal.textColor = Color.white;
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }
        public static GUIStyle ButtonMini
        {
            get
            {
                GUIStyle style = new GUIStyle("MiniButton");
                return style;
            }
        }
    }
}
