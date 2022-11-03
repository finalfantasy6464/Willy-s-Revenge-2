using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorbelt : MonoBehaviour
{
    public Sprite[] ConveyorSprites;

    private float Conveyortimer = 0.0f;
    public float Conveyorstep = 0.075f;
    public int mysprite = 0;

    private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = spriteRenderer.sprite;
    }

    void Update()
    { 
            this.Conveyortimer += Time.smoothDeltaTime;

        if (Conveyortimer >= Conveyorstep)
        {
            Conveyortimer = 0.0f;
            spritechange();
        }
    }

    private void spritechange()
    {
        mysprite++;
        if(mysprite == ConveyorSprites.Length)
        {
            mysprite = 0;
        }
        currentSprite = ConveyorSprites[mysprite];
        spriteRenderer.sprite = currentSprite;
    }
}
