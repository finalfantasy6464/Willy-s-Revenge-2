using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BigOrangeSound : LocalAudioPlayer
{
    [SerializeField]
    BigOrange orange;
    [SerializeField]
    PositionalSoundData[] sounds;
    GameSoundManagement soundManagement => GameSoundManagement.instance;
    Vector3 midFeet => (orange.leftFoot.position + orange.rightFoot.position) / 2f;
    Vector3 midHands => (orange.leftHand.position + orange.rightHand.position) / 2f;

    public AudioSource PlayElectric(Vector3 armPosition)
    {
        return soundManagement.PlayPositional(sounds[3], armPosition);
    }

    public void PlayFist(int handIndex)
    {
        PlayFromClip(sounds[4], handIndex < 1 ? orange.leftHand.position : orange.rightHand.position );
    }

    public void PlayWindupFist(int handIndex)  
    {
        PlayFromClip(sounds[10], handIndex < 1 ? orange.leftHand.position : orange.rightHand.position );
    }

    public void PlayWindupFistFire(int handIndex)
    {
        PlayFromClip(sounds[13], handIndex < 1 ? orange.leftHand.position : orange.rightHand.position );
    }

    public void PlayStomp()
    {
        PlayFromClip(sounds[12], midFeet);
    }
    
    public void PlayClap()    
    {
        PlayFromClip(sounds[0], midHands);
    }

    public void PlayDamage()  
    {
        PlayFromClip(sounds[1]);
    }

    public void PlayDeath()   
    {
        PlayFromClip(sounds[2]);
    }

    public void PlayJump()    
    {
        PlayFromClip(sounds[5], midFeet);
    }

    public void PlayLand()    
    {
        PlayFromClip(sounds[6], midFeet);
    }

    public void PlaySpringJump()  
    {
        PlayFromClip(sounds[7], midFeet);
    }

    public void PlayMetalHit()    
    {
        PlayFromClip(sounds[8]);
    }

    public void PlayWindupCharge()
    {
        PlayFromClip(sounds[9]);
    }

    public void PlaySonar()       
    {
        PlayFromClip(sounds[11]);
    }

    public void PlaySmallLand()
    {
        PlayFromClip(sounds[14], midFeet);
    }

    public void PlayClapExtend()
    {
        PlayFromClip(sounds[15], midHands);
    }

    public void PlayClapRetract()
    {
        PlayFromClip(sounds[16], midHands);
    }

    public void PlayCharge()
    {
        PlayFromClip(sounds[17]);
    }

    public void PlayLaugh()
    {
        PlayFromClip(sounds[18]);
    }

    void PlayFromClip(PositionalSoundData sound, Vector2 position)
    {
        soundManagement.PlayPositional(sound, position);
    }

    void PlayFromClip(PositionalSoundData sound)
    {
        soundManagement.PlayPositional(sound, orange.transform.position);
    }
}
