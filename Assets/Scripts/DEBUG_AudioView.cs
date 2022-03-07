using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class DEBUG_AudioView : MonoBehaviour
{
    public TextMeshProUGUI sfxLabel;
    public TextMeshProUGUI bgmLabel;
    public TextMeshProUGUI masterLabel;
    public TextMeshProUGUI sourceLabel;
    public AudioSource bgmSource;
    public AudioMixer mixer;

    float sfx;
    float bgm;
    float master;

    
    void Start()
    {
        
    }

    void Update()
    {

        if(bgmSource == null)
        {
            AudioSource[] allsourcesfuckeverything = FindObjectsOfType<AudioSource>();

            foreach (AudioSource source in allsourcesfuckeverything)
            {
                if(source.outputAudioMixerGroup.name == "BGM")
                    bgmSource = source;
            }
        }
        
        mixer.GetFloat("SFXVolume", out sfx);
        mixer.GetFloat("MusicVolume", out bgm);
        mixer.GetFloat("MasterVolume", out master);

        sfxLabel.SetText(sfx.ToString());
        bgmLabel.SetText(bgm.ToString());
        masterLabel.SetText(master.ToString());
        sourceLabel.SetText(bgmSource.isPlaying.ToString());
    }

    public void Button()
    {
        string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        string fullPath = $"file://{ appData }/../LocalLow/Marshall Inc/Willy's Revenge 2";
        Debug.Log(fullPath);
        Application.OpenURL(fullPath);
    }
}
