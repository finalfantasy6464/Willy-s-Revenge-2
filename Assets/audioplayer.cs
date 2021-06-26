using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioplayer : MonoBehaviour
{
	public AudioClip smash;

    // Start is called before the first frame update
    void SoundPlay()
    {
		GameSoundManagement.instance.PlayOneShot (smash);
    }

}
