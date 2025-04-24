/*using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound{

    public GameObject parent=null;
    public AudioClip clip;
    //[Range(0.0f,1.0f)]
    public float volume=1f;
    //[Range(0.1f, 3.0f)]
    public float pitch=1f;
    public AudioSource source;

    public void AddParent(GameObject _parent)
    {
        parent = _parent;
    }

    public void Play()
    {
        if (source)
        {
            source.Play();
        }
        else
        {
            CreateSource();
            source.Play();
        }
    }

    private void CreateSource()
    {
        source = parent.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
    }


}*/
