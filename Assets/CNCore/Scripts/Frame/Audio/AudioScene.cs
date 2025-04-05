
using Mirror;
using UnityEngine;

public class AudioScene : LocalAudioBase
{
    void Awake()
    {
        OperateAudio(AudioMethod.Play);
    }
}