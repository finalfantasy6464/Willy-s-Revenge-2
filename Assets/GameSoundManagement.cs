using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSoundManagement : MonoBehaviour
{

	public AudioSource efxSource;

    public AudioMixer audioMixer;

    public Slider slider;

    float sliderValue;
    float logvolume;

	public static GameSoundManagement instance = null;

	public float lowPitchRange = 0.85f;
	public float highPitchRange = 1.15f;


    const string SOUND_VOLUME = "SFXVolume";

    void Awake ()
	{

		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

    public void Start()
    {
        slider.value = PlayerPrefs.GetFloat(SOUND_VOLUME);
        audioMixer.SetFloat(SOUND_VOLUME, Mathf.Log10(slider.value) * 20);
    }

    public void SetLevel()
    {
        logvolume = Mathf.Log10(slider.value) * 20;
        sliderValue = slider.value;
        audioMixer.SetFloat(SOUND_VOLUME, logvolume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, sliderValue);
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
