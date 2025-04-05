using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }

    public void Play(AudioSource audio)
    {
        if (audio != null)
        {
            audio.Play();
        }
    }

    public void Pause(AudioSource audio)
    {
        if (audio != null)
        {
            audio.Pause();
        }
    }

    public void Stop(AudioSource audio)
    {
        if (audio != null)
        {
            audio.Stop();
        }
    }
}
