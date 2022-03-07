using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionOptions : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown fullscreenDropdown;

    Resolution[] resolutions;
    List<Resolution> options;

    [HideInInspector] public int currentFullscreenIndex;
    [HideInInspector] public int currentResolutionIndex;
    const string RESOLUTION_OPTION = "ResolutionOption";
    const string FULLSCREEN_OPTION = "FullscreenOption";

    public ScriptablePlayerSettings settings;


    void Awake()
    {
        resolutions = Screen.resolutions;
        options = new List<Resolution>();
    }

    void Start()
    {
        resolutionDropdown.ClearOptions();

        List<string> optionlabels = new List<string>();

        Vector2 size;
        int hz;
        float ratio;
        for (int i = 0; i < resolutions.Length; i++)
        {
            size = new Vector2(resolutions[i].width, resolutions[i].height);
            hz = resolutions[i].refreshRate;
            ratio = (float)resolutions[i].width / (float)resolutions[i].height;
            if (hz != 60 || ratio < 1.7f || ratio > 1.8f)
            {
                continue;
            }
            options.Add(resolutions[i]);
            optionlabels.Add(size.x + " x " + size.y + " (" + hz + "Hz)");
        }
        for (int i = 0; i < options.Count; i++)
        {
            size = new Vector2(options[i].width, options[i].height);
            if (size.x == settings.resolutionWidth
                    && size.y == settings.resolutionHeight)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(optionlabels);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetFullscreen(settings.displayModeIndex);
        SetResolution(currentResolutionIndex);
    }

    public void SetResolution(int index)
    {
        currentResolutionIndex = index;
        resolutionDropdown.value = index;
        Resolution res = options[index];

        if (settings.resolutionWidth != res.width)
        {
            settings.resolutionWidth = res.width;
            settings.resolutionHeight = res.height;
        }

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void SetResolution(int width, int height)
    {
        if(width == settings.resolutionWidth || height == settings.resolutionHeight)
        {
            return;
        }

        Screen.SetResolution(width, height, (FullScreenMode)settings.displayModeIndex);
        settings.resolutionHeight = height;
        settings.resolutionWidth = width;

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].width == width
                    && options[i].height == height)
            {
                currentResolutionIndex = i;
                return;
            }
        }
        currentResolutionIndex = 0;
    }

    public void SetResolutionFromSettings()
    {
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].width == settings.resolutionWidth
                    && options[i].height == settings.resolutionHeight)
            {
                currentResolutionIndex = i;
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
                return;
            }
        }
    }

    public void SetFullscreen(int index)
    {
        currentFullscreenIndex = index;
        fullscreenDropdown.value = index;
        settings.displayModeIndex = index;

        if (index == 0)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else if (index == 1)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else if (index == 2)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else if (index == 3)
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }
}