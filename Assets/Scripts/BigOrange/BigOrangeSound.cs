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
    public void PlaySpringJump()  { PlayFromClip(clips[7], 1.0f); }
    public void PlayMetalHit()    { PlayFromClip(clips[8]); }
    public void PlayWindupCharge(){ PlayFromClip(clips[9]); }
    public void PlayWindupFist()  { PlayFromClip(clips[10]); }
    public void PlaySonar()       { PlayFromClip(clips[11]); }
    public void PlayStomp() { PlayFromClip(clips[12]); }

    public void PlayWindupFistFire() { PlayFromClip(clips[13]); }
    public void PlaySmallLand() { PlayFromClip(clips[14], 1.0f); }
    public void PlayClapExtend() { PlayFromClip(clips[15]); }
    public void PlayClapRetract() { PlayFromClip(clips[16]); }
    public void PlayCharge() { PlayFromClip(clips[17]); }

    public void PlayLaugh() { PlayFromClip(clips[18]); }

    public void PlayChargeWindup() { PlayFromClip(clips[19]); }


    void PlayFromClip(AudioClip clip)
    {
        GameSoundManagement.instance.efxSource.pitch = 1.0f;
        GameSoundManagement.instance.efxSource.loop = false;
        GameSoundManagement.instance.efxSource.PlayOneShot(clip);
    }

    void PlayFromClip(AudioClip clip, float pitch)
    {
        GameSoundManagement.instance.efxSource.loop = false;
        GameSoundManagement.instance.efxSource.pitch = UnityEngine.Random.Range(pitch - 0.1f, pitch + 0.1f);
        GameSoundManagement.instance.efxSource.PlayOneShot(clip);
    }
}
