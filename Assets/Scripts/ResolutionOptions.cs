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
            if (size.x == Screen.currentResolution.width
                    && size.y == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(optionlabels);
        currentResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_OPTION);
        currentFullscreenIndex = PlayerPrefs.GetInt(FULLSCREEN_OPTION);
        SetFullscreen(currentFullscreenIndex);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        currentResolutionIndex = index;
        resolutionDropdown.value = index;
        Resolution res = options[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt(RESOLUTION_OPTION, currentResolutionIndex);
    }

    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].width == width
                    && options[i].height == height)
            {
                currentResolutionIndex = i;
                PlayerPrefs.SetInt(RESOLUTION_OPTION, currentResolutionIndex);
                return;
            }
        }
        currentResolutionIndex = 0;
        PlayerPrefs.SetInt(RESOLUTION_OPTION, currentResolutionIndex);
    }

    public void SetFullscreen(int index)
    {
        currentFullscreenIndex = index;
        fullscreenDropdown.value = index;
        if (index == 0)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else if (index == 1)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else if (index == 2)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else if (index == 3)
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        PlayerPrefs.SetInt(FULLSCREEN_OPTION, currentFullscreenIndex);
    }
}