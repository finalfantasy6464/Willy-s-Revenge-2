using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public AudioClip Meteor1;
    public AudioClip Meteor2;
   

    public Localaudioplayer localaudio;

    PositionalSoundData soundData;

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
}
