using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionalSFX : MonoBehaviour
{
    public Transform player;
    public AudioSource source;
    public AudioClip clip;
    public float minVolume;
    public float maxVolume;
    public float minDistance;
    public float maxDistance;

    public bool playsOnStart;
    public bool looping;

    public float currentDistance;
    public float distanceProgress;

    public void PlayPositionalSound()
    {
        if (looping)
        {
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            player = GameObject.Find("Character").GetComponent<Character>().transform;
        }
        else
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>().transform;
        }
        source.loop = looping;
        source.clip = clip;

        if (playsOnStart)
        {
            PlayPositionalSound();
        }
    }

    void Update()
    {
        if(player == null || !source.isPlaying) return;

        currentDistance = Vector2.Distance(transform.position, player.transform.position);
        distanceProgress = Mathf.InverseLerp(maxDistance, minDistance, currentDistance);
        source.volume = Mathf.Lerp(minVolume, maxVolume, distanceProgress);
    }
}
