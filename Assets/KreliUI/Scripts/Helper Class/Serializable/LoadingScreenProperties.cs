using System;
using UnityEngine;
using UnityEngine.Events;

namespace KreliStudio
{
    [Serializable]
    public class LoadingEvent : UnityEvent<float> { }
    [Serializable]
    public class LoadingScreenProperties
    {

        //public int loadingScreenIndex;
        public CanvasGroup canvasGroup;
        public float fadeInDuration;
        public float fadeOutDuration;
        public LoadingEvent onLoading = new LoadingEvent();



        public void OnLoading(float _progress)
        {
            if (onLoading != null)
            {
                try
                {
                    onLoading.Invoke(_progress);
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event onLoading. Error: " + exception.Message);
                }
            }            
        }
    }
}