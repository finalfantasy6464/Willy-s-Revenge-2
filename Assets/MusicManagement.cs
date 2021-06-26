using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManagement : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip OverworldMusic;
    public AudioClip MenuMusic;
    public AudioMixer audioMixer;
    public Slider slider;

    float sliderValue;
    float logvolume;

    const string MUSIC_VOLUME = "MusicVolume";

    AudioClip CurrentMusic;

    [HideInInspector] public UnityEvent onLevelStart;
    [HideInInspector] public UnityEvent onOverworld;
    [HideInInspector] public UnityEvent onMainMenu;

    public List<AudioClip> musicClips = new List<AudioClip>();

    private void Awake()
    {
        onLevelStart = new UnityEvent();
        onLevelStart.AddListener(MusicCheck);
        onOverworld.AddListener(PlayOverworldMusic);
        onMainMenu.AddListener(PlayMenuMusic);
    }

    public void Start()
    {
        if(slider != null)
        {
            slider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME);
            audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(slider.value) * 20);
        }
    }

    public void SetLevel()
    {
        logvolume = Mathf.Log10(slider.value) * 20;
        sliderValue = slider.value;
        audioMixer.SetFloat(MUSIC_VOLUME, logvolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, sliderValue);
    }


    private void MusicCheck()
    {
        if (CurrentMusic == null || CurrentMusic != GetFromBuildIndex())
        {
            SetFromBuildIndex();
            StopMusic();
            musicSource.clip = CurrentMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void SetFromBuildIndex()
    {
        CurrentMusic = GetFromBuildIndex();
    }

    private AudioClip GetFromBuildIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int rangeSize = 5;
        int upperRange = 1;

        while (upperRange <= 100)
        {
            if (index < upperRange)
                return musicClips[Mathf.CeilToInt(upperRange / rangeSize) - 1];
            upperRange += rangeSize;
        }
        return null;
    }

    public void PlayOverworldMusic()
    {
        musicSource.clip = OverworldMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = MenuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void OnDisable()
    {
        onLevelStart.RemoveListener(MusicCheck);
        onOverworld.RemoveListener(PlayOverworldMusic);
        onMainMenu.RemoveListener(PlayMenuMusic);
    }
}