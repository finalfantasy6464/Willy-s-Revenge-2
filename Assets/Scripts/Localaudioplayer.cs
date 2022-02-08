using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAudioPlayer : MonoBehaviour
{
    public Transform emitter;

    public PositionalSoundData soundData;

    private void Start()
    {
        if(emitter == null)
        {
            emitter = transform;
        }
    }

    // Start is called before the first frame update
    public AudioSource SoundPlay()
    {
        if(this.gameObject != null)
        {
            return GameSoundManagement.instance.PlayPositional(soundData, emitter.position);
        }
        return null;
    }

    public AudioSource SoundStop()
    {
        if (this.gameObject != null)
        {
            return GameSoundManagement.instance.PlayPositional(soundData, emitter.position);
        }
        return null;
    }
}

