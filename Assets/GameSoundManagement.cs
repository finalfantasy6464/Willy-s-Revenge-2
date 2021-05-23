using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManagement : MonoBehaviour
{

	public AudioSource efxSource;

	public static GameSoundManagement instance = null;

	public float lowPitchRange = 0.85f;
	public float highPitchRange = 1.15f;

	void Awake ()
	{

		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle (AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void PlayOneShot (AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.PlayOneShot (clip);
	}

	public void RandomizeSFX (params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}
}
