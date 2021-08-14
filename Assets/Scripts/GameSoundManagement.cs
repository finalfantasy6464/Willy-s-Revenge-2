using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSoundManagement : MonoBehaviour
{

    public AudioSource efxSource;

    public AudioSource[] sources;

    List<PositionalSoundData> soundDataList = new List<PositionalSoundData>();

    public AudioMixer audioMixer;

    public Slider slider;

    float sliderValue;
    float logvolume;

    public float currentDistance;
    public float distanceProgress;

    public static GameSoundManagement instance = null;

    public float lowPitchRange = 0.85f;
    public float highPitchRange = 1.15f;

    private Transform player;


    const string SOUND_VOLUME = "SFXVolume";

    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (AudioSource source in sources)
        {
            soundDataList.Add(null);
        }
    }

    public void Start()
    {
        if(slider != null)
        {
            slider.value = PlayerPrefs.GetFloat(SOUND_VOLUME);
            audioMixer.SetFloat(SOUND_VOLUME, Mathf.Log10(slider.value) * 20);
        }

        if (SceneManager.GetActiveScene().name != "Overworld" && SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Credits")
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void Update()
    {

      for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying) continue;

            if(player != null)
            {
                sources[i].volume = soundDataList[i].VolumeUpdate(Vector2.Distance(sources[i].transform.position, player.transform.position));
                sources[i].pitch = Random.Range(soundDataList[i].minPitch, soundDataList[i].maxPitch);
            }     
        }  
    }

    public void PlayerCheck()
    {
        if (SceneManager.GetActiveScene().name == "Overworld" && SceneManager.GetActiveScene().isLoaded)
        {
            player = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>().transform;
        }

        else
        {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform;
        }
    }


    public void SetLevel()
    {
        logvolume = Mathf.Log10(slider.value) * 20;
        sliderValue = slider.value;
        audioMixer.SetFloat(SOUND_VOLUME, logvolume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, sliderValue);
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public AudioSource PlayPositional(PositionalSoundData soundData, Vector2 objectposition)
    {
        AudioSource source = default(AudioSource);         
            
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying)
            {
               source = sources[i];
               source.volume = 0.0f;
               soundDataList[i] = soundData;
                break;
            }
        }

            PlayerCheck();

        source.transform.position = objectposition;

        source.clip = soundData.clip;
        source.Play();
        return source;
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
