using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionTransparency : MonoBehaviour
{
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            sprite.color = new Color(1, 1, 1, 0.6f);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}
