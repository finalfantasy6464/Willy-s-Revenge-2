using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class MusicManagement : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioMixer audioMixer;
    public Slider slider;

    public const string MUSIC_VOLUME = "MusicVolume";

    AudioClip CurrentMusic;

    [HideInInspector] public UnityEvent onLevelStart;
    [HideInInspector] public UnityEvent onOverworld;
    [HideInInspector] public UnityEvent onMainMenu;
    [HideInInspector] public UnityEvent onCredits;
    [HideInInspector] public UnityEvent onArena;

    public List<AudioClip> musicClips = new List<AudioClip>();

    private void Awake()
    {
        onLevelStart = new UnityEvent();
    }

    public void Start()
    {
        onLevelStart.AddListener(MusicCheck);
        if(SceneManager.GetActiveScene().name == "Overworld")
            slider = GameObject.FindGameObjectWithTag("musicSlider").GetComponent<Slider>();

        MusicCheck();
    }

    private void MusicCheck()
    {
        if(musicSource == null)
        {
            musicSource = GetComponents<AudioSource>()[1]; // 0 is SFX
        }

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

       if(index == 0)
        {
            return musicClips[37];
        }
       if(index == 1 || index == 2)
        {
            return musicClips[0];
        }
  
        if (index >= 3 & index < 6)
        {
            return musicClips[1];
        }

        if (index >= 6 & index < 11)
        {
            return musicClips[2];
        }

        if (index >= 11 && index < 16)
        {
            return musicClips[3];
        }
        if (index >= 16 && index < 21)
        {
            return musicClips[4];
        }
        if (index >= 21 && index < 26)
        {
            return musicClips[5];
        }
        if (index >= 26 && index < 31)
        {
            return musicClips[6];
        }
        if (index >= 31 && index < 36)
        {
            return musicClips[7];
        }
        if (index >= 36 && index < 41)
        {
            return musicClips[8];
        }
        if (index >= 41 && index < 46)
        {
            return musicClips[9];
        }
        if (index >= 46 && index < 51)
        {
            return musicClips[10];
        }
        if (index >= 51 && index < 56)
        {
            return musicClips[11];
        }
        if (index >= 56 && index < 61)
        {
            return musicClips[12];
        }
        if (index >= 61 && index < 66)
        {
            return musicClips[13];
        }
        if (index >= 66 && index < 71)
        {
            return musicClips[14];
        }
        if (index >= 71 && index < 76)
        {
            return musicClips[15];
        }
        if (index >= 76 && index < 81)
        {
            return musicClips[16];
        }
        if (index >= 81 && index < 86)
        {
            return musicClips[17];
        }
        if (index >= 86 && index < 91)
        {
            return musicClips[18];
        }

        if (index == 91)
        {
            return musicClips[19];
        }
        if (index == 92)
        {
            return musicClips[20];
        }
        if (index == 93)
        {
            return musicClips[21];
        }
        if (index == 94)
        {
            return musicClips[22];
        }
        if (index == 95)
        {
            return musicClips[23];
        }
        if (index == 96)
        {
            return musicClips[24];
        }
        if (index == 97)
        {
            return musicClips[25];
        }
        if (index == 98)
        {
            return musicClips[26];
        }
        if (index == 99)
        {
            return musicClips[27];
        }

        if (index == 100)
        {
            return musicClips[28];
        }

        if (index == 101)
        {
            return musicClips[31];
        }

        if (index == 102)
        {
            return musicClips[29];
        }

        if (index == 103)
        {
            return musicClips[30];
        }

        return null;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void OnDisable()
    {
        onLevelStart.RemoveListener(MusicCheck);
    }
}