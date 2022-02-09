using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BigOrangeSound : LocalAudioPlayer
{
    [SerializeField]
    AudioClip[] clips;
    
    public void PlayClap()    { PlayFromClip(clips[0]); }
    public void PlayDamage()  { PlayFromClip(clips[1]); }
    public void PlayDeath()   { PlayFromClip(clips[2]); }
    public void PlayElectric(){ PlayFromClip(clips[3]); }
    public void PlayFist()    { PlayFromClip(clips[4]); }
    public void PlayJump()    { PlayFromClip(clips[5]); }
    public void PlayLand()    { PlayFromClip(clips[6]); }

    void PlayFromClip(AudioClip clip)
    {
        GameSoundManagement.instance.efxSource.loop = false;
        GameSoundManagement.instance.efxSource.PlayOneShot(clip);
    }
}
