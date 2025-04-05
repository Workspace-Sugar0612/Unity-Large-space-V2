using System.Collections.Generic;
using System.ComponentModel;
using Mirror;
using UnityEngine;

public class NetworkAudioBase : NetworkBehaviour
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
    [ClientRpc]
    public virtual void RpcPlay()
    {
        Log.cinput("Green", "rpc audio playing..");
        AudioManager.singleton.Play(audioSource[currIndex]);
    }

    /// <summary>
    /// 暂停音乐
    /// </summary>
    [ClientRpc]
    public virtual void RpcPause()
    {
        AudioManager.singleton.Pause(audioSource[currIndex]);
    }

    /// <summary>
    /// 停止音乐
    /// </summary>
    [ClientRpc]
    public virtual void RpcStop()
    {
        AudioManager.singleton.Stop(audioSource[currIndex]);
    }

    [Command(requiresAuthority = false)]
    public void CmdOperateAudio(AudioMethod method)
    {
        Log.cinput("green", "Enter CmdOperateAudio function.");
        if (method == AudioMethod.Play)
        {
            RpcPlay();
        }
        else if (method == AudioMethod.Pause)
        {
            RpcPause();
        }
        else if (method == AudioMethod.Stop)
        {
            RpcStop();
        }
    }
}