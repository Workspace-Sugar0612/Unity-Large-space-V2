using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CNNetworkScene : NetworkBehaviour
{
    public ParticleSystem particleSystem; // when button poked, particle system play.
    public AudioSource audioSource; // when button poked, audio play.
    public Text incrementalNumberText;

    /// <summary>
    /// poke number.
    /// </summary>
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
        if (method == 1) { RpcSetAudioSourceState(true); }
        else if (method == 2) { RpcSetParticleState(true); }
        else if (method == 3) { RpcSetParticleState(false); }
        else if (method == 4) { CmdPokeNumberButton(); }
        else { }
    }

    [ClientRpc]
    private void RpcSetAudioSourceState(bool state)
    {
        if (state)
            audioSource.Play(); 
        else
            audioSource.Stop();
    }

    [ClientRpc]
    private void RpcSetParticleState(bool state)
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
