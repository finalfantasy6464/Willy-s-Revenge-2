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
    public AudioSource SoundPlay()
    {
        if(this.gameObject != null)
        {
            return GameSoundManagement.instance.PlayPositional(soundData, Emitter.position);
        }
        return null;
    }

    public AudioSource SoundStop()
    {
        if (this.gameObject != null)
        {
            return GameSoundManagement.instance.PlayPositional(soundData, Emitter.position);
        }
        return null;
    }
}

