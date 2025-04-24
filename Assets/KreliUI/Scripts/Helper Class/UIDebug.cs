using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KreliStudio {
    public static class UIDebug {

        public enum DebugType
        {
            UI,
            EVENT,
            SCREEN,
            SCENE
        }
        public static void PrintDebug(DebugType debugType, Transform t, string c, string f, string m)
        {

            string tName;
            int id;
            if (t != null)
            {
                tName = t.name;
                id = t.GetInstanceID();
            }
            else
            {
                tName = "Something";
                id = 0;
            }
            if (UIManager.Instance)
            {
                switch (debugType)
                {
                    case DebugType.UI:
                        if (UIManager.Instance.debugUIElements)
                        {
                            Debug.Log("[Debug UI Element]->[" + tName + ", id:" +id  + "]->[" + c + "]->[" + f + "()]-> " + m);
                        }
                        break;
                    case DebugType.EVENT:
                        if (UIManager.Instance.debugEvents)
                        {
                            Debug.Log("[Debug UI Event]->[" + tName + ", id:" +id + "]->[" + c + "]->[" + f + "()]-> " + m);
                        }
                        break;
                    case DebugType.SCREEN:
                        if (UIManager.Instance.debugUIScreen)
                        {
                            Debug.Log("[Debug UI Screen]->[" + tName + ", id:" +id + "]->[" + c + "]->[" + f + "()]-> " + m);
                        }
                        break;
                    case DebugType.SCENE:
                        if (UIManager.Instance.debugUIScene)
                        {
                            Debug.Log("[Debug UI Scene]->[" + tName + ", id:" +id + "]->[" + c + "]->[" + f + "()]-> " + m);
                        }
                        break;
                }
            }
        }
    }
}