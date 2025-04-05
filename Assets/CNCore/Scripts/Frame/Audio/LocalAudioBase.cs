using System.Collections.Generic;
using System.ComponentModel;
using Mirror;
using UnityEngine;

public class LocalAudioBase : MonoBehaviour
{
    [Tooltip("音乐资源")]
    [SerializeField]
    List<AudioSource> audioSource;

    /// <summary>
    /// 目前播放音乐的索引
    /// </summary>
    int currIndex = 0;

    void Awake()
    {
        currIndex = 0;
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    public virtual void Play()
    {
        Log.cinput("green", "audio playing..");
        AudioManager.singleton.Play(audioSource[currIndex]);
    }

    /// <summary>
    /// 暂停音乐
    /// </summary>
    public virtual void Pause()
    {
        Log.cinput("green", "audio Pause..");
        AudioManager.singleton.Pause(audioSource[currIndex]);
    }

    /// <summary>
    /// 停止音乐
    /// </summary>
    public virtual void Stop()
    {
        Log.cinput("green", "audio Stop..");
        AudioManager.singleton.Stop(audioSource[currIndex]);
    }

    public void OperateAudio(AudioMethod method)
    {
        Log.cinput("green", "Enter OperateAudio function.");
        if (method == AudioMethod.Play)
        {
            Play();
        }
        else if (method == AudioMethod.Pause)
        {
            Pause();
        }
        else if (method == AudioMethod.Stop)
        {
            Stop();
        }
    }
}

public enum AudioMethod
{
    Neno,
    Play,
    Stop,
    Pause
}