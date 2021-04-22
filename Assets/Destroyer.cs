using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = collision.gameObject;

        if(hit.tag == "Enemy" || hit.tag == "Enemy3")
        {
            Destroy(hit);
        }
    }
}
