using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousCloud : MonoBehaviour
{
    public AudioClip Cloud1;
    public AudioClip Cloud2;
    public AudioClip Cloud3;
    public AudioClip Cloud4;

    public Localaudioplayer localaudio;

    PositionalSoundData soundData;

    public void Start()
    {
        soundData = localaudio.soundData;
    }
    public void PlayCloud1()
    {
        soundData.clip = Cloud1;
        localaudio.SoundPlay();
    }

    public void PlayCloud2()
    {
        soundData.clip = Cloud2;
        localaudio.SoundPlay();
    }

    public void PlayCloud3()
    {
        soundData.clip = Cloud3;
        localaudio.SoundPlay();
    }

    public void PlayCloud4()
    {
        soundData.clip = Cloud4;
        localaudio.SoundPlay();
    }
}
