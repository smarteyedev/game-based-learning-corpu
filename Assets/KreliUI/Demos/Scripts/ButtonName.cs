using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonName : MonoBehaviour {

    public Text buttonText;

    public void SetText(string str)
    {
        buttonText.text = str;
    }
}
