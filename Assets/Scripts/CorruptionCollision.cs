using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionCollision : MonoBehaviour
{
    PlayerCollision playercoll;

    private void Start()
    {
        playercoll = GameObject.FindObjectOfType<PlayerCollision>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.CompareTag("Tail") && playercoll.canbehit == false)
        {
            hit.GetComponent<Animator>().Play("SegmentCorruption");
        }
    }
}
