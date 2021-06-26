using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localaudioplayer : MonoBehaviour
{
    public Transform Emitter;

    public PositionalSoundData soundData;

    private void Start()
    {
        if(Emitter == null)
        {
            Emitter = transform;
        }
    }

    // Start is called before the first frame update
    void SoundPlay()
    {
        GameSoundManagement.instance.PlayPositional(soundData, Emitter.position);
           //source.clip = smash;
           //source.PlayOneShot(smash);
    }

}

