using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private Track backsound;
    private void Start()
    {
        PlayBacksound();
    }

    public void PlayBacksound()
    {
        MusicManager.Main.Play(backsound);
    }
}
