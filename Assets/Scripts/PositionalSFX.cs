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
    public float panAmount;

    public bool playsOnStart;
    public bool looping;

    public float currentDistance;
    public float horizontalDistance;
    public float distanceProgress;

    private const float panThreshold = 0.72f;

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
        horizontalDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceProgress = Mathf.InverseLerp(maxDistance, minDistance, currentDistance);

        if (transform.position.x > player.transform.position.x + panThreshold)
        {
            panAmount = Mathf.Lerp(0.0f, 0.8f, (horizontalDistance + panThreshold) / maxDistance);
        }

        else if (transform.position.x < player.transform.position.x - panThreshold)
        {
            panAmount = Mathf.Lerp(0.0f, -0.8f, (horizontalDistance + panThreshold) / maxDistance);
        }

        else panAmount = 0;

        source.volume = Mathf.Lerp(minVolume, maxVolume, distanceProgress);
        source.volume = source.volume * Mathf.Lerp(0.5f, 1.0f, (maxVolume - minVolume) / maxVolume);

        source.panStereo = panAmount;
    }
}
