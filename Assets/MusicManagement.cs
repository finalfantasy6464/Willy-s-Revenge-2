using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManagement : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip OverworldMusic;

    AudioClip CurrentMusic;

    [HideInInspector] public UnityEvent onLevelStart;
    [HideInInspector] public UnityEvent onOverworld;

    public List<AudioClip> bgm = new List<AudioClip>();

    private void Awake()
    {
        onLevelStart = new UnityEvent();
        onLevelStart.AddListener(MusicCheck);
        onOverworld.AddListener(PlayOverworldMusic);
    }

    private void MusicCheck()
    {
        if(CurrentMusic == null || CurrentMusic != GetFromBuildIndex())
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
        CurrentMusic = bgm[SceneManager.GetActiveScene().buildIndex];
    }

    private AudioClip GetFromBuildIndex()
    {
        return bgm[SceneManager.GetActiveScene().buildIndex];
    }

    public void PlayOverworldMusic()
    {
        musicSource.clip = OverworldMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
