using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localaudioplayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip smash;

    // Start is called before the first frame update
    void SoundPlay()
    {
           source.clip = smash;
           source.PlayOneShot(smash);
    }

}

