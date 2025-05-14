using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using DG.Tweening;

public class WindowSizeController : MonoBehaviour
{
    public Button toggleFullscreenButton;
    public List<imgType> imageTypes;
    public float fadeDuration = 0.5f;


    [System.Serializable]
    public struct imgType
    {
        public string typeName;
        public Sprite img;
    }

    void Start()
    {
        if (toggleFullscreenButton != null)
        {
            toggleFullscreenButton.onClick.AddListener(ToggleFullscreen);
            UpdateButtonImage();
        }
    }

    public void ToggleFullscreen()
    {
        if (IsFullScreenWindow())
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            if (isMobile())
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else RequestFullscreen();
        }

        UpdateButtonImageWithDelay(); // Update image with delay and fade
    }

    #region browserInteraction

    [DllImport("__Internal")]
    private static extern void WebGLFullscreen();

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    private void RequestFullscreen()
    {
        WebGLFullscreen();
    }

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
#endif
        return false;
    }
    #endregion

    private void UpdateButtonImageWithDelay()
    {
        if (toggleFullscreenButton != null && toggleFullscreenButton.image != null && imageTypes != null)
        {
            toggleFullscreenButton.interactable = false;
            toggleFullscreenButton.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration).OnComplete(() =>
            {
                UpdateButtonImage();
            });
        }
    }

    private void UpdateButtonImage()
    {
        if (toggleFullscreenButton != null && toggleFullscreenButton.image != null && imageTypes != null)
        {
            if (IsFullScreenWindow())
            {
                // Set to minimize sprite
                var minimizeImage = imageTypes.Find(img => img.typeName == "minimize");
                if (minimizeImage.img != null)
                {
                    toggleFullscreenButton.image.sprite = minimizeImage.img;
                }
            }
            else
            {
                // Set to maximize sprite
                var maximizeImage = imageTypes.Find(img => img.typeName == "maximize");
                if (maximizeImage.img != null)
                {
                    toggleFullscreenButton.image.sprite = maximizeImage.img;
                }
            }

            toggleFullscreenButton.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration).OnComplete(() =>
            {
                toggleFullscreenButton.interactable = true;
            });
        }
    }

    private bool IsFullScreenWindow()
    {
        return (Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.MaximizedWindow);
    }
}
