using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Audio;

[CreateAssetMenu]
public class ScriptablePlayerSettings : ScriptableObject
{
    static string SETTINGS_PATH;
    public float sfxVolume;
    public float bgmVolume;

    public float refreshRate;
    public int resolutionWidth;
    public int resolutionHeight;
    public int displayModeIndex; // FullScreenMode
    public AudioMixer mixer;

    void OnEnable()
    {
        SETTINGS_PATH = Application.persistentDataPath + "/config.ini";
    }

    public void SetVolume(float sfxVolume, float bgmVolume)
    {
        SetSFX(sfxVolume);
        SetBGM(bgmVolume);
    }

    public void SetSFX(float value)
    {
        sfxVolume = value;
    }

    public void SetBGM(float value)
    {
        bgmVolume = value;
    }

    public void SetDisplaySettings(Resolution resolution, FullScreenMode displayMode, float refreshRate = -1)
    {
        if(refreshRate != -1)
            this.refreshRate = refreshRate;

        resolutionWidth = resolution.width;
        resolutionHeight = resolution.height;
        displayModeIndex = (int)displayMode;
    }

    public void SaveToDisk()
    {
        string[] saveLines = new string[]
        {
            $"SFX_VOLUME = {sfxVolume.ToString()}",
            $"BGM_VOLUME = {bgmVolume.ToString()}",
            $"REFRESHRATE = {refreshRate.ToString()}",
            $"RESOLUTION_WIDTH = {resolutionWidth.ToString()}",
            $"RESOLUTION_HEIGHT = {resolutionHeight.ToString()}",
            $"FULLSCREENMODE = {displayModeIndex.ToString()}",
        };

        File.WriteAllLines(SETTINGS_PATH, saveLines);
    }

    public void CreateNew()
    {
        string[] saveLines = new string[]
        {
            $"SFX_VOLUME = 0.2",
            $"BGM_VOLUME = 0.7",
            $"REFRESHRATE = 60",
            $"RESOLUTION_WIDTH = 1280",
            $"RESOLUTION_HEIGHT = 720",
            $"FULLSCREENMODE = 0",
        };

        File.WriteAllLines(SETTINGS_PATH, saveLines);
    }

    public bool TryLoadFromDisk()
    {
        if(!File.Exists(SETTINGS_PATH))
            return false;

        string[] loadLines = File.ReadAllLines(SETTINGS_PATH);
        
        sfxVolume = float.Parse(TrimFromLine(loadLines[0]));
        bgmVolume = float.Parse(TrimFromLine(loadLines[1]));
        refreshRate = float.Parse(TrimFromLine(loadLines[2]));
        resolutionWidth = int.Parse(TrimFromLine(loadLines[3]));
        resolutionHeight = int.Parse(TrimFromLine(loadLines[4]));
        displayModeIndex = int.Parse(TrimFromLine(loadLines[5]));
        return true;
    }

    string TrimFromLine(string s) => (s.Split('=')[1]).TrimStart();
}
