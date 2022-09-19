using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public static class FadeAudioGroup
{
    public static float volumecache;

    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        volumecache = currentVol;
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);


        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator EndFade(AudioMixer audioMixer, string exposedParam, float duration)
    {
        float currentTime = 0;
        float currentVol;

        audioMixer.GetFloat(exposedParam, out currentVol);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, volumecache, currentTime / duration);
            audioMixer.SetFloat(exposedParam, newVol);
            yield return null;
        }
        yield break;
    }
}
