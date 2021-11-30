using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    public ScriptablePlayerSettings settings;
    public Slider bgmSlider;
    public Slider sfxSlider;

    float bgmValue;
    float sfxValue;

    public void SetBGM()
    {
        bgmValue = bgmSlider.value;
        settings.bgmVolume = bgmValue;
    }

    public void SetSFX()
    {
        sfxValue = sfxSlider.value;
        settings.sfxVolume = sfxValue;
    }
}
