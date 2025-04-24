using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PopupInformation : MonoBehaviour {

    public string[] desc;
    public Text text;

    public void SetDecs(int i)
    {
        text.text = desc[i];
    }
}
