using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOneAudio : MonoBehaviour
{

    public AudioSource source;
    public AudioClip WallHit;

    public EnemyMovement Enemymov;

   

    // Start is called before the first frame update
    void Start()
    {
        Enemymov.onWallHit.AddListener(() => PlayClip(WallHit));
    }

    void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    private void OnDisable()
    {
        Enemymov.onWallHit.RemoveListener(() => PlayClip(WallHit));
    }
}
