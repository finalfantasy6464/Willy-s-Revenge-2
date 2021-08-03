using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip pelletGet;
    public AudioClip goldenpelletGet;
    public AudioClip keyget;
    public AudioClip enemyhit;
    public AudioClip gatehit;
    public AudioClip lavaburn;
    public AudioClip falling;
    public AudioClip shock;
    public AudioClip shieldPickup;

    public PlayerController Playercontrol;
    public PlayerCollision Playercoll;

    float vollowrange = 0.8f;
    float volhighrange = 1.0f;

    float pitchlowrange = 0.85f;
    float pitchhighrange = 1.15f;

    void Start()
    { 
        Playercontrol.onEatPellet.AddListener(() => PlayClip(pelletGet, true, true, false));
        Playercontrol.onGoldenPellet.AddListener(() => PlayClip(goldenpelletGet, true, true, false));
        Playercontrol.onCollectShield.AddListener(() => PlayClip(shieldPickup,true));
        Playercoll.onKeyCollect.AddListener(() => PlayClip(keyget, true));
        Playercoll.onWallCollide.AddListener(() => PlayClip(gatehit, true));
        Playercoll.onLavaBurn.AddListener(() => PlayClip(lavaburn, true));
        Playercoll.onFalling.AddListener(() => PlayClip(falling, true));
        Playercoll.onElectricHit.AddListener(() => PlayClip(shock, true));

    }


    void PlayClip(AudioClip clip, bool persistent)
    {
        PlayClip(clip, false, false, persistent);
    }

    void PlayClip(AudioClip clip, bool randompitch, bool randomvolume, bool persistent)
    {
        float vol = source.volume;

        if (randompitch)
        {
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
        }

        if (randomvolume)
        {
            vol = Random.Range(vollowrange, volhighrange);
        }

        if (persistent)
        {
            GameSoundManagement.instance.efxSource.volume = vol;
            GameSoundManagement.instance.PlayOneShot(clip);
        }else
        {
            source.PlayOneShot(clip, vol);
        }
    }

    void OnDisable()
    {
        Playercontrol.onEatPellet.RemoveListener(() => PlayClip(pelletGet, true, true, false));
        Playercontrol.onGoldenPellet.RemoveListener(() => PlayClip(goldenpelletGet, true, true, false));
        Playercoll.onKeyCollect.RemoveListener(() => PlayClip(keyget, true));
        Playercoll.onWallCollide.RemoveListener(() => PlayClip(gatehit, true));
        Playercoll.onLavaBurn.RemoveListener(() => PlayClip(lavaburn, true));
        Playercoll.onFalling.RemoveListener(() => PlayClip(falling, true));
        Playercoll.onElectricHit.RemoveListener(() => PlayClip(shock, true));
        Playercontrol.onCollectShield.RemoveListener(() => PlayClip(shieldPickup, true));
    }


}
