using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionEnterChecker : MonoBehaviour
{
    PlayerCollision playerCollision;
    SpriteRenderer myRenderer;

    void Start()
    {
        playerCollision = transform.parent.GetComponentInChildren<PlayerCollision>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Corruption"))
        {
            StartCoroutine(playerCollision.CorruptionDeathRoutine(myRenderer));
        }
    }
}
