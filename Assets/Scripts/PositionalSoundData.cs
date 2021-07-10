using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class PositionalSoundData
{
    public float minDistance;
    public float maxDistance;

    public float minVolume;
    public float maxVolume;

    public float minPitch;
    public float maxPitch;

    public float distanceProgress;

    public AudioClip clip;

    public float VolumeUpdate(float currentDistance)
    {
        distanceProgress = Mathf.InverseLerp(maxDistance, minDistance, currentDistance);
        return Mathf.Lerp(minVolume, maxVolume, distanceProgress);
    }
}
