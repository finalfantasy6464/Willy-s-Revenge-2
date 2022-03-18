using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionSegmentChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<SpriteRenderer>().enabled = false;
    }
}
