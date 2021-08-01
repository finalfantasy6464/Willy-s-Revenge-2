using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip WallHit;

    public PositionalSFX sfx;

    public EnemyMovementTwo Enemymov;



    // Start is called before the first frame update
    void Start()
    {
        Enemymov.onWallHit.AddListener(() => PlayClip(WallHit));
    }

    void PlayClip(AudioClip clip)
    {
        sfx.PlayPositionalSound();
    }

    private void OnDisable()
    {
        Enemymov.onWallHit.RemoveListener(() => PlayClip(WallHit));
    }
}
