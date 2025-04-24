using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingText : MonoBehaviour {

    public Text loadingProgressText;

	public void SetProgress(float progress)
    {
        loadingProgressText.text = (progress*100).ToString("0");
    }
}
