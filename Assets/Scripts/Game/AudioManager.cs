using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private AudioSource audioSource;



    public void ToggleAudioMute()
    {
        this.audioSource.mute = !this.audioSource.mute;
    }

    public bool IsAudioMuted()
    {
        return this.audioSource.mute;
    }
}
