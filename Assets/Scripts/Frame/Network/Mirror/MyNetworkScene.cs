using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkScene : NetworkBehaviour
{
    public ParticleSystem particleSystem; // when button poked, particle system play.
    public AudioSource audioSource; // when button poked, audio play.
    public Text incrementalNumberText;

    [SyncVar(hook = nameof(OnPokeNumberChangedHook))]
    private int pokeNumber;

    [Command(requiresAuthority = false)]
    public void RpcGenericInterface(int method)
    {
        RpcGenericNetwork(method);
    }

    //[ClientRpc]
    private void RpcGenericNetwork(int method)
    {
        if (method == 1) { SetAudioSourceState(true); }
        else if (method == 2) { SetParticleState(true); }
        else if (method == 3) { SetParticleState(false); }
        else if (method == 4) { CmdPokeNumberButton(); }
        else { }
    }

    [ClientRpc]
    private void SetAudioSourceState(bool state)
    {
        if (state)
            audioSource.Play(); 
        else
            audioSource.Stop();
    }

    [ClientRpc]
    private void SetParticleState(bool state)
    {
        if (state)
            particleSystem.Play();
        else
            particleSystem.Stop();
    }

    //[Command(requiresAuthority = false)]
    private void CmdPokeNumberButton()
    {
        //Log.input("================CmdPokeNumberButton========================");
        pokeNumber += 1;
    }

    private void OnPokeNumberChangedHook(int _old, int _new)
    {
        //Log.input("==============OnPokeNumberChangedHook========================");
        incrementalNumberText.text = pokeNumber.ToString();
    }
}
