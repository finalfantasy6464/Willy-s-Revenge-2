using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    public ScriptablePlayerSettings settings;
    const string MUSIC_VOLUME = "MusicVolume";
    const string SOUND_VOLUME = "SFXVolume";
    // public Slider bgmSlider;
    // public Slider sfxSlider;

    float bgmValue;
    float sfxValue;

    public void SetBGM(Slider bgmSlider)
    {
        bgmValue = bgmSlider.value;
        settings.bgmVolume = bgmValue;
        settings.mixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(bgmValue) * 20);
        FadeAudioGroup.volumecache = Mathf.Log10(bgmValue) * 20;
    }

    public void SetSFX(Slider sfxSlider)
    {
        sfxValue = sfxSlider.value;
        settings.sfxVolume = sfxValue;
        settings.mixer.SetFloat(SOUND_VOLUME, Mathf.Log10(sfxValue) * 20);
    }
}
