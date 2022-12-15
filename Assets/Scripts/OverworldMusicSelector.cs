using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMusicSelector : MonoBehaviour
{
    public AudioSource[] sources;
    public AudioClip currentClip;

    public ScriptablePlayerSettings settings;

    public int currentProgress;

    public void Start()
    {
        overworldmusicCheck();
    }

    public void overworldmusicCheck()
    {
        foreach (AudioSource source in sources)
        {
            StartCoroutine(musicFadeOut(source));
        }
        if (currentProgress == 0 || currentProgress == 1)
            SetOverworldMusic(sources[0]);
        else if (currentProgress == 2 || currentProgress == 3)
            SetOverworldMusic(sources[1]);
        else if (currentProgress == 4)
            SetOverworldMusic(sources[2]);
        else if (currentProgress == 5)
            SetOverworldMusic(sources[3]);
        else
        {
            SetOverworldMusic(sources[4]);
        }
    }

    private void SetOverworldMusic(AudioSource source)
    {
        currentClip = source.clip;
            StartCoroutine(musicFadeIn(source));
    }

    IEnumerator musicFadeOut(AudioSource source)
    {
        if(source.volume == 0)
        {
            yield break;
        }

        while (source.volume > 0)
        {
            source.volume -= Mathf.Max(Time.deltaTime / 2f, 0);
            yield return 0;
        }
        yield break;
    }

    IEnumerator musicFadeIn(AudioSource source)
    {
        if(source.volume == 1)
        {
            yield break;
        }

        while (source.volume < 1)
        {
            source.volume += Mathf.Min(Time.deltaTime / 2f, 1);
            yield return 0;

            if (source.volume == 1)
            {
                source.volume = 1;
                yield break;
            }
        }
    }
}
