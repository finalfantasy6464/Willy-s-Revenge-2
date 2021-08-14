using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrangeSound : MonoBehaviour
{
    public AudioClip OrangeStomp;
    public AudioClip OrangeHandSmash;
    public AudioClip OrangeJump;
    public AudioClip OrangeLanding;
    public AudioClip OrangeHurt;
    public AudioClip OrangeDeath;

    public Transform Lefthand;
    public Transform Righthand;
    public Transform Leftfoot;
    public Transform Head;
    public Transform Body;


    public Localaudioplayer localaudio;

    PositionalSoundData soundData;

    public void Start()
    {
        soundData = localaudio.soundData;
    }
    public void PlayOrangeStomp()
    {
        soundData.clip = OrangeStomp;
        localaudio.Emitter = Leftfoot;
        localaudio.SoundPlay();
    }

    public void PlayOrangeSmashLeft()
    {
        soundData.clip = OrangeHandSmash;
        localaudio.Emitter = Lefthand;
        localaudio.SoundPlay();
    }

    public void PlayOrangeSmashRight()
    {
        soundData.clip = OrangeHandSmash;
        localaudio.Emitter = Righthand;
        localaudio.SoundPlay();
    }

    public void PlayOrangeJump()
    {
        soundData.clip = OrangeJump;
        localaudio.Emitter = Body;
        localaudio.SoundPlay();
    }

    public void PlayOrangeLanding()
    {
        soundData.clip = OrangeLanding;
        localaudio.Emitter = Leftfoot;
        localaudio.SoundPlay();
    }

    public void PlayOrangeHurt()
    {
        soundData.clip = OrangeHurt;
        localaudio.Emitter = Head;
        localaudio.SoundPlay();
    }

    public void PlayOrangeDeath()
    {
        soundData.clip = OrangeDeath;
        localaudio.Emitter = Body;
        localaudio.SoundPlay();
    }
}
