using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Tail"))
        {
            hit.GetComponent<Animator>().Play("SegmentJump");
        }
    }
}
