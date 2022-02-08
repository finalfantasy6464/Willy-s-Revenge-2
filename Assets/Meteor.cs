using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour, IPausable
{
    public AudioClip Meteor1;
    public AudioClip Meteor2;

    public Animator anim;
   

    public LocalAudioPlayer localaudio;

    PositionalSoundData soundData;

    public bool isPaused { get; set; }

    public void Start()
    {
        soundData = localaudio.soundData;
    }
    public void PlayMeteor1()
    {
        soundData.clip = Meteor1;
        localaudio.SoundPlay();
    }

    public void PlayMeteor2()
    {
        soundData.clip = Meteor2;
        localaudio.SoundPlay();
    }

    public void PlayMeteor3()
    {
        Destroy(gameObject);
    }

    public void OnPause()
    {
        anim.speed = 0;
    }

    public void OnUnpause()
    {
        anim.speed = 1;
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }

    public void UnPausedUpdate()
    {
        
    }
}
