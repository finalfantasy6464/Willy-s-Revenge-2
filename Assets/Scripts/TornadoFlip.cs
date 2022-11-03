using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoFlip : MonoBehaviour
{
    public float fliptime;
    public float flipcounter;

    SpriteRenderer spriteRenderer;
    Sprite sprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        flipcounter += Time.deltaTime;
        if(flipcounter >= fliptime)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            flipcounter = 0;
        }
    }
}
