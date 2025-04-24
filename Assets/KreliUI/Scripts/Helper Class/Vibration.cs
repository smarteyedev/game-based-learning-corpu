using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour {

#if UNITY_ANDROID && !UNITY_EDITOR
    private static readonly AndroidJavaObject Vibrator =
    new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
    .GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
    .Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.
#endif
    public static bool isVibration=true;

    static Vibration()
    {
        // Trick Unity into giving the App vibration permission when it builds.
        // This check will always be false, but the compiler doesn't know that.
#if UNITY_ANDROID || UNITY_EDITOR
        if (Application.isEditor) Handheld.Vibrate();
#endif
    }

    public static void Vibrate(long milliseconds)
    {
        if (isVibration) { 
#if UNITY_ANDROID && !UNITY_EDITOR
        Vibrator.Call("vibrate", milliseconds);
#else
              Debug.Log("Vibrate " + milliseconds +" milliseconds.");
#endif
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isVibration)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        Vibrator.Call("vibrate", pattern, repeat);
#else
             Debug.Log("Vibrate pattern.");
#endif
        }
    }
}
